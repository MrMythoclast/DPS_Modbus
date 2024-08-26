Imports System.IO.Ports
Imports Modbus.Device


'register': 0x00,  # Register für Spannungseinstellungen  # Register for voltage settings
'register': 0x01,  # Register für Stromstärke-Einstellungen  # Register for current settings
'register': 0x02,  # Register für Ausgangsspannung  # Register for output voltage
'register': 0x03,  # Register für Ausgangsstrom  # Register for output current
'register': 0x04,  # Register für Ausgangsleistung (Watt)  # Register for output power (Watt)
'register': 0x05,  # Register für Eingangsspannung  # Register for input voltage
'register': 0x06,  # Register für Tastensperre (Lock)  # Register for button lock
'register': 0x07,  # Register für Schutzstatus  # Register for protection status
'register': 0x08,  # Register für Konstantstrommodus  # Register for constant current mode
'register': 0x09,  # Register für Ausgangszustand  # Register for output state
'register': 0x0a,  # Register für Helligkeitsstufe  # Register for brightness level
'register': 0x0b,  # Register für Modell des Geräts  # Register for device model
'register': 0x0c,  # Register für Gerätesoftware (Firmware)  # Register for device software (firmware)
'register': 0x23,  # Register für Gruppenladegerät (Read-Only)  # Register for group charger (Read-Only)



