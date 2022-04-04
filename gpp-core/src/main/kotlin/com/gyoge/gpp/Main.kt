package com.gyoge.gpp

import kotlinx.serialization.decodeFromString
import kotlinx.serialization.json.Json
import kotlinx.serialization.json.JsonElement
import kotlinx.serialization.json.encodeToJsonElement
import java.io.File
import javax.swing.UIManager


val format = Json {
    encodeDefaults = true
    ignoreUnknownKeys = true
    prettyPrint = true
}

fun main(args: Array<String>) {
    UIManager.setLookAndFeel(UIManager.getSystemLookAndFeelClassName())

    val configDir = File("${System.getProperty("user.home")}/.gpp")
    val configFile = File("${configDir.absolutePath}/config.json")
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
        configJson = format.encodeToJsonElement(format.decodeFromString<Config>(configFile.readText()))
    }


    if (args.isEmpty()) {
        val mf = MainFrame(configJson)
    } else {
        val mf = MainFrame(configJson, args[0])
    }
}