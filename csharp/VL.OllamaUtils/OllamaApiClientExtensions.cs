// For examples, see:
// https://thegraybook.vvvv.org/reference/extending/writing-nodes.html#examples

using System;
using System.Net;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using OllamaSharp;
using OllamaSharp.Models;


/// <summary>
/// This is a class with extension methods for the OllamaApiClient class.
/// This is a way to add new methods to existing classes, without modifying the original class.
/// This is useful when you don't have access to the original source code, or when you want to keep the original class clean.
/// This class is static, so it can't be instantiated, and its methods can be called directly on the class.
/// Extension methods can be used like regular methods on the original class, but they are defined in a separate class.
/// This class is in the same namespace as the OllamaApiClient class, so it can access its public members.
/// </summary>
public static class OllamaApiClientExtensions
{

    /// <summary>
    /// This is an extension method for the DeleteModelAsync method of the OllamaApiClient class.
    /// It takes a model name as a parameter and deletes the model.
    /// </summary>
    /// <param name="client">The OllamaApiClient instance.</param>
    /// <param name="model">The model name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public static Task DeleteModel(IOllamaApiClient client, string model, CancellationToken cancellationToken = default)
    {
        return Task.Run(async () =>
        {
            await client.DeleteModelAsync(model, cancellationToken);
        }, cancellationToken);
    }

    /// <summary>
    /// This is an extension method for the PullModelAsync method of the OllamaApiClient class.
    /// It takes a model name as a parameter and writes the status to the console.
    /// </summary>
    /// <param name="client">The OllamaApiClient instance.</param>
    /// <param name="model">The model name.</param>
    /// <param name="onResponse">The action to perform on each response.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>  
    public static Task<List<PullModelResponse?>> PullModel(IOllamaApiClient client, string model, Action<PullModelResponse?> onResponse, CancellationToken cancellationToken = default)
    {
        return Task.Run(async () =>
        {
            var responses = new List<PullModelResponse?>();

            // Call the PullModelAsync method and await its completion
            await foreach (var response in client.PullModelAsync(model, cancellationToken))
            {
                onResponse(response);
                responses.Add(response);
            }
            return responses;
        }, cancellationToken);
    }

    /// <summary>
    /// This is an extension method for the SendAsync method of the Chat class.
    /// It takes a message, an action to perform on each response, and an optional cancellation token as parameters.
    /// It returns a Task of a list of string responses.
    /// </summary>
    /// <typeparam name="T">The type parameter.</typeparam>
    /// <param name="chat">The Chat instance.</param>
    /// <param name="message">The message to send.</param>
    /// <param name="onResponse">The action to perform on each response.</param>
    /// 
    /// <param name="base64Image">The base64 encoded images.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A Task of a list of string responses.</returns>
    public static Task<List<string>> SendAsync(this Chat chat, string message, Action<string?> onResponse, IEnumerable<string> base64Image, CancellationToken cancellationToken = default)
    {
        return Task.Run(async () =>
        {
            var responses = new List<string>();

            await foreach (var response in chat.SendAsync(message, base64Image, cancellationToken))
            {
                onResponse(response);
                responses.Add(response);
            }
            return responses;
        }, cancellationToken);
    }
}