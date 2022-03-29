package com.gyoge.gpp

import java.awt.Font
import java.io.File
import javax.swing.JFileChooser
import javax.swing.JOptionPane
import javax.swing.JScrollPane
import javax.swing.JTextArea
import javax.swing.filechooser.FileSystemView


open class EditorTab(private val config: Config) {

    var textPane = JTextPane()
        private set
    var editor = JScrollPane(textPane)
        private set
    var name: String = ""
        private set
    var isUntitled: Boolean = true
        private set
    var file: File = File("")
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
            textPane.isEditable = file.canWrite()

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
            textPane.name = file.name

            textPane.font = Font("JetBrains Mono", Font.PLAIN, 13)
            textPane.name = file.name
            this.isUntitled = false
            editor = JScrollPane(textPane)
        } else {
            this.isUntitled = true
        }
    }

    fun untitledTab() {
        textPane.isEditable = true
        textPane.text = ""
        this.isUntitled = true
        textPane.name = "Untitled"
        textPane.font = Font("JetBrains Mono", Font.PLAIN, 13)
        name = "Untitled"
        this.file = File("Untitled")
        editor = JScrollPane(textArea)
    }

}