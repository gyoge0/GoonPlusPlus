package com.gyoge.gpp

import java.awt.BorderLayout
import java.awt.Dimension
import javax.swing.BoxLayout
import javax.swing.JFrame
import javax.swing.JLabel
import javax.swing.JPanel
import kotlin.reflect.full.memberProperties
import kotlin.reflect.jvm.javaField

class PreferencesFrame(private val config: Config) : JFrame() {
    init {
        val panel = JPanel()
        this.defaultCloseOperation = DISPOSE_ON_CLOSE
        this.title = String.format("Goon++ :  Preferences   |   v%s", MainFrame.VERSION)


        config.javaClass.declaredFields.forEach { f ->
            f.annotations.forEach { a ->
                if (a::class == PrettyName::class) {
                    a as PrettyName

                    // do stuff
                }
            }
        }

        this.add(panel)
        this.pack()
        this.isLocationByPlatform = true
        this.isVisible = true

        // Setting size of the window
        // This would be at the top of the method, but it doesn't work for some reason
        this.isResizable = true
        this.size = Dimension(400, 400)
        this.isResizable = false

        this.requestFocus()

    }
}