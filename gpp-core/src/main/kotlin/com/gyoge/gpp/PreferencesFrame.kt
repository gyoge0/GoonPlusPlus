package com.gyoge.gpp

import com.gyoge.gpp.filters.AnyFilter
import com.gyoge.gpp.filters.BooleanFilter
import com.gyoge.gpp.filters.DoubleFilter
import com.gyoge.gpp.filters.IntFilter
import java.awt.Dimension
import java.awt.GridBagConstraints
import java.awt.GridBagLayout
import javax.swing.BoxLayout
import javax.swing.JFrame
import javax.swing.JPanel
import javax.swing.JTextField
import javax.swing.text.PlainDocument
import kotlin.reflect.full.memberProperties
import kotlin.reflect.jvm.javaField


class PreferencesFrame(private val config: Config) : JFrame() {
    init {
        val panel = JPanel()
        this.defaultCloseOperation = DISPOSE_ON_CLOSE
        this.title = String.format("Goon++ :  Preferences   |   v%s", MainFrame.VERSION)
        this.layout = GridBagLayout()


        // Init a 2 column design
        val names = JPanel()
        val values = JPanel()
        names.layout = BoxLayout(names, BoxLayout.Y_AXIS)
        values.layout = BoxLayout(values, BoxLayout.Y_AXIS)



        config::class.memberProperties.forEach { p ->
            val javaField = p.javaField
            if (javaField != null && javaField.isAnnotationPresent(PrettyName::class.java)) {
                val name = JTextField()
                name.text = javaField.getAnnotation(PrettyName::class.java).name
                name.horizontalAlignment = JTextField.CENTER
                name.isEditable = false
                names.add(name)

                val value = JTextField()
                value.text = p.getter.call(config).toString()

                val doc = value.document as PlainDocument
                doc.documentFilter = when (javaField.type) {
                    String::class.java -> {
                        AnyFilter()
                    }
                    Double::class.java -> {
                        DoubleFilter()
                    }
                    Int::class.java -> {
                        IntFilter()
                    }
                    Boolean::class.java -> {
                        BooleanFilter()
                    }
                    else -> {
                        AnyFilter()
                    }
                }

                value.horizontalAlignment = JTextField.CENTER
                value.isEditable = true
                values.add(value)

            }
        }

        val gbc = GridBagConstraints()
        gbc.weightx = 1.0
        gbc.fill = GridBagConstraints.HORIZONTAL
        gbc.gridwidth = 1
        gbc.gridy = 0

        gbc.gridx = 0
        this.add(names, gbc)
        gbc.gridx = 1
        this.add(values, gbc)


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