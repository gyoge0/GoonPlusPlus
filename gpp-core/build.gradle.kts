application {
    mainClass.set("com.gyoge.gpp.MainKt")
}

repositories {
    mavenLocal()
    maven {
        url = uri("https://repo1.maven.org/maven2/")
    }

    maven {
        url = uri("https://repo.maven.apache.org/maven2/")
    }
}

dependencies {
    implementation("org.jetbrains.kotlin:kotlin-stdlib-jdk8:1.6.10")
    implementation("org.jetbrains.dokka:dokka-cli:1.6.10")
    implementation("org.jetbrains.kotlinx:kotlinx-serialization-json:1.3.2")
    implementation("org.jetbrains.kotlin:kotlin-reflect:1.6.10")
}

plugins {
    kotlin("jvm") version "1.6.10"
    kotlin("plugin.serialization") version "1.6.10"
    id("edu.sc.seis.launch4j") version "2.5.3"
    application
}

group = "com.gyoge"
version = "0.1"
description = "gpp"


tasks.withType<JavaCompile> {
    options.encoding = "UTF-8"
}

tasks.withType<Jar> {
    manifest {
        attributes["Main-Class"] = "com.gyoge.gpp.MainKt"
        attributes["Manifest-Version"] = "1.0"
        attributes["Class-Path"] = ""

        // Literally the hackiest way I could think of to get the manifest to include the dependencies
        // I'm not proud of this, but it works
        File("$buildDir/dependencies").listFiles()?.forEach { f ->
            attributes["Class-Path"] = "${attributes["Class-Path"]} ../dependencies/${f.name}"
        }

    }
}

task("copyDependencies", Copy::class) {
    configurations.compileClasspath.get()
        .filter { it.extension == "jar" }
        .forEach { from(it.absolutePath).into("$buildDir/dependencies") }
}

tasks.named("assemble") {
    finalizedBy("copyDependencies")
}


kotlin {
    jvmToolchain {
        (this as JavaToolchainSpec).languageVersion.set(JavaLanguageVersion.of(17))
    }
}

tasks.withType<edu.sc.seis.launch4j.tasks.DefaultLaunch4jTask> {
    outfile = "GoonPlusPlus.exe"
    libraryDir = "dependencies"
    val version = 17
    jreMinVersion = "1.$version"
    downloadUrl =
        "https://download.oracle.com/java/$version/latest/jdk-${version}_windows-x64_bin.exe"
    messagesJreVersionError =
        "This program requires the java version listed below. Please run the downloaded installer to install the latest version.\n\n"
    icon = "$projectDir/icon.ico"

}