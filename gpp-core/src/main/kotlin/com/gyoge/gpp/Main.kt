package com.gyoge.gpp

import kotlinx.serialization.decodeFromString
import kotlinx.serialization.json.Json
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

    if (!configDir.exists()) {
        configDir.mkdir()
        configFile.createNewFile()
        config = Config()
        configFile.writeText(format.encodeToJsonElement(config).toString())
    } else if (!configFile.exists()) {
        configFile.createNewFile()
        config = Config()
        configFile.writeText(format.encodeToJsonElement(config).toString())
    } else {
        config = format.decodeFromString(configFile.readText())
    }

    println("config = $config")


    if (args.isEmpty()) {
        val mf = MainFrame(config)
    } else {
        val mf = MainFrame(config, args[0])
    }
}