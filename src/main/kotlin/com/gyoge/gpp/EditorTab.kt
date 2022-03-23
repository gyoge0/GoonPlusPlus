package com.gyoge.gpp

import java.awt.Font
import java.io.File
import javax.swing.JFileChooser
import javax.swing.JOptionPane
import javax.swing.JScrollPane
import javax.swing.JTextArea
import javax.swing.filechooser.FileSystemView

open class EditorTab {

    var textArea = JTextArea()
        private set
    var editor = JScrollPane(textArea)
        private set
    var name: String = ""
        private set
    var isUntitled: Boolean = true
        private set
    lateinit var file: File
        /* Private setter for "direct" access. Others should go through the file chooser. */
        @JvmName("directSetFile")
        private set

    fun setFile(filePath: String?): Boolean {
        val opener: JFileChooser


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

    fun realTab(filePath: String) {
        if (this.setFile(filePath)) {
            textArea.isEditable = file.canWrite()

            if (file.canRead()) {
                textArea.text = file.readText()
            } else {
                JOptionPane.showMessageDialog(
                    null,
                    "File is not readable.\nOpen a new file via File -> Open",
                    "Error",
                    JOptionPane.ERROR_MESSAGE
                )
                textArea.isEditable = false
                textArea.text = "File is not readable.\\nOpen a new file via File -> Open"
            }
            name = file.name
            textArea.name = file.name

            textArea.font = Font("JetBrains Mono", Font.PLAIN, 13)
            textArea.name = file.name
            this.isUntitled = false
            editor = JScrollPane(textArea)
        } else {
            this.isUntitled = true
        }
    }

    fun untitledTab() {
        textArea.isEditable = true
        textArea.text = ""
        this.isUntitled = true
        textArea.name = "Untitled"
        textArea.font = Font("JetBrains Mono", Font.PLAIN, 13)
        name = "Untitled"
        this.file = File("Untitled")
        editor = JScrollPane(textArea)
    }

}