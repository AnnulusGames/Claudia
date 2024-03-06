﻿using System.Text.Json.Serialization;
using System.Text.Json;
using System.Collections.ObjectModel;

namespace Claudia;

// https://docs.anthropic.com/claude/reference/messages_post
public class MessageRequest
{
    /// <summary>
    /// The model that will complete your prompt.
    /// </summary>
    [JsonPropertyName("model")]
    public required string Model { get; init; }

    /// <summary>
    /// The maximum number of tokens to generate before stopping.
    /// Note that our models may stop before reaching this maximum.This parameter only specifies the absolute maximum number of tokens to generate.
    /// Different models have different maximum values for this parameter
    /// </summary>
    [JsonPropertyName("max_tokens")]
    public required int MaxTokens { get; init; }

    /// <summary>
    /// Input messages.
    /// </summary>
    [JsonPropertyName("messages")]
    public required Message[] Messages { get; init; }

    // optional parameters

    /// <summary>
    /// System prompt.
    /// A system prompt is a way of providing context and instructions to Claude, such as specifying a particular goal or role.
    /// </summary>
    [JsonPropertyName("system")]
    public string? System { get; init; }

    /// <summary>
    /// An object describing metadata about the request.
    /// </summary>
    [JsonPropertyName("metadata")]
    public Metadata? Metadata { get; init; }

    /// <summary>
    /// Custom text sequences that will cause the model to stop generating.
    /// Our models will normally stop when they have naturally completed their turn, which will result in a response stop_reason of "end_turn".
    /// If you want the model to stop generating when it encounters custom strings of text, you can use the stop_sequences parameter.If the model encounters one of the custom sequences, the response stop_reason value will be "stop_sequence" and the response stop_sequence value will contain the matched stop sequence.
    /// </summary>
    [JsonPropertyName("stop_sequences")]
    public string[]? StopSequences { get; init; }

    /// <summary>
    /// Whether to incrementally stream the response using server-sent events.
    /// </summary>
    [JsonPropertyName("stream")]
    public bool? Stream { get; init; }

    /// <summary>
    /// Amount of randomness injected into the response.
    /// Defaults to 1.0. Ranges from 0.0 to 1.0. Use temperature closer to 0.0 for analytical / multiple choice, and closer to 1.0 for creative and generative tasks.
    /// Note that even with temperature of 0.0, the results will not be fully deterministic.
    /// </summary>
    [JsonPropertyName("temperature")]
    public int? Temperature { get; init; }

    /// <summary>
    /// Use nucleus sampling.
    /// In nucleus sampling, we compute the cumulative distribution over all the options for each subsequent token in decreasing probability order and cut it off once it reaches a particular probability specified by top_p.You should either alter temperature or top_p, but not both.
    /// Recommended for advanced use cases only. You usually only need to use temperature.
    /// </summary>
    [JsonPropertyName("top_p")]
    public int? TopP { get; init; }

    /// <summary>
    /// Only sample from the top K options for each subsequent token.
    /// Used to remove "long tail" low probability responses.
    /// Recommended for advanced use cases only. You usually only need to use temperature.
    /// </summary>
    [JsonPropertyName("top_k")]
    public int? TopK { get; init; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, Anthropic.DefaultJsonSerializerOptions);
    }
}
public class Message
{
    /// <summary>
    /// user or assistant.
    /// </summary>
    [JsonPropertyName("role")]
    public required string Role { get; init; }

    /// <summary>
    /// single string or an array of content blocks.
    /// </summary>
    [JsonPropertyName("content")]
    public required Contents Content { get; init; }
}

public class Contents : Collection<Content>
{
    public static implicit operator Contents(string text)
    {
        var content = new Content
        {
            Type = ContentTypes.Text,
            Text = text
        };
        return new Contents { content };
    }
}

public record class Content
{
    [JsonPropertyName("type")]
    public required string? Type { get; init; }

    // Text or Source

    [JsonPropertyName("text")]
    public string? Text { get; init; }

    [JsonPropertyName("source")]
    public string? Source { get; init; }

    public static implicit operator Content(string text) => new Content
    {
        Type = ContentTypes.Text,
        Text = text
    };
}

public record class Metadata
{
    /// <summary>
    /// An external identifier for the user who is associated with the request.
    /// This should be a uuid, hash value, or other opaque identifier.Anthropic may use this id to help detect abuse. Do not include any identifying information such as name, email address, or phone number.
    /// </summary>
    [JsonPropertyName("user_id")]
    public required string UserId { get; init; }
}

public record class Source
{
    /// <summary>
    /// We currently support the base64 source type for images.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; init; } = "base64";

    /// <summary>
    /// We currently support the base64 source the image/jpeg, image/png, image/gif, and image/webp media types.
    /// </summary>
    [JsonPropertyName("image/jpeg")]
    public required string MediaType { get; init; }

    [JsonPropertyName("data")]
    public required byte[] Data { get; init; } // Base64
}