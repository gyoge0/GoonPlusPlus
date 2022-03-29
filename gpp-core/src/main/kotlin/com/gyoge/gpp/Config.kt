package com.gyoge.gpp

import kotlinx.serialization.Serializable

@Serializable
data class Config(
    @PrettyName("Font Name") var fontName: String = "Comic Sans MS",
    @PrettyName("Font Size") var fontSize: Int = 12,
    @PrettyName("Font Color") var fontColor: Int = 0x000000,
    @PrettyName("Background Color") var backgroundColor: Int = 0xFFFFFF,
)

@Target(AnnotationTarget.FIELD)

annotation class PrettyName(val name: String)