package com.goon.gpp

import java.awt.Font
import java.io.File
import javax.swing.JFileChooser
import javax.swing.JOptionPane
import javax.swing.JTextArea
import javax.swing.filechooser.FileSystemView

class EditorTab(filePath: String?) {

    var editor = JTextArea()
        private set
    var name: String
        private set
    var isUntitled: Boolean = true
        private set
    lateinit var file: File
        @JvmName("directSetFile")
        private set

    @Throws(NoFileException::class)
    fun setFile(filePath: String?): Boolean {
        var opener: JFileChooser


        if (filePath == null) {
            // no file path passed int
            opener = JFileChooser(FileSystemView.getFileSystemView().homeDirectory)
        } else if (File(filePath).exists() && !File(filePath).isDirectory) {
            // Filepath exists, set file to file at filepath
            this.file = File(filePath)
            return true
        } else if (File(filePath).isDirectory) {
            // directory
            opener = JFileChooser(File(filePath))
        } else {
            // something else
            opener = JFileChooser(FileSystemView.getFileSystemView().homeDirectory)
        }

        when (opener.showOpenDialog(null)) {
            // file selected
            JFileChooser.APPROVE_OPTION -> {
                val selectedFile = opener.selectedFile
                return if (selectedFile.isFile) {
                    this.file = selectedFile
                    true
                } else {
                    // something is wrong
                    JOptionPane.showMessageDialog(
                        null,
                        "Something went wrong!",
                        "Error",
                        JOptionPane.INFORMATION_MESSAGE
                    )
                    false
                }
            }
            // cancelled
            JFileChooser.CANCEL_OPTION -> {
                JOptionPane.showMessageDialog(
                    null,
                    "Operation Cancelled",
                    "Operation Cancelled",
                    JOptionPane.INFORMATION_MESSAGE
                )
                return false
            }
            // error
            JFileChooser.ERROR_OPTION -> {
                JOptionPane.showMessageDialog(
                    null,
                    "Something went wrong!",
                    "Error",
                    JOptionPane.INFORMATION_MESSAGE
                )
                return false
            }
        }
        return false
    }

    init {
        if (this.setFile(filePath)) {
            editor.isEditable = file.canWrite()

            if (file.canRead()) {
                editor.text = file.readText()
            } else {
                JOptionPane.showMessageDialog(
                    null,
                    "File is not readable.\nOpen a new file via File -> Open",
                    "Error",
                    JOptionPane.ERROR_MESSAGE
                )
                editor.isEditable = false
                editor.text = "File is not readable.\\nOpen a new file via File -> Open"
            }
            name = file.name
            editor.name = file.name

            editor.font = Font("JetBrains Mono", Font.PLAIN, 13)
            editor.name = file.name
        } else {
            editor.isEditable = true
            editor.text = ""
            editor.name = "Untitled"
            editor.font = Font("JetBrains Mono", Font.PLAIN, 13)
            name = "Untitled"
            this.file = File("Untitled")
        }
    }

    class NoFileException : Exception()


}