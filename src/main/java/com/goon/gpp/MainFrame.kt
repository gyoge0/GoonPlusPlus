package com.goon.gpp

import java.awt.*
import java.io.File
import javax.sound.sampled.AudioInputStream
import javax.sound.sampled.AudioSystem
import javax.swing.JEditorPane
import javax.swing.JFileChooser
import javax.swing.JFrame
import javax.swing.JOptionPane
import javax.swing.filechooser.FileSystemView
import kotlin.system.exitProcess


class MainFrame() : JFrame() {

    private var editor = JEditorPane()
    private val fileButton = Button("File")
    private val editButton = Button("Edit")
    private var nameLabel: Label

    init {
        this.defaultCloseOperation = EXIT_ON_CLOSE

        val pane = this.contentPane
        this.layout = GridBagLayout()
        val gbc = GridBagConstraints()

        val openFile = openFile()
        editor.isEditable = openFile.canWrite()
        nameLabel = Label(String.format("Goon++    |     Editing: %s", openFile.name), Label.CENTER)

        if (openFile.canRead()) {
            editor.text = openFile.readText()
        } else {
            JOptionPane.showMessageDialog(this, "File is not readable.\nOpen a new file via File -> Open", "Error", JOptionPane.ERROR_MESSAGE)
            editor.isEditable = false
            editor.text = ""
        }

        editor.font = Font("JetBrains Mono", Font.PLAIN, 13)

        pane.layout = GridBagLayout()


        gbc.weightx = 0.0
        gbc.weighty = 0.0
        gbc.fill = GridBagConstraints.NONE
        gbc.gridwidth = 1
        gbc.gridx = 0
        gbc.gridy = 0
        pane.add(fileButton, gbc)

        gbc.fill = GridBagConstraints.NONE
        gbc.gridwidth = 1
        gbc.gridx = 1
        gbc.gridy = 0
        pane.add(editButton, gbc)

        gbc.weightx = 1.0
        gbc.fill = GridBagConstraints.NONE
        gbc.gridwidth = 1
        gbc.gridx = 2
        gbc.gridy = 0
        gbc.fill = GridBagConstraints.HORIZONTAL
        pane.add(nameLabel, gbc)

        gbc.weighty = 1.0
        gbc.fill = GridBagConstraints.BOTH
        gbc.gridwidth = 5
        gbc.gridx = 0
        gbc.gridy = 1
        pane.add(editor, gbc)

        this.pack()
        this.isVisible = true
    }

    fun setFile(file: File) {
        editor.text = file.readText()
        nameLabel.text = String.format("Goon++    |     Editing: %s", file.name)
    }

    fun openFile() {
        return file
    }

    fun parseFile(opener: JFileChooser): File? {
        when (opener.showOpenDialog(null)) {
            JFileChooser.APPROVE_OPTION -> {
                val file = opener.selectedFile
                if (file.isFile) {
                    return file
                } else {
                    JOptionPane.showMessageDialog(
                        null,
                        "Something went wrong!",
                        "Error",
                        JOptionPane.INFORMATION_MESSAGE
                    )
                }
            }
            JFileChooser.CANCEL_OPTION -> {
                println("Canceled")
                JOptionPane.showMessageDialog(
                    null,
                    "Operation Cancelled",
                    "Operation Cancelled",
                    JOptionPane.INFORMATION_MESSAGE
                )
                return null
            }
            JFileChooser.ERROR_OPTION -> {
                JOptionPane.showMessageDialog(
                    null,
                    "Something went wrong!",
                    "Error",
                    JOptionPane.INFORMATION_MESSAGE
                )
                return null
            }
        }
        return null
    }


    @Synchronized
    fun playSound(path: String) {
        Thread {
            try {
                val clip = AudioSystem.getClip()
                val inputStream: AudioInputStream = AudioSystem.getAudioInputStream(
                    this.javaClass.getResourceAsStream(path)
                )
                clip.open(inputStream)
                clip.start()
            } catch (e: Exception) {
                System.err.println(e.message)
            }
        }.start()
    }

}