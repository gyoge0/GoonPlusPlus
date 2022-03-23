package com.gyoge.gpp

fun main(args: Array<String>) {
    if (args.isEmpty()) {
        val mf = MainFrame()
    } else {
        val mf = MainFrame(args[0])
    }
}