Public Class Form1



    Dim DpsPort As String = "COM5" ' You have to edit This Port to your needs before! 



    ' SerialPort1 wird als neues SerialPort-Objekt erstellt
    ' SerialPort1 is created as a new SerialPort object
    Private WithEvents SerialPort1 As New SerialPort
    ' modbusMaster ist eine Instanz des Modbus RTU Masters
    ' modbusMaster is an instance of the Modbus RTU Master
    Private modbusMaster As IModbusSerialMaster

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Konfiguration des seriellen Ports
        ' Configuring the serial port
        SerialPort1.PortName = DpsPort  ' Ihr serieller Port / Your Serial Port
        SerialPort1.BaudRate = 115200
        SerialPort1.Parity = IO.Ports.Parity.None
        SerialPort1.DataBits = 8
        SerialPort1.StopBits = IO.Ports.StopBits.One
        SerialPort1.ReadTimeout = 2000
        SerialPort1.WriteTimeout = 1000

        Try
            ' Öffnen des seriellen Ports
            ' Opening the serial port
            SerialPort1.Open()
            ' Erstellen des Modbus RTU Masters
            ' Creating the Modbus RTU Master
            modbusMaster = ModbusSerialMaster.CreateRtu(SerialPort1)
            ' Lese alle Register
            ' Read all registers
            ReadAllRegisters()
        Catch ex As Exception
            ' Fehlerbehandlung, falls der serielle Port nicht geöffnet werden kann
            ' Error handling in case the serial port cannot be opened
            MessageBox.Show("Fehler beim Öffnen des seriellen Ports: " & ex.Message)
        End Try
    End Sub

    ' Wird ausgeführt, wenn das Formular geschlossen wird
    ' Executes when the form is closing
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        ' Seriellen Port schließen, wenn er geöffnet ist
        ' Close the serial port if it is open
        If SerialPort1.IsOpen Then
            SerialPort1.Close()
        End If
    End Sub

    ' Ereignis-Handler für den Klick auf btnSetVoltage
    ' Event handler for the click on btnSetVoltage
    Private Sub btnSetVoltage_Click(sender As Object, e As EventArgs) Handles btnSetVoltage.Click
        ' Timer stoppen
        ' Stop the timer
        tmrOutput.Stop()

        ' Stopuhr starten, um die Zeit zu messen
        ' Start the stopwatch to measure time
        Dim stopwatch As New Stopwatch()
        stopwatch.Start()

        Try
            ' Modbus-Adresse des Geräts und Startregister definieren
            ' Define the Modbus address of the device and the start register
            Dim slaveId As Byte = 1
            Dim startAddress As UShort = &H0 ' Spannungsregister / Voltage register

            ' Eingabewert aus TextBox1 auslesen und formatieren
            ' Read and format the input value from TextBox1
            Dim inputText As String = TextBox1.Text.Trim()
            inputText = inputText.Replace(","c, "."c)

            Dim voltage As Double
            Dim valueToWrite As UShort

            ' Versuch, den Text in eine Zahl umzuwandeln
            ' Attempt to convert the text to a number
            If Double.TryParse(inputText, Globalization.NumberStyles.Number, Globalization.CultureInfo.InvariantCulture, voltage) Then
                ' Umrechnung der Spannung in einen Registerwert
                ' Convert the voltage to a register value
                Dim tempValue As Double = voltage * 100

                ' Sicherstellen, dass der Wert im Bereich von UShort liegt
                ' Ensure the value is within the range of UShort
                If tempValue < 0 OrElse tempValue > UShort.MaxValue Then
                    MessageBox.Show("Der berechnete Wert liegt außerhalb des zulässigen Bereichs für das Register.")
                    Return
                End If

                ' Rundung und Konvertierung des Wertes zu UShort
                ' Rounding and converting the value to UShort
                valueToWrite = CUInt(Math.Round(tempValue))

                ' Debug-Ausgaben zur Überprüfung der Umrechnung
                ' Debug output to check the conversion
                Debug.WriteLine("Eingabewert: " & voltage.ToString("F2"))
                Debug.WriteLine("Berechneter Registerwert: " & valueToWrite.ToString())

                ' Schreiben des Wertes in das Register
                ' Writing the value to the register
                modbusMaster.WriteSingleRegister(slaveId, startAddress, valueToWrite)

                Debug.WriteLine("Register erfolgreich geschrieben: " & valueToWrite.ToString("D"))

                Dim currentVoltage As Decimal
                ' Versuch, den Wert aus TextBox2 in eine Dezimalzahl zu konvertieren
                ' Attempt to convert the value from TextBox2 to a decimal
                If Decimal.TryParse(TextBox1.Text, currentVoltage) Then
                    ' Zahl formatieren und anzeigen
                    ' Format and display the number
                    lblsetVoltage.Text = currentVoltage.ToString("F2") & " V"
                Else
                    ' Bei Fehler in der Konvertierung eine Warnung ausgeben
                    ' If conversion fails, display a warning
                    lblsetCurrent.Text = "Ungültiger Wert"
                End If

                tmrOutput.Start()
                TextBox1.Text = ""
            Else
                ' Warnung bei ungültigem Wert
                ' Warning for invalid value
                MessageBox.Show("Ungültiger Wert. Bitte geben Sie eine gültige Zahl ein.")
            End If
        Catch ex As Exception
            ' Fehlerbehandlung bei Problemen mit dem Schreiben des Registers
            ' Error handling for issues with writing the register
            Debug.WriteLine("Fehler beim Schreiben des Registers: " & ex.Message)
        Finally
            ' Stoppen der Stoppuhr und Anpassen des Timer-Intervalls
            ' Stopping the stopwatch and adjusting the timer interval
            stopwatch.Stop()
            Dim elapsedMilliseconds As Long = stopwatch.ElapsedMilliseconds
            Debug.WriteLine("Zeit zum Schreiben des Registers: " & elapsedMilliseconds & " ms")

            ' Timer-Intervall anpassen, um Verzögerungen zu berücksichtigen
            ' Adjust the timer interval to account for delays
            tmrOutput.Interval = Math.Max(1000, elapsedMilliseconds + 100) ' Füge 100ms Pufferzeit hinzu / Add 100ms buffer time
            Console.WriteLine("timer intervall nun: " & tmrOutput.Interval)
            tmrOutput.Start()

            ' TextBox1 zurücksetzen
            ' Reset TextBox1
            TextBox1.Text = ""
        End Try
    End Sub

    ' Ereignis-Handler für den Klick auf btnSetCurrent
    ' Event handler for the click on btnSetCurrent
    Private Sub btnSetCurrent_Click(sender As Object, e As EventArgs) Handles btnSetCurrent.Click
        ' Timer stoppen
        ' Stop the timer
        tmrOutput.Stop()

        ' Stopuhr starten, um die Zeit zu messen
        ' Start the stopwatch to measure time
        Dim stopwatch As New Stopwatch()
        stopwatch.Start()

        Try
            ' Modbus-Adresse des Geräts und Startregister definieren
            ' Define the Modbus address of the device and the start register
            Dim slaveId As Byte = 1
            Dim startAddress As UShort = &H1 ' Stromregister / Current register

            ' Eingabewert aus TextBox2 auslesen und formatieren
            ' Read and format the input value from TextBox2
            Dim inputText As String = TextBox2.Text.Trim()
            inputText = inputText.Replace(","c, "."c)

            Dim current As Double
            Dim valueToWrite As UShort

            ' Versuch, den Text in eine Zahl umzuwandeln
            ' Attempt to convert the text to a number
            If Double.TryParse(inputText, Globalization.NumberStyles.Number, Globalization.CultureInfo.InvariantCulture, current) Then
                Dim tempValue As Double = current * 100

                ' Sicherstellen, dass der berechnete Wert innerhalb des Bereichs von UShort liegt
                ' Ensure the calculated value is within the UShort range
                If tempValue < 0 OrElse tempValue > UShort.MaxValue Then
                    MessageBox.Show("Der berechnete Wert liegt außerhalb des zulässigen Bereichs für das Register.")
                    Return
                End If

                ' Rundung auf die nächste ganze Zahl und Konvertierung zu UShort
                ' Round to the nearest whole number and convert to UShort
                valueToWrite = CUInt(Math.Round(tempValue))

                ' Debug-Ausgaben zur Überprüfung der Umrechnung
                ' Debug output to check the conversion
                Debug.WriteLine("Eingabewert: " & current.ToString("F2"))
                Debug.WriteLine("Berechneter Registerwert: " & valueToWrite.ToString())

                ' Register schreiben
                ' Write the register
                modbusMaster.WriteSingleRegister(slaveId, startAddress, valueToWrite)

                Debug.WriteLine("Register erfolgreich geschrieben: " & valueToWrite.ToString("D"))

                Dim currentValue As Decimal

                ' Versuche, den Wert aus TextBox2 in eine Dezimalzahl zu konvertieren
                ' Attempt to convert the value from TextBox2 to a decimal
                If Decimal.TryParse(TextBox2.Text, currentValue) Then
                    ' Formatiere die Zahl mit zwei Dezimalstellen und füge " A" hinzu
                    ' Format the number with two decimal places and add " A"
                    lblsetCurrent.Text = currentValue.ToString("F2") & " A"
                Else
                    ' Falls die Konvertierung fehlschlägt, setze den Text auf einen Fehlerwert oder eine Warnung
                    ' If conversion fails, set the text to an error value or warning
                    lblsetCurrent.Text = "Ungültiger Wert"
                End If
            Else
                ' Warnung bei ungültigem Wert
                ' Warning for invalid value
                MessageBox.Show("Ungültiger Wert. Bitte geben Sie eine gültige Zahl ein.")
            End If
        Catch ex As Exception
            ' Fehlerbehandlung bei Problemen mit dem Schreiben des Registers
            ' Error handling for issues with writing the register
            Debug.WriteLine("Fehler beim Schreiben des Registers: " & ex.Message)
        Finally
            ' Stoppe die Stoppuhr und passe den Timer-Intervall an
            ' Stop the stopwatch and adjust the timer interval
            stopwatch.Stop()
            Dim elapsedMilliseconds As Long = stopwatch.ElapsedMilliseconds
            Debug.WriteLine("Zeit zum Schreiben des Registers: " & elapsedMilliseconds & " ms")

            ' Timer-Intervall anpassen, um Verzögerungen zu berücksichtigen
            ' Adjust the timer interval to account for delays
            tmrOutput.Interval = Math.Max(1000, elapsedMilliseconds + 100) ' Füge 100ms Pufferzeit hinzu / Add 100ms buffer time
            Console.WriteLine("Timer-Intervall nun: " & tmrOutput.Interval)
            tmrOutput.Start()

            ' TextBox2 zurücksetzen
            ' Reset TextBox2
            TextBox2.Text = ""
        End Try
    End Sub

    Private Sub ReadRegisters()
        Try
            ' Modbus-Adresse des Geräts definieren
            ' Define the Modbus address of the device
            Dim slaveId As Byte = 1

            ' Registeradressen und Anzahl der Register definieren
            ' Define register addresses and number of registers
            Dim voltageAddress As UShort = &H0 ' &H0 Spannung / Voltage
            Dim currentAddress As UShort = &H1 ' &H1 Strom / Current
            Dim numRegisters As UShort = 1 ' Anzahl der zu lesenden Register / Number of registers to read

            ' Register für Spannung lesen
            ' Read registers for voltage
            Dim voltageRegisters As UShort() = modbusMaster.ReadHoldingRegisters(slaveId, voltageAddress, numRegisters)
            Dim currentRegisters As UShort() = modbusMaster.ReadHoldingRegisters(slaveId, currentAddress, numRegisters)

            ' Rohwerte auslesen
            ' Read raw values
            Dim rawVoltage As UShort = voltageRegisters(0)
            Dim rawCurrent As UShort = currentRegisters(0)

            ' Werte umrechnen
            ' Convert values
            Dim voltage As Double = rawVoltage / 100.0 ' Beispiel: Rohwert durch 100 teilen / Example: divide raw value by 100
            Dim current As Double = rawCurrent / 100.0 ' Beispiel: Rohwert durch 100 teilen / Example: divide raw value by 100

            ' Ergebnisse anzeigen
            ' Display results
            lblsetVoltage.Text = $"{voltage:F2} V" ' Formatieren auf zwei Dezimalstellen / Format to two decimal places
            lblsetCurrent.Text = $"{current:F2} A" ' Formatieren auf zwei Dezimalstellen / Format to two decimal places
        Catch ex As Exception
            ' Fehlerbehandlung beim Lesen der Register
            ' Error handling when reading registers
            MessageBox.Show("Fehler beim Lesen der Register: " & ex.Message)
        End Try
    End Sub

    Sub anAus(ByVal State As Integer)
        Try
            ' Timer stoppen
            ' Stop the timer
            tmrOutput.Stop()

            ' Modbus-Adresse und Register definieren
            ' Define the Modbus address and register
            Dim slaveId As Byte = 1
            Dim startAddress As UShort = &H9

            ' Register schreiben
            ' Write the register
            modbusMaster.WriteSingleRegister(slaveId, startAddress, State)

            ' Timer starten
            ' Start the timer
            tmrOutput.Start()
        Catch ex As Exception
            ' Fehlerbehandlung beim Schreiben des Registers
            ' Error handling for issues with writing the register
            Debug.WriteLine("Fehler beim Schreiben des Registers: " & ex.Message)
        End Try
    End Sub

    Private Sub ReadAllRegisters()
        Try
            ' Modbus-Adresse des Geräts / Modbus address of the device
            Dim slaveId As Byte = 1

            ' Registeradressen / Register addresses
            Dim voltageAddress As UShort = &H0 ' Spannung / Voltage
            Dim currentAddress As UShort = &H1 ' Strom / Current
            Dim outputVoltAddress As UShort = &H2 ' Ausgangsspannung / Output Voltage
            Dim outputCurrentAddress As UShort = &H3 ' Ausgangsstrom / Output Current
            Dim outputWattAddress As UShort = &H4 ' Ausgangsleistung / Output Power
            Dim inputVoltageAddress As UShort = &H5 ' Eingangsspannung / Input Voltage
            Dim lockAddress As UShort = &H6 ' Tastensperre / Key Lock
            Dim protectionAddress As UShort = &H7 ' Schutzstatus / Protection Status
            Dim constantCurrentAddress As UShort = &H8 ' Konstantstrommodus / Constant Current Mode
            Dim outputStatusAddress As UShort = &H9 ' Ausgangszustand / Output Status
            Dim screenBrightnessAddress As UShort = &HA ' Helligkeitsstufe / Screen Brightness
            Dim modelAddress As UShort = &HB ' Modell des Geräts / Device Model
            Dim firmwareVersionAddress As UShort = &HC ' Gerätesoftware (Firmware) / Firmware Version
            Dim groupLoaderAddress As UShort = &H23 ' Gruppenladegerät / Group Loader

            ' Anzahl der Register, die gelesen werden sollen / Number of registers to be read
            Dim numRegisters As UShort = 1

            ' Register lesen / Read registers
            Dim voltageRegisters As UShort() = modbusMaster.ReadHoldingRegisters(slaveId, voltageAddress, numRegisters)
            Dim currentRegisters As UShort() = modbusMaster.ReadHoldingRegisters(slaveId, currentAddress, numRegisters)
            Dim outputVoltRegisters As UShort() = modbusMaster.ReadHoldingRegisters(slaveId, outputVoltAddress, numRegisters)
            Dim outputCurrentRegisters As UShort() = modbusMaster.ReadHoldingRegisters(slaveId, outputCurrentAddress, numRegisters)
            Dim outputWattRegisters As UShort() = modbusMaster.ReadHoldingRegisters(slaveId, outputWattAddress, numRegisters)
            Dim inputVoltageRegisters As UShort() = modbusMaster.ReadHoldingRegisters(slaveId, inputVoltageAddress, numRegisters)
            Dim lockRegisters As UShort() = modbusMaster.ReadHoldingRegisters(slaveId, lockAddress, numRegisters)
            Dim protectionRegisters As UShort() = modbusMaster.ReadHoldingRegisters(slaveId, protectionAddress, numRegisters)
            Dim constantCurrentRegisters As UShort() = modbusMaster.ReadHoldingRegisters(slaveId, constantCurrentAddress, numRegisters)
            Dim outputStatusRegisters As UShort() = modbusMaster.ReadHoldingRegisters(slaveId, outputStatusAddress, numRegisters)
            Dim screenBrightnessRegisters As UShort() = modbusMaster.ReadHoldingRegisters(slaveId, screenBrightnessAddress, numRegisters)
            Dim modelRegisters As UShort() = modbusMaster.ReadHoldingRegisters(slaveId, modelAddress, numRegisters)
            Dim firmwareVersionRegisters As UShort() = modbusMaster.ReadHoldingRegisters(slaveId, firmwareVersionAddress, numRegisters)
            Dim groupLoaderRegisters As UShort() = modbusMaster.ReadHoldingRegisters(slaveId, groupLoaderAddress, numRegisters)

            ' Rohwerte extrahieren / Extract raw values
            Dim rawVoltage As UShort = voltageRegisters(0)
            Dim rawCurrent As UShort = currentRegisters(0)
            Dim rawOutputVolt As UShort = outputVoltRegisters(0)
            Dim rawOutputCurrent As UShort = outputCurrentRegisters(0)
            Dim rawOutputWatt As UShort = outputWattRegisters(0)
            Dim rawInputVoltage As UShort = inputVoltageRegisters(0)
            Dim rawLock As UShort = lockRegisters(0)
            Dim rawProtection As UShort = protectionRegisters(0)
            Dim rawConstantCurrent As UShort = constantCurrentRegisters(0)
            Dim rawOutputStatus As UShort = outputStatusRegisters(0)
            Dim rawScreenBrightness As UShort = screenBrightnessRegisters(0)
            Dim rawModel As UShort = modelRegisters(0)
            Dim rawFirmwareVersion As UShort = firmwareVersionRegisters(0)
            Dim rawGroupLoader As UShort = groupLoaderRegisters(0)

            ' Werte umrechnen / Convert raw values
            Dim voltage As Double = rawVoltage / 100.0 ' Beispiel: Rohwert durch 100 teilen / Example: Divide raw value by 100
            Dim current As Double = rawCurrent / 100.0 ' Beispiel: Rohwert durch 100 teilen / Example: Divide raw value by 100
            Dim outputVolt As Double = rawOutputVolt / 100.0
            Dim outputCurrent As Double = rawOutputCurrent / 100.0
            Dim outputWatt As Double = rawOutputWatt / 100.0
            Dim inputVoltage As Double = rawInputVoltage / 100.0
            Dim lock As Boolean = (rawLock <> 0) ' Boolean-Wert bestimmen / Determine boolean value
            Dim protection As Integer = rawProtection
            Dim constantCurrent As Boolean = (rawConstantCurrent <> 0) ' Boolean-Wert bestimmen / Determine boolean value
            Dim outputStatus As Boolean = (rawOutputStatus <> 0) ' Boolean-Wert bestimmen / Determine boolean value
            Dim screenBrightness As Integer = rawScreenBrightness
            Dim model As Integer = rawModel
            Dim firmwareVersion As Integer = rawFirmwareVersion
            Dim groupLoader As Integer = rawGroupLoader

            ' Ergebnisse anzeigen / Display results
            lblsetVoltage.Text = $"{voltage:F2} V" ' F2 formatieren auf zwei Dezimalstellen / Format to two decimal places
            lblsetCurrent.Text = $"{current:F2} A" ' F2 formatieren auf zwei Dezimalstellen / Format to two decimal places
            lbloutputVoltage.Text = $"{outputVolt:F2} V"
            lbloutputCurrent.Text = $"{outputCurrent:F2} A"
            lbloutputWatt.Text = $"{outputWatt:F2} W"
            lblinputVoltage.Text = $"U-In: {inputVoltage:F2} V"
            cbLock.Checked = lock ' Tastensperre auf CheckBox setzen / Set key lock status on checkbox
            cbStatus.Checked = outputStatus ' Ausgangszustand auf CheckBox setzen / Set output status on checkbox
            trbBrightnes.Value = screenBrightness ' Helligkeitsstufe auf Schieberegler setzen / Set brightness level on slider
            If constantCurrent = True Then
                lblCVCC.Text = constantCurrent ' Konstantstrommodus anzeigen / Display constant current mode
            Else
                lblCVCC.Text = "CV" ' Konstantspannung anzeigen / Display constant voltage mode
            End If
            lblModel.Text = "Model: " & model ' Gerätemodell anzeigen / Display device model
            lblFirmware.Text = "Firmware: " & firmwareVersion ' Firmware-Version anzeigen / Display firmware version
            'lblProtection.Text = $"Schutzstatus: {protection}" ' Schutzstatus anzeigen / Display protection status
            'lblConstantCurrent.Text = $"Konstantstrommodus: {(If(constantCurrent, "Aktiv", "Inaktiv"))}" ' Konstantstrommodus anzeigen / Display constant current mode
            'lblModel.Text = $"Modell: {model}" ' Modell anzeigen / Display model
            'lblFirmwareVersion.Text = $"Firmware-Version: {firmwareVersion}" ' Firmware-Version anzeigen / Display firmware version
            'lblGroupLoader.Text = $"Gruppenladegerät: {groupLoader}" ' Gruppenladegerät anzeigen / Display group loader
        Catch ex As Exception
            MessageBox.Show("Fehler beim Lesen der Register: " & ex.Message) ' Fehlermeldung anzeigen / Display error message
        End Try
    End Sub

    Private Async Sub tmrOutput_Tick(sender As Object, e As EventArgs) Handles tmrOutput.Tick
        Try
            ' Asynchrone Ausführung des Modbus-Lesevorgangs / Asynchronous execution of Modbus read operation
            Await Task.Run(Sub()
                               ' Modbus-Adresse des Geräts / Modbus address of the device
                               Dim slaveId As Byte = 1

                               Dim outputVoltAddress As UShort = &H2 ' Ausgangsspannung / Output Voltage
                               Dim outputCurrentAddress As UShort = &H3 ' Ausgangsstrom / Output Current
                               Dim outputWattAddress As UShort = &H4 ' Ausgangsleistung / Output Power
                               Dim constantCurrentAddress As UShort = &H8 ' Konstantstrommodus / Constant Current Mode

                               ' Anzahl der Register, die gelesen werden sollen / Number of registers to be read
                               Dim numRegisters As UShort = 1

                               ' Register lesen / Read registers
                               Dim outputVoltRegisters As UShort() = modbusMaster.ReadHoldingRegisters(slaveId, outputVoltAddress, numRegisters)
                               Dim outputCurrentRegisters As UShort() = modbusMaster.ReadHoldingRegisters(slaveId, outputCurrentAddress, numRegisters)
                               Dim outputWattRegisters As UShort() = modbusMaster.ReadHoldingRegisters(slaveId, outputWattAddress, numRegisters)
                               Dim constantCurrentRegisters As UShort() = modbusMaster.ReadHoldingRegisters(slaveId, constantCurrentAddress, numRegisters)

                               ' Rohwerte extrahieren / Extract raw values
                               Dim rawOutputVolt As UShort = outputVoltRegisters(0)
                               Dim rawOutputCurrent As UShort = outputCurrentRegisters(0)
                               Dim rawOutputWatt As UShort = outputWattRegisters(0)
                               Dim rawConstantCurrent As UShort = constantCurrentRegisters(0)

                               ' Werte umrechnen / Convert raw values
                               Dim outputVolt As Double = rawOutputVolt / 100.0
                               Dim outputCurrent As Double = rawOutputCurrent / 100.0
                               Dim outputWatt As Double = rawOutputWatt / 100.0
                               Dim constantCurrent As Boolean = (rawConstantCurrent <> 0) ' Boolean-Wert bestimmen / Determine boolean value

                               ' Ergebnisse auf der UI aktualisieren / Update results on the UI
                               Invoke(Sub()
                                          lbloutputVoltage.Text = $"{outputVolt:F2} V"
                                          lbloutputCurrent.Text = $"{outputCurrent:F2} A"
                                          lbloutputWatt.Text = $"{outputWatt:F2} W"
                                          If constantCurrent Then
                                              lblCVCC.Text = "CC" ' Konstantstrommodus anzeigen / Display constant current mode
                                          Else
                                              lblCVCC.Text = "CV" ' Konstantspannung anzeigen / Display constant voltage mode
                                          End If
                                      End Sub)
                           End Sub)
        Catch ex As Exception
            Console.WriteLine(ex.Message) ' Fehlermeldung in der Konsole ausgeben / Output error message to console
        End Try
    End Sub


    'Checkbox Output On/Off
    Private Sub cbStatus_CheckedChanged(sender As Object, e As EventArgs) Handles cbStatus.CheckedChanged
        If cbStatus.Checked Then
            ' Wenn die Checkbox aktiviert ist, rufe anAus(1) auf und ändere die Hintergrundfarbe der Labels auf Grün.
            ' If the checkbox is checked, call anAus(1) and change the background color of the labels to green.
            anAus(1)
            lbloutputCurrent.BackColor = Color.Green
            lbloutputVoltage.BackColor = Color.Green
            lbloutputWatt.BackColor = Color.Green
        Else
            ' Wenn die Checkbox deaktiviert ist, rufe anAus(0) auf und stelle die Hintergrundfarbe der Labels auf Transparent.
            ' If the checkbox is unchecked, call anAus(0) and set the background color of the labels to transparent.
            anAus(0)
            lbloutputCurrent.BackColor = Color.Transparent
            lbloutputVoltage.BackColor = Color.Transparent
            lbloutputWatt.BackColor = Color.Transparent
        End If
    End Sub


    'Private Sub trbBrightnes_Scroll(sender As Object, e As EventArgs) Handles trbBrightnes.Scroll
    '    ' Stoppe den Timer, bevor Änderungen vorgenommen werden.
    '    ' Stop the timer before making changes.
    '    tmrOutput.Stop()

    '    ' Stopuhr starten, um die Zeit zum Schreiben des Registers zu messen.
    '    ' Start a stopwatch to measure the time taken to write the register.
    '    Dim stopwatch As New Stopwatch()
    '    stopwatch.Start()

    '    Try
    '        ' Modbus-Adresse des Geräts und Startregister festlegen.
    '        ' Set the Modbus address of the device and the starting register.
    '        Dim slaveId As Byte = 1
    '        Dim startAddress As UShort = &HA ' Helligkeits-Register / Brightness register

    '        ' Debug-Ausgaben zur Überprüfung der Umrechnung.
    '        ' Debug output to check the conversion.
    '        Debug.WriteLine("Eingabewert: " & trbBrightnes.Value.ToString("F2"))

    '        ' Register schreiben.
    '        ' Write to the register.
    '        modbusMaster.WriteSingleRegister(slaveId, startAddress, trbBrightnes.Value)

    '        Debug.WriteLine("Register erfolgreich geschrieben: " & trbBrightnes.Value.ToString("D"))

    '        ' Starte den Timer erneut nach dem Schreiben des Registers.
    '        ' Restart the timer after writing to the register.
    '        tmrOutput.Start()

    '    Catch ex As Exception
    '        ' Fehler beim Schreiben des Registers behandeln.
    '        ' Handle any errors that occur while writing to the register.
    '        Debug.WriteLine("Fehler beim Schreiben des Registers: " & ex.Message)
    '    Finally
    '        ' Stoppe die Stoppuhr und gebe die benötigte Zeit aus.
    '        ' Stop the stopwatch and output the elapsed time.
    '        stopwatch.Stop()
    '        Dim elapsedMilliseconds As Long = stopwatch.ElapsedMilliseconds
    '        Debug.WriteLine("Zeit zum Schreiben des Registers: " & elapsedMilliseconds & " ms")

    '        ' Starte den Timer erneut, nachdem die Zeitmessung abgeschlossen ist.
    '        ' Restart the timer after measuring the elapsed time.
    '        tmrOutput.Start()

    '    End Try
    'End Sub


    ' Timer zur Verzögerung des Register-Schreibens
    ' Timer zur Verzögerung des Register-Schreibens
    Private Sub tmrDebounce_Tick(sender As Object, e As EventArgs) Handles tmrDebounce.Tick
        ' Stoppe den Timer, um zu verhindern, dass er wiederholt auslöst
        tmrDebounce.Stop()

        ' Stoppe den Timer für die Output-Updates
        tmrOutput.Stop()

        ' Stopuhr starten, um die Zeit zum Schreiben des Registers zu messen
        Dim stopwatch As New Stopwatch()
        stopwatch.Start()

        Try
            ' Modbus-Adresse des Geräts und Startregister festlegen
            Dim slaveId As Byte = 1
            Dim startAddress As UShort = &HA ' Helligkeits-Register / Brightness register

            ' Debug-Ausgaben zur Überprüfung der Umrechnung
            Debug.WriteLine("Eingabewert: " & trbBrightnes.Value.ToString("F2"))

            ' Register schreiben
            modbusMaster.WriteSingleRegister(slaveId, startAddress, trbBrightnes.Value)

            Debug.WriteLine("Register erfolgreich geschrieben: " & trbBrightnes.Value.ToString("D"))

        Catch ex As Exception
            ' Fehler beim Schreiben des Registers behandeln
            Debug.WriteLine("Fehler beim Schreiben des Registers: " & ex.Message)
        Finally
            ' Stoppe die Stoppuhr und gebe die benötigte Zeit aus
            stopwatch.Stop()
            Dim elapsedMilliseconds As Long = stopwatch.ElapsedMilliseconds
            Debug.WriteLine("Zeit zum Schreiben des Registers: " & elapsedMilliseconds & " ms")

            ' Starte den Timer für die Output-Updates erneut
            tmrOutput.Start()
        End Try
    End Sub

    Private Sub trbBrightnes_Scroll(sender As Object, e As EventArgs) Handles trbBrightnes.Scroll
        ' Stoppe den Debounce-Timer, falls er bereits läuft
        tmrDebounce.Stop()

        ' Setze den Timer für 500 Millisekunden, um das Ende des Scrollens abzuwarten
        tmrDebounce.Interval = 500
        tmrDebounce.Start()
    End Sub


    Private Sub cbLock_CheckedChanged(sender As Object, e As EventArgs) Handles cbLock.CheckedChanged
        ' Stoppe den Timer, bevor Änderungen vorgenommen werden.
        ' Stop the timer before making changes.
        tmrOutput.Stop()

        ' Stopuhr starten, um die Zeit zum Schreiben des Registers zu messen.
        ' Start a stopwatch to measure the time taken to write the register.
        Dim stopwatch As New Stopwatch()
        stopwatch.Start()

        Try
            ' Modbus-Adresse des Geräts und Startregister festlegen.
            ' Set the Modbus address of the device and the starting register.
            Dim slaveId As Byte = 1
            Dim startAddress As UShort = &H6 ' Sperr-Register / Lock register
            Dim lockState As Integer = cbLock.CheckState

            ' Debug-Ausgaben zur Überprüfung der Umrechnung.
            ' Debug output to check the conversion.
            Debug.WriteLine("Eingabewert: " & lockState.ToString("F2"))

            ' Register schreiben.
            ' Write to the register.
            modbusMaster.WriteSingleRegister(slaveId, startAddress, lockState)

            Debug.WriteLine("Register erfolgreich geschrieben: " & lockState.ToString("D"))

            ' Starte den Timer erneut nach dem Schreiben des Registers.
            ' Restart the timer after writing to the register.
            tmrOutput.Start()

        Catch ex As Exception
            ' Fehler beim Schreiben des Registers behandeln.
            ' Handle any errors that occur while writing to the register.
            Debug.WriteLine("Fehler beim Schreiben des Registers: " & ex.Message)
        Finally
            ' Stoppe die Stoppuhr und gebe die benötigte Zeit aus.
            ' Stop the stopwatch and output the elapsed time.
            stopwatch.Stop()
            Dim elapsedMilliseconds As Long = stopwatch.ElapsedMilliseconds
            Debug.WriteLine("Zeit zum Schreiben des Registers: " & elapsedMilliseconds & " ms")

            ' Starte den Timer erneut, nachdem die Zeitmessung abgeschlossen ist.
            ' Restart the timer after measuring the elapsed time.
            tmrOutput.Start()

        End Try
    End Sub

End Class