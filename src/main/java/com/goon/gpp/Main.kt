package com.goon.gpp

import java.io.File
import javax.swing.JFileChooser
import javax.swing.JFrame
import javax.swing.JOptionPane
import javax.swing.WindowConstants
import javax.swing.filechooser.FileSystemView
import kotlin.system.exitProcess

fun main(args: Array<String>) {
    val file = if (args.isEmpty()) {
        val mainFrame = MainFrame(FileSystemView.getFileSystemView().homeDirectory)
        parseFile(opener) ?: exitProcess(-1)
    } else if (File(args[0]).isDirectory) {
        val mainFrame = MainFrame(args[0])
        parseFile(opener) ?: exitProcess(-1)
    } else {
        File(args[0])
    }
}
