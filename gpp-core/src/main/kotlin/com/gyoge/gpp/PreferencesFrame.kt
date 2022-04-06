package com.gyoge.gpp

import com.gyoge.gpp.filters.AnyFilter
import com.gyoge.gpp.filters.BooleanFilter
import com.gyoge.gpp.filters.DoubleFilter
import com.gyoge.gpp.filters.IntFilter
import kotlinx.serialization.decodeFromString
import kotlinx.serialization.encodeToString
import kotlinx.serialization.json.*
import java.awt.Dimension
import java.awt.GridBagConstraints
import java.awt.GridBagLayout
import java.awt.event.ActionEvent
import java.awt.event.ActionListener
import javax.imageio.ImageIO
import javax.swing.JButton
import javax.swing.JFrame
import javax.swing.JPanel
import javax.swing.JTextField
import javax.swing.text.PlainDocument

/** A new JFrame to edit the preferences. */
class PreferencesFrame(private val configWrap: ConfigWrapper) : JFrame() {

    private val config = configWrap.json.jsonObject

    /** The panel storing preferences in [JTextField]s. */
    private val panel = JPanel()

    /** Places the config into the panel. */
    init {
        this.defaultCloseOperation = DISPOSE_ON_CLOSE
        this.title = String.format("Goon++ :  Preferences   |   v%s", MainFrame.VERSION)
        this.setIconImage(ImageIO.read(javaClass.getResourceAsStream("/icon.png")))
        panel.layout = GridBagLayout()

        var row = -1

        val gbc = GridBagConstraints()
        gbc.weightx = 1.0
        gbc.fill = GridBagConstraints.HORIZONTAL
        gbc.gridwidth = 1
        gbc.gridy = 0


        var name: JTextField
        for ((k, v) in config) {
            v as JsonObject

            gbc.gridy = ++row

            gbc.gridx = 0
            name = JTextField()
            name.text = k
            name.horizontalAlignment = JTextField.CENTER
            name.isEditable = false
            gbc.gridx = 0
            panel.add(name, gbc)

            val value = JTextField()
            value.text = v["v"].toString()

            val doc = value.document as PlainDocument

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
            value.horizontalAlignment = JTextField.CENTER
            value.isEditable = true
            panel.add(value, gbc)
        }

        val saveButton = JButton("Save")
        saveButton.addActionListener(SaveButtonListener())
        gbc.gridx = 1
        gbc.gridy = ++row
        gbc.fill = GridBagConstraints.NONE
        gbc.weightx = 0.0
        gbc.weighty = 0.0
        gbc.anchor = GridBagConstraints.EAST
        panel.add(saveButton, gbc)

        this.add(panel)
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

    /** Swaps out the config with the new one generated from the current inputs. */
    inner class SaveButtonListener : ActionListener {
        override fun actionPerformed(e: ActionEvent) {
            val newConfig: MutableMap<String, JsonElement> = HashMap()
            newConfig.putAll(config)


            for (i in 0..this@PreferencesFrame.panel.componentCount - 2 step 2) {
                val key = (this@PreferencesFrame.panel.getComponent(i) as JTextField).text!!
                val value =
                    (this@PreferencesFrame.panel.getComponent(i + 1) as JTextField) // Get the Text field at i
                        .text!! // Get the text from it
                        .replace("\"", "").replace("\\", "") // replace the quotes and backslashes
                val type = config[key]!!.jsonObject["t"]!! // Get the type of the key
                    .toString().replace("\"", "")
                    .replace("\\", "") // replace the quotes and backslashes

                newConfig[key] = JsonObject(
                    mapOf(
                        "v" to JsonPrimitive(value),
                        "t" to JsonPrimitive(type)
                    )
                )
            }

            // yay for serialization
            this@PreferencesFrame.configWrap.json = format.encodeToJsonElement(
                format.decodeFromString<Config>(
                    format.encodeToString(newConfig)
                )
            )
        }
    }
}