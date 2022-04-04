package com.gyoge.gpp

import kotlinx.serialization.Contextual
import kotlinx.serialization.SerialName
import kotlinx.serialization.Serializable

@Serializable
data class Config(
    @SerialName("Font Name") var fontName: ConfigPair<String> = ConfigPair("Comic Sans MS"),
    @SerialName("Font Size") var fontSize: ConfigPair<Int> = ConfigPair(12),
    @SerialName("Font Color") var fontColor: ConfigPair<Int> = ConfigPair(0x000000),
    @SerialName("Background Color") var backgroundColor: ConfigPair<Int> = ConfigPair(0xFFFFFF),
)

@Serializable
data class ConfigPair<T : Any>(
    @SerialName("v") @Contextual val value: T,
    @SerialName("t") val type: String = value::class.simpleName!!
)