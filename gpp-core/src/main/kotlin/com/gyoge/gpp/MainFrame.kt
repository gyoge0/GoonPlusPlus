package com.gyoge.gpp

import java.awt.*
import java.awt.event.MouseEvent
import java.io.File
import java.io.PrintWriter
import javax.sound.sampled.AudioInputStream
import javax.sound.sampled.AudioSystem
import javax.swing.*
import javax.swing.event.ChangeEvent
import javax.swing.event.ChangeListener
import javax.swing.plaf.LayerUI
import kotlin.system.exitProcess


@Suppress("JoinDeclarationAndAssignment")
class MainFrame(private val config: Config, startingDir: String = "~") : JFrame() {

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
        this.title = String.format("Goon++   |   v%s", VERSION)

        // Init a bunch of fields
        this.layout = GridBagLayout()
        editorTabBar = JTabbedPane()
        editorTabBar.tabLayoutPolicy = JTabbedPane.SCROLL_TAB_LAYOUT

        val startTab = EditorTab(config)
        startTab.realTab(startingDir)
        if (startTab.isUntitled) {
            startTab.untitledTab()
        }
        editorTabBar.addTab(startTab.name + "  ", startTab.editor)
        editorTabs.add(startTab)
        currentTabIdx = 0


        setFile(startTab.file)

        nameLabel.alignment = Label.CENTER


        initMenu()
        initLayout()
        initListeners()

