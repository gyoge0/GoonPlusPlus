package com.gyoge.gpp

import com.gyoge.gpp.filters.AnyFilter
import com.gyoge.gpp.filters.BooleanFilter
import com.gyoge.gpp.filters.DoubleFilter
import com.gyoge.gpp.filters.IntFilter
import kotlinx.serialization.json.JsonElement
import kotlinx.serialization.json.JsonObject
import kotlinx.serialization.json.jsonObject
import java.awt.Dimension
import java.awt.GridBagConstraints
import java.awt.GridBagLayout
import javax.swing.JFrame
import javax.swing.JTextField
import javax.swing.text.PlainDocument

class PreferencesFrame(configElem: JsonElement) : JFrame() {

    private val config: Map<String, *> = configElem.jsonObject

    init {
        this.defaultCloseOperation = DISPOSE_ON_CLOSE
        this.title = String.format("Goon++ :  Preferences   |   v%s", MainFrame.VERSION)
        this.layout = GridBagLayout()

        var row = -1

        val gbc = GridBagConstraints()
        gbc.weightx = 1.0
        gbc.fill = GridBagConstraints.HORIZONTAL
        gbc.gridwidth = 1
        gbc.gridy = 0


        for ((k, v) in config) {
            v as JsonObject

            val name = JTextField()
            name.text = k
            name.horizontalAlignment = JTextField.CENTER
            name.isEditable = false
            gbc.gridx = 0
            this.add(name, gbc)

            val value = JTextField()
            value.text = v["v"].toString()

            val doc = value.document as PlainDocument
            println("v[\"t\"] = ${v["t"]}")

            doc.documentFilter = when (v["t"]!!.toString()) {
                "\"String\"" -> {
                    // Remove the quotes
                    val vString = v["v"].toString()
                    value.text = vString.substring(1, vString.length - 1)

                    AnyFilter()
                }
                "\"Double\"" -> {
                    DoubleFilter()
                }
                "\"Int\"" -> {
                    IntFilter()
                }
                "\"Boolean\"" -> {
                    BooleanFilter()
                }
                else -> {
                    AnyFilter()
                }
            }

            gbc.gridx = 1
            gbc.gridy = ++row
            value.horizontalAlignment = JTextField.CENTER
            value.isEditable = true

            this.add(value, gbc)
        }

        gbc.gridx = 2
        gbc.gridy = 0

        this.pack()
        this.isLocationByPlatform = true
        this.isVisible = true

        // Setting size of the window
        // This would be at the top of the method, but it doesn't work for some reason
        this.isResizable = true
        this.size = Dimension(400, 400)
        this.isResizable = true

        this.requestFocus()

    }
}