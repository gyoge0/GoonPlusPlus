package com.gyoge.gpp

import javax.swing.UIManager

fun main(args: Array<String>) {
    UIManager.setLookAndFeel(UIManager.getSystemLookAndFeelClassName())
    if (args.isEmpty()) {
        val mf = MainFrame()
    } else {
        val mf = MainFrame(args[0])
    }
}
