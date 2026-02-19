<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmConvertSTL
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.txtASCIIFilename = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnBrowseASCII = New System.Windows.Forms.Button
        Me.txtBinaryFilename = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnBrowseBinary = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOK = New System.Windows.Forms.Button
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog
        Me.SuspendLayout()
        '
        'txtASCIIFilename
        '
        Me.txtASCIIFilename.AllowDrop = True
        Me.txtASCIIFilename.Location = New System.Drawing.Point(16, 29)
        Me.txtASCIIFilename.Name = "txtASCIIFilename"
        Me.txtASCIIFilename.Size = New System.Drawing.Size(358, 20)
        Me.txtASCIIFilename.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(132, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Input ASCII STL Filename:"
        '
        'btnBrowseASCII
        '
        Me.btnBrowseASCII.Location = New System.Drawing.Point(380, 27)
        Me.btnBrowseASCII.Name = "btnBrowseASCII"
        Me.btnBrowseASCII.Size = New System.Drawing.Size(75, 23)
        Me.btnBrowseASCII.TabIndex = 2
        Me.btnBrowseASCII.Text = "Browse..."
        Me.btnBrowseASCII.UseVisualStyleBackColor = True
        '
        'txtBinaryFilename
        '
        Me.txtBinaryFilename.AllowDrop = True
        Me.txtBinaryFilename.Location = New System.Drawing.Point(16, 77)
        Me.txtBinaryFilename.Name = "txtBinaryFilename"
        Me.txtBinaryFilename.Size = New System.Drawing.Size(358, 20)
        Me.txtBinaryFilename.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(13, 61)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(142, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Output Binary STL Filename:"
        '
        'btnBrowseBinary
        '
        Me.btnBrowseBinary.Location = New System.Drawing.Point(380, 75)
        Me.btnBrowseBinary.Name = "btnBrowseBinary"
        Me.btnBrowseBinary.Size = New System.Drawing.Size(75, 23)
        Me.btnBrowseBinary.TabIndex = 2
        Me.btnBrowseBinary.Text = "Browse..."
        Me.btnBrowseBinary.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(378, 112)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(297, 112)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 4
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'frmConvertSTL
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(465, 147)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnBrowseBinary)
        Me.Controls.Add(Me.btnBrowseASCII)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtBinaryFilename)
        Me.Controls.Add(Me.txtASCIIFilename)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "frmConvertSTL"
        Me.Text = "Convert ASCII STL to Binary"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtASCIIFilename As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnBrowseASCII As System.Windows.Forms.Button
    Friend WithEvents txtBinaryFilename As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnBrowseBinary As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog

End Class
