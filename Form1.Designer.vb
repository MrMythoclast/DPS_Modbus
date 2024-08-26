<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.btnSetVoltage = New System.Windows.Forms.Button()
        Me.lblsetVoltage = New System.Windows.Forms.Label()
        Me.lblsetCurrent = New System.Windows.Forms.Label()
        Me.btnSetCurrent = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.lbloutputCurrent = New System.Windows.Forms.Label()
        Me.lbloutputVoltage = New System.Windows.Forms.Label()
        Me.lbloutputWatt = New System.Windows.Forms.Label()
        Me.lblinputVoltage = New System.Windows.Forms.Label()
        Me.cbLock = New System.Windows.Forms.CheckBox()
        Me.tmrOutput = New System.Windows.Forms.Timer(Me.components)
        Me.cbStatus = New System.Windows.Forms.CheckBox()
        Me.trbBrightnes = New System.Windows.Forms.TrackBar()
        Me.lblCVCC = New System.Windows.Forms.Label()
        Me.lblModel = New System.Windows.Forms.Label()
        Me.lblFirmware = New System.Windows.Forms.Label()
        Me.tmrDebounce = New System.Windows.Forms.Timer(Me.components)
        CType(Me.trbBrightnes, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnSetVoltage
        '
        Me.btnSetVoltage.Location = New System.Drawing.Point(22, 177)
        Me.btnSetVoltage.Name = "btnSetVoltage"
        Me.btnSetVoltage.Size = New System.Drawing.Size(75, 23)
        Me.btnSetVoltage.TabIndex = 1
        Me.btnSetVoltage.Text = "set Voltage"
        Me.btnSetVoltage.UseVisualStyleBackColor = True
        '
        'lblsetVoltage
        '
        Me.lblsetVoltage.AutoSize = True
        Me.lblsetVoltage.Font = New System.Drawing.Font("Consolas", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblsetVoltage.Location = New System.Drawing.Point(28, 61)
        Me.lblsetVoltage.Name = "lblsetVoltage"
        Me.lblsetVoltage.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblsetVoltage.Size = New System.Drawing.Size(47, 34)
        Me.lblsetVoltage.TabIndex = 2
        Me.lblsetVoltage.Text = "V:"
        '
        'lblsetCurrent
        '
        Me.lblsetCurrent.AutoSize = True
        Me.lblsetCurrent.Font = New System.Drawing.Font("Consolas", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblsetCurrent.Location = New System.Drawing.Point(28, 95)
        Me.lblsetCurrent.Name = "lblsetCurrent"
        Me.lblsetCurrent.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblsetCurrent.Size = New System.Drawing.Size(47, 34)
        Me.lblsetCurrent.TabIndex = 3
        Me.lblsetCurrent.Text = "A:"
        '
        'btnSetCurrent
        '
        Me.btnSetCurrent.Location = New System.Drawing.Point(103, 177)
        Me.btnSetCurrent.Name = "btnSetCurrent"
        Me.btnSetCurrent.Size = New System.Drawing.Size(75, 23)
        Me.btnSetCurrent.TabIndex = 4
        Me.btnSetCurrent.Text = "set Amp"
        Me.btnSetCurrent.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(22, 206)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(75, 20)
        Me.TextBox1.TabIndex = 5
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(103, 206)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(75, 20)
        Me.TextBox2.TabIndex = 6
        '
        'lbloutputCurrent
        '
        Me.lbloutputCurrent.AutoSize = True
        Me.lbloutputCurrent.Font = New System.Drawing.Font("Consolas", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbloutputCurrent.Location = New System.Drawing.Point(167, 101)
        Me.lbloutputCurrent.Name = "lbloutputCurrent"
        Me.lbloutputCurrent.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lbloutputCurrent.Size = New System.Drawing.Size(77, 28)
        Me.lbloutputCurrent.TabIndex = 8
        Me.lbloutputCurrent.Text = "outA:"
        '
        'lbloutputVoltage
        '
        Me.lbloutputVoltage.AutoSize = True
        Me.lbloutputVoltage.Font = New System.Drawing.Font("Consolas", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbloutputVoltage.Location = New System.Drawing.Point(167, 67)
        Me.lbloutputVoltage.Name = "lbloutputVoltage"
        Me.lbloutputVoltage.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lbloutputVoltage.Size = New System.Drawing.Size(77, 28)
        Me.lbloutputVoltage.TabIndex = 7
        Me.lbloutputVoltage.Text = "outV:"
        '
        'lbloutputWatt
        '
        Me.lbloutputWatt.AutoSize = True
        Me.lbloutputWatt.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbloutputWatt.Location = New System.Drawing.Point(88, 140)
        Me.lbloutputWatt.Name = "lbloutputWatt"
        Me.lbloutputWatt.Size = New System.Drawing.Size(90, 22)
        Me.lbloutputWatt.TabIndex = 9
        Me.lbloutputWatt.Text = "outWatt:"
        '
        'lblinputVoltage
        '
        Me.lblinputVoltage.AutoSize = True
        Me.lblinputVoltage.Location = New System.Drawing.Point(204, 280)
        Me.lblinputVoltage.Name = "lblinputVoltage"
        Me.lblinputVoltage.Size = New System.Drawing.Size(69, 13)
        Me.lblinputVoltage.TabIndex = 10
        Me.lblinputVoltage.Text = "input Voltage"
        '
        'cbLock
        '
        Me.cbLock.AutoSize = True
        Me.cbLock.Location = New System.Drawing.Point(22, 267)
        Me.cbLock.Name = "cbLock"
        Me.cbLock.Size = New System.Drawing.Size(50, 17)
        Me.cbLock.TabIndex = 11
        Me.cbLock.Text = "Lock"
        Me.cbLock.UseVisualStyleBackColor = True
        '
        'tmrOutput
        '
        Me.tmrOutput.Interval = 1500
        '
        'cbStatus
        '
        Me.cbStatus.AutoSize = True
        Me.cbStatus.Location = New System.Drawing.Point(22, 244)
        Me.cbStatus.Name = "cbStatus"
        Me.cbStatus.Size = New System.Drawing.Size(97, 17)
        Me.cbStatus.TabIndex = 13
        Me.cbStatus.Text = "Output: On/Off"
        Me.cbStatus.UseVisualStyleBackColor = True
        '
        'trbBrightnes
        '
        Me.trbBrightnes.AutoSize = False
        Me.trbBrightnes.LargeChange = 1
        Me.trbBrightnes.Location = New System.Drawing.Point(184, 241)
        Me.trbBrightnes.Maximum = 5
        Me.trbBrightnes.Name = "trbBrightnes"
        Me.trbBrightnes.Size = New System.Drawing.Size(117, 20)
        Me.trbBrightnes.TabIndex = 14
        '
        'lblCVCC
        '
        Me.lblCVCC.AutoSize = True
        Me.lblCVCC.Font = New System.Drawing.Font("Consolas", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCVCC.Location = New System.Drawing.Point(229, 177)
        Me.lblCVCC.Name = "lblCVCC"
        Me.lblCVCC.Size = New System.Drawing.Size(44, 32)
        Me.lblCVCC.TabIndex = 15
        Me.lblCVCC.Text = "XX"
        '
        'lblModel
        '
        Me.lblModel.AutoSize = True
        Me.lblModel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblModel.Location = New System.Drawing.Point(44, 23)
        Me.lblModel.Name = "lblModel"
        Me.lblModel.Size = New System.Drawing.Size(57, 20)
        Me.lblModel.TabIndex = 16
        Me.lblModel.Text = "model"
        '
        'lblFirmware
        '
        Me.lblFirmware.AutoSize = True
        Me.lblFirmware.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFirmware.Location = New System.Drawing.Point(162, 23)
        Me.lblFirmware.Name = "lblFirmware"
        Me.lblFirmware.Size = New System.Drawing.Size(77, 20)
        Me.lblFirmware.TabIndex = 17
        Me.lblFirmware.Text = "firmware"
        '
        'tmrDebounce
        '
        Me.tmrDebounce.Interval = 1000
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(313, 302)
        Me.Controls.Add(Me.lblFirmware)
        Me.Controls.Add(Me.lblModel)
        Me.Controls.Add(Me.lblCVCC)
        Me.Controls.Add(Me.trbBrightnes)
        Me.Controls.Add(Me.cbStatus)
        Me.Controls.Add(Me.cbLock)
        Me.Controls.Add(Me.lblinputVoltage)
        Me.Controls.Add(Me.lbloutputWatt)
        Me.Controls.Add(Me.lbloutputCurrent)
        Me.Controls.Add(Me.lbloutputVoltage)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.btnSetCurrent)
        Me.Controls.Add(Me.lblsetCurrent)
        Me.Controls.Add(Me.lblsetVoltage)
        Me.Controls.Add(Me.btnSetVoltage)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "DPS 5015 - MOTBus Communication"
        CType(Me.trbBrightnes, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnSetVoltage As Button
    Friend WithEvents lblsetVoltage As Label
    Friend WithEvents lblsetCurrent As Label
    Friend WithEvents btnSetCurrent As Button
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents lbloutputCurrent As Label
    Friend WithEvents lbloutputVoltage As Label
    Friend WithEvents lbloutputWatt As Label
    Friend WithEvents lblinputVoltage As Label
    Friend WithEvents cbLock As CheckBox
    Friend WithEvents tmrOutput As Timer
    Friend WithEvents cbStatus As CheckBox
    Friend WithEvents trbBrightnes As TrackBar
    Friend WithEvents lblCVCC As Label
    Friend WithEvents lblModel As Label
    Friend WithEvents lblFirmware As Label
    Friend WithEvents tmrDebounce As Timer
End Class
