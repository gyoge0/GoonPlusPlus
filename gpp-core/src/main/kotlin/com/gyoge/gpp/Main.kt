package com.gyoge.gpp

import kotlinx.serialization.decodeFromString
import kotlinx.serialization.encodeToString
import kotlinx.serialization.json.Json
import kotlinx.serialization.json.JsonElement
import kotlinx.serialization.json.encodeToJsonElement
import java.io.File
import javax.swing.UIManager


val format = Json {
    encodeDefaults = true
    ignoreUnknownKeys = true
    prettyPrint = true
    coerceInputValues = true
}

val configDir = File("${System.getProperty("user.home")}/.gpp")
val configFile = File("${configDir.absolutePath}/config.json")

fun main(args: Array<String>) {
    UIManager.setLookAndFeel(UIManager.getSystemLookAndFeelClassName())

    val config: Config
    val configJson: JsonElement

    if (!configDir.exists()) {
        configDir.mkdir()
        configFile.createNewFile()
        config = Config()
        configJson = format.encodeToJsonElement(config)
        configFile.writeText(configJson.toString())
    } else if (!configFile.exists()) {
        configFile.createNewFile()
        config = Config()
        configJson = format.encodeToJsonElement(config)
        configFile.writeText(configJson.toString())
    } else {
        configJson =
            format.encodeToJsonElement(format.decodeFromString<Config>(configFile.readText()))
    }


    var mf = if (args.isEmpty()) {
        MainFrame(ConfigWrapper(configJson))
    } else {
        MainFrame(ConfigWrapper(configJson), args[0])
    }
}

fun saveConfig(config: ConfigWrapper) {
    configFile.writeText(format.encodeToString(config.json))
}