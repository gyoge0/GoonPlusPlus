package com.gyoge.gpp

import javax.swing.JFrame
import javax.swing.JPanel

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
        this.isResizable = false
        this.requestFocus()

    }
}