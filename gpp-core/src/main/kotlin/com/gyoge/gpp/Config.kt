package com.gyoge.gpp

import kotlinx.serialization.Serializable

@Serializable
data class Config(
    var fontName: String = "Comic Sans MS",
    var fontSize: Int = 12,
    var fontColor: Int = 0x000000,
    var backgroundColor: Int = 0xFFFFFF,
)