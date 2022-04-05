package com.gyoge.gpp

import com.gyoge.gpp.nowrap.WrapEditorKit
import kotlinx.serialization.json.int
import kotlinx.serialization.json.jsonObject
import kotlinx.serialization.json.jsonPrimitive
import java.awt.Color
import java.awt.Font
import java.io.File
import javax.swing.JFileChooser
import javax.swing.JOptionPane
import javax.swing.JScrollPane
import javax.swing.JTextPane
import javax.swing.filechooser.FileSystemView


/**
 * Represents a tab in the editor.
 */
open class EditorTab(private val config: ConfigWrapper) {

    /** The editing pane of the tab. */
    var textPane = JTextPane()
        private set

    /** Wraps the [EditorTab.textPane] in a scrollable UI. */
    var editor = JScrollPane(textPane)
        private set

    /** Name of the tab. */
    var name: String = ""
        private set

    /** Flag if the tab is untitled or not. */
    var isUntitled: Boolean = true
        private set

    /** The file being edited. */
    var file: File = File("")
        /* Private setter for "direct" access. Others should go through the file chooser. */
        @JvmName("directSetFile")
        private set

    /**
     * Initializes a new tab with a file path.
     *
     * @return if the tab was successfully created.
     */
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

    /** Sets up the tab if it is a real file. */
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

    /** Sets up the file if it is an untitled file. */
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


    /** Sets up the styles of the tab by accessing [EditorTab.config] through [EditorTab.jsonGet]. */
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

    /** Helper function to get values from [EditorTab.config]. */
    private fun jsonGet(key: String) = config.json.jsonObject[key]!!.jsonObject["v"]!!.jsonPrimitive

}