using System;
using System.Net;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using OllamaSharp;
using OllamaSharp.Models;
using OllamaSharp.Models.Chat;


/// <summary>
/// Provides extension methods for OllamaApiClient.
/// </summary>
public static class OllamaApiClientExtensions
{

    /// <summary>
    /// Extension method for DeleteModelAsync, deletes a model.
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
    /// Extension method for PullModelAsync, prints status.
    /// </summary>
    /// <param name="client">The OllamaApiClient instance.</param>
    /// <param name="model">The model name.</param>
    /// <param name="onResponse">The action to perform on each responsestream.</param>
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
    /// It takes a message, an action to perform on each responsestream, and an optional cancellation token as parameters.
    /// It returns a Task of a list of string responses.
    /// </summary>
    /// <typeparam name="T">The type parameter.</typeparam>
    /// <param name="chat">The Chat instance.</param>
    /// <param name="message">The message to send.</param>
    /// <param name="onResponse">The action to perform on each responsestream.</param>
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


    /// <summary>
    /// This is an extension method for the ChatAsync method of the OllamaApiClient class.
    /// It takes a ChatRequest, an action to perform on each responsestream, and an optional cancellation token as parameters.
    /// It returns a Task of a list of string responses.
    /// </summary>
    /// <param name="client">The OllamaApiClient instance.</param>
    /// <param name="request">The ChatRequest instance.</param>
    /// <param name="onResponse">The action to perform on each responsestream.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A Task of a list of string responses.</returns>
    public static Task<List<string>> ChatAsync(IOllamaApiClient client, ChatRequest request, Action<string?> onResponse, CancellationToken cancellationToken = default)
    {
        return Task.Run(async () =>
        {
            var responses = new List<string>();
            await foreach (ChatResponseStream? response in client.ChatAsync(request, cancellationToken))
            {
                if (response?.Message != null && response.Message.Content != null)
                {
                    onResponse(response.Message.Content);
                    responses.Add(response.Message.Content);
                }
            }
            return responses;
        }, cancellationToken);
    }

    /// <summary>
    /// This is an extension method for the GenerateAsync method of the OllamaApiClient class.
    /// It takes a prompt, a conversation context, an action to perform on each response stream, and an optional cancellation token as parameters.
    /// It returns a Task of a list of string responses.
    /// </summary>
    /// <param name="client">The OllamaApiClient instance.</param>
    /// <param name="prompt">The prompt to generate a response for.</param>
    /// <param name="context">The conversation context.</param>
    /// <param name="onResponse">The action to perform on each response stream.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A Task of a list of string responses.</returns>
    public static Task<List<string>> GenerateAsync(IOllamaApiClient client, string prompt, ConversationContext context, Action<string?> onResponse, CancellationToken cancellationToken = default)
    {
        return Task.Run(async () =>
        {
            var responses = new List<string>();

            await foreach (var responsestream in client.GenerateAsync(prompt, context, cancellationToken))
            {
                if (responsestream?.Response != null)
                {
                    onResponse(responsestream.Response);
                    responses.Add(responsestream.Response);
                }
            }
            return responses;
        }, cancellationToken);
    }


    /// <summary>
    /// This is an extension method for the GenerateAsync method of the OllamaApiClient class.
    /// It takes a GenerateRequest, an action to perform on each response stream, and an optional cancellation token as parameters.
    /// It returns a Task of a list of string responses.
    /// </summary>
    /// <param name="client">The OllamaApiClient instance.</param>
    /// <param name="request">The GenerateRequest instance.</param>
    /// <param name="onResponse">The action to perform on each response stream.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A Task of a list of string responses.</returns>
    public static Task<List<string>> GenerateAsync(IOllamaApiClient client, GenerateRequest request, Action<string?> onResponse, CancellationToken cancellationToken = default)
    {
        return Task.Run(async () =>
        {
            var responses = new List<string>();

            await foreach (var responsestream in client.GenerateAsync(request, cancellationToken))
            {
                if (responsestream?.Response != null)
                {
                    onResponse(responsestream.Response);
                    responses.Add(responsestream.Response);
                }
            }
            return responses;
        }, cancellationToken);
    }

}