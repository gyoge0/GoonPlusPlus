package com.goon.gpp

import java.awt.GridBagConstraints
import java.awt.GridBagLayout
import java.awt.Label
import java.io.File
import java.io.PrintWriter
import javax.sound.sampled.AudioInputStream
import javax.sound.sampled.AudioSystem
import javax.swing.*


class MainFrame(startingDir: String? = null) : JFrame() {

    /** Label displaying the name of the current file. */
    private var nameLabel: Label = Label("")

    /** The menu bar at the top. */
    private var menuBar = JMenuBar()

    /** The tabbed pane with all the editor. */
    private var editorTabBar: JTabbedPane

    /** ArrayList to keep track of where the editors are in the TabbedPane. */
    private var editorTabs: ArrayList<EditorTab> = ArrayList()

    /** Index of the current tab in the TabbedPane in order to get the corresponding editor. */
    private var currentTabIdx: Int = 0

    /**
     * Initializes the gui.
     */
    init {
        this.defaultCloseOperation = EXIT_ON_CLOSE

        val pane = this.contentPane
        this.layout = GridBagLayout()
        val gbc = GridBagConstraints()
        editorTabBar = JTabbedPane()
        editorTabBar.tabLayoutPolicy = JTabbedPane.SCROLL_TAB_LAYOUT

        val startTab = EditorTab(startingDir)
        editorTabBar.addTab(startTab.name, startTab.editor)
        editorTabs.add(startTab)
        currentTabIdx = 0
        setFile(startTab.file)

        GridBagLayout().also { pane.layout = it }


        var menu: JMenu
        menu = JMenu("File")

        var menuItem: JMenuItem
        menuItem = JMenuItem("Open")
        menuItem.addActionListener {
            val newTab = EditorTab(editorTabs[currentTabIdx].file.parent)
            if (!newTab.isUntitled) {
                editorTabBar.addTab(newTab.name, newTab.editor)
                setFile(newTab.file)
                currentTabIdx = editorTabBar.tabCount - 1
            }
        }
        menu.add(menuItem)

        menuItem = JMenuItem("Save")
        menuItem.addActionListener {
            if (editorTabs[currentTabIdx].isUntitled) {
                JOptionPane.showMessageDialog(this, "You must \"Save As\" to set a file location first!", "Error", JOptionPane.ERROR_MESSAGE)
            } else {
                val prw = PrintWriter(editorTabs[currentTabIdx].file)
                for (line in startTab.editor.text.lines()) {
                    prw.println(line)
                }
                prw.close()
            }
        }
        menu.add(menuItem)

        menuItem = JMenuItem("Save As")
        menuItem.addActionListener {
            val currentTab = editorTabs[currentTabIdx]
            currentTab.setFile(currentTab.file.parent)
            val prw = PrintWriter(currentTab.file)
            for (line in currentTab.editor.text.lines()) {
                prw.println(line)
            }
            prw.close()
            setFile(currentTab.file)
        }

        menu.add(menuItem)

        menuBar.add(menu)


        gbc.weightx = 0.0
        gbc.weighty = 0.0
        gbc.fill = GridBagConstraints.NONE
        gbc.gridwidth = 1
        gbc.gridx = 0
        gbc.gridy = 0
        pane.add(menuBar, gbc)

        gbc.weightx = 1.0
        gbc.weighty = 0.0
        gbc.fill = GridBagConstraints.HORIZONTAL
        gbc.gridwidth = 1
        gbc.gridx = 2
        gbc.gridy = 0
        pane.add(nameLabel, gbc)


        gbc.weighty = 1.0
        gbc.fill = GridBagConstraints.BOTH
        gbc.gridwidth = 5
        gbc.gridx = 0
        gbc.gridy = 1
        pane.add(editorTabBar, gbc)



        this.pack()
        this.isVisible = true
    }

    /**
     * Corrects the name label when a file is opened.
     */
    private fun setFile(file: File) {
        nameLabel.text = String.format("Goon++    |     Editing: %s", file.name)
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