        this.pack()
        this.isVisible = true
    }

    /**
     * Corrects the name label when a file is opened.
     */
    private fun setFile(file: File) {
        nameLabel.text = String.format("Goon++    |     Editing: %s", file.name)
    }


    /**
     * Listener that sets the file when the tab is changed.
     */
    private inner class TabChangedListener : ChangeListener {
        override fun stateChanged(e: ChangeEvent) {
            currentTabIdx = editorTabBar.selectedIndex
        }
    }

    /**
     * Initializes the menu bar.
     */
    private fun initMenu() {
        var menu: JMenu
        var menuItem: JMenuItem

        menu = JMenu("File")

        menuItem = JMenuItem("Open")
        menuItem.addActionListener {
            val newTab = EditorTab(config)

            // If there are no tabs open
            if (currentTabIdx != -1) {
                newTab.realTab(editorTabs[currentTabIdx].file.parent)
            } else {
                newTab.realTab("~")
            }

            if (!newTab.isUntitled) {
                editorTabBar.addTab(newTab.name + "  ", newTab.editor)
                editorTabs.add(newTab)
                currentTabIdx = editorTabBar.tabCount - 1
            }
        }
        menu.add(menuItem)

        menuItem = JMenuItem("Save")
        menuItem.addActionListener {
            if (editorTabs[currentTabIdx].isUntitled) {
                JOptionPane.showMessageDialog(
                    this,
                    "You must \"Save As\" to set a file location first!",
                    "Error",
                    JOptionPane.ERROR_MESSAGE
                )
            } else {
                val prw = PrintWriter(editorTabs[currentTabIdx].file)
                for (line in editorTabs[currentTabIdx].textPane.text.lines()) {
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
            for (line in currentTab.textPane.text.lines()) {
                prw.println(line)
            }
            prw.close()
            setFile(currentTab.file)
        }
        menu.add(menuItem)

        menuItem = JMenuItem("New")
        menuItem.addActionListener {
            val newTab = EditorTab(config)
            newTab.untitledTab()

            editorTabBar.addTab(newTab.name + "  ", newTab.editor)
            editorTabs.add(newTab)
            currentTabIdx = editorTabBar.tabCount - 1
        }
        menu.add(menuItem)

        menuBar.add(menu)

        menu = JMenu("Window")

        menuItem = JMenuItem("Preferences")
        menu.add(menuItem)

        menuItem = JMenuItem("Exit")
        menuItem.addActionListener {
            exitProcess(0)
        }
        menu.add(menuItem)

        menuBar.add(menu)
    }

    /**
     * Initializes the layout with all the elements.
     */
    private fun initLayout() {
        val pane = this.contentPane
        var gbc: GridBagConstraints

        gbc = GridBagConstraints()
        gbc.weightx = 0.0
        gbc.weighty = 0.0
        gbc.fill = GridBagConstraints.NONE
        gbc.gridwidth = 1
        gbc.gridx = 0
        gbc.gridy = 0
        pane.add(menuBar, gbc)

        gbc = GridBagConstraints()
        gbc.weightx = 1.0
        gbc.weighty = 0.0
        gbc.fill = GridBagConstraints.HORIZONTAL
        gbc.gridwidth = 1
        gbc.gridx = 2
        gbc.gridy = 0
        pane.add(nameLabel, gbc)


        gbc = GridBagConstraints()
        gbc.weighty = 1.0
        gbc.fill = GridBagConstraints.BOTH
        gbc.gridwidth = 5
        gbc.gridx = 0
        gbc.gridy = 1

        pane.add(JLayer(editorTabBar, CloseableTabbedPaneLayerUI()), gbc)

    }

    /**
     * Adds all the required listeners to components.
     */
    private fun initListeners() {
        editorTabBar.addChangeListener(TabChangedListener())
    }

    /**
     * Plays the sound at the path specified.
     */
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

    companion object {
        /**
         * Version number.
         */
        const val VERSION = "0.1"
    }

    /**
     * TabbedPane that allows the user to close tabs.
     *
     * @see <a href="http://www.java2s.com/Tutorials/Java/Swing_How_to/JTabbedPane/Create_Closeable_JTabbedPane_alignment_of_the_close_button.htm">Source</a>
     */
    inner class CloseableTabbedPaneLayerUI : LayerUI<JTabbedPane>() {
        private var p = JPanel()
        private var pt = Point(-100, -100)
        private var button = MyCloseButton()
        override fun paint(g: Graphics, c: JComponent) {
            super.paint(g, c)
            if (c !is JLayer<*>) {
                return
            }
            val tabPane = c.view as JTabbedPane
            for (i in 0 until tabPane.tabCount) {
                val rect = tabPane.getBoundsAt(i)
                val d = button.preferredSize
                val x = rect.x + rect.width - d.width + 2
                val y = rect.y + (rect.height - d.height) / 2
                val r = Rectangle(x, y, d.width, d.height)
                button.foreground = if (r.contains(pt)) Color.RED else Color.BLACK
                SwingUtilities.paintComponent(g, button, p, r)
            }
        }

        override fun installUI(c: JComponent) {
            super.installUI(c)
            (c as JLayer<*>).layerEventMask = (AWTEvent.MOUSE_EVENT_MASK
                    or AWTEvent.MOUSE_MOTION_EVENT_MASK)
        }

        override fun uninstallUI(c: JComponent) {
            (c as JLayer<*>).layerEventMask = 0
            super.uninstallUI(c)
        }

        override fun processMouseEvent(e: MouseEvent, l: JLayer<out JTabbedPane>) {
            if (e.id != MouseEvent.MOUSE_CLICKED) {
                return
            }
            pt.location = e.point
            val tabbedPane = l.view as JTabbedPane
            val index = tabbedPane.indexAtLocation(pt.x, pt.y)
            if (index >= 0) {
                val rect = tabbedPane.getBoundsAt(index)
                val d = button.preferredSize
                val x = rect.x + rect.width - d.width - 2
                val y = rect.y + (rect.height - d.height) / 2
                val r = Rectangle(x, y, d.width, d.height)
                if (r.contains(pt)) {
                    tabbedPane.removeTabAt(index)
                    editorTabs.removeAt(index)
                }
            }
            l.view.repaint()
        }

        override fun processMouseMotionEvent(
            e: MouseEvent,
            l: JLayer<out JTabbedPane>
        ) {
            pt.location = e.point
            val tabbedPane = l.view as JTabbedPane
            val index = tabbedPane.indexAtLocation(pt.x, pt.y)
            if (index >= 0) {
                tabbedPane.repaint(tabbedPane.getBoundsAt(index))
            } else {
                tabbedPane.repaint()
            }
        }

        inner class MyCloseButton : JButton("X") {
            init {
                border = BorderFactory.createEmptyBorder()
                isFocusPainted = false
                isBorderPainted = false
                isContentAreaFilled = false
                isRolloverEnabled = false
            }

            override fun getPreferredSize(): Dimension {
                return Dimension(16, 16)
            }
        }
    }

}