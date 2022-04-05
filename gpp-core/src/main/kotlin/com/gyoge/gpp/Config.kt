package com.gyoge.gpp

import kotlinx.serialization.Contextual
import kotlinx.serialization.SerialName
import kotlinx.serialization.Serializable
import kotlinx.serialization.json.JsonElement

/**
 * Stores a configuration and its type in a single class (represented as a single JSON object)
 */
@Serializable
data class ConfigPair<T : Any>(
    @SerialName("v") @Contextual val value: T,
    @SerialName("t") val type: String = value::class.simpleName!!
)

/**
 * Stores the entire config of the main app in its fields as a collection of [ConfigPair]s.
 * Represented as a single JSON object, with "pretty" field names as keys, and [ConfigPair]s as values.
 */
@Serializable
data class Config(
    @SerialName("Font Name") var fontName: ConfigPair<String> = ConfigPair("Comic Sans MS"),
    @SerialName("Font Size") var fontSize: ConfigPair<Int> = ConfigPair(12),
    @SerialName("Font Color") var fontColor: ConfigPair<Int> = ConfigPair(0x000000),
    @SerialName("Background Color") var backgroundColor: ConfigPair<Int> = ConfigPair(0xFFFFFF),
)

/**
 * Wraps a [Config] object into a [JsonElement]. Classes with access to the config store a reference to the [ConfigWrapper],
 * and use the [ConfigWrapper.json] to access the config. This allows swapping out the config without other classes knowing.
 */
data class ConfigWrapper(
    var json: JsonElement
)
