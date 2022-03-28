package com.gyoge.gpp

import kotlinx.serialization.json.Json
import javax.swing.UIManager


val format = Json {
    encodeDefaults = true
    ignoreUnknownKeys = true
    prettyPrint = true
}

fun main(args: Array<String>) {
    UIManager.setLookAndFeel(UIManager.getSystemLookAndFeelClassName())

    val config = Config()

    if (args.isEmpty()) {
        val mf = MainFrame(config)
    } else {
        val mf = MainFrame(config, args[0])
    }
}