package com.gyoge.gpp

import java.awt.Color
import java.awt.Font
import java.io.File
import javax.swing.*
import javax.swing.filechooser.FileSystemView
import com.gyoge.gpp.nowrap.*
import kotlinx.serialization.json.*


open class EditorTab(config: JsonElement) {

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
    private val config = config.jsonObject

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
                // No wrap
                this.textPane.editorKit = WrapEditorKit()

                textPane.text = file.readText()
            } else {
                JOptionPane.showMessageDialog(
                    null,
                    "File is not readable.\nOpen a new file via File -> Open",
                    "Error",
                    JOptionPane.ERROR_MESSAGE
                )
                // No wrap
                this.textPane.editorKit = WrapEditorKit()

                textPane.isEditable = false
                textPane.text = "File is not readable.\\nOpen a new file via File -> Open"
            }
            name = file.name
            textPane.name = file.name



            textPane.font = Font(
                jsonGet("Font Name").toString(), Font.PLAIN, 13
            )
            textPane.name = file.name
            this.isUntitled = false
            editor = JScrollPane(textPane)
        } else {
            this.isUntitled = true
        }


        this.setStyles()
    }

    fun untitledTab() {
        textPane.isEditable = true
        textPane.text = ""
        this.isUntitled = true
        textPane.name = "Untitled"
        textPane.font = Font(
            jsonGet("Font Name").toString(),
            Font.PLAIN,
            jsonGet("Font Size").int
        )
        name = "Untitled"
        this.file = File("Untitled")
        editor = JScrollPane(textPane)

        this.setStyles()
    }


    private fun setStyles() {

        // Font
        this.textPane.font = Font(
            jsonGet("Font Name").toString(),
            Font.PLAIN,
            jsonGet("Font Size").jsonPrimitive.int
        )

        // Colors
        this.textPane.foreground = Color(jsonGet("Font Color").int)
        this.textPane.background =
            Color(jsonGet("Background Color").int)

    }

    private fun jsonGet(key: String) = config[key]!!.jsonObject["v"]!!.jsonPrimitive

}