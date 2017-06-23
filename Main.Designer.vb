<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Main
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
        Me.But_Open = New System.Windows.Forms.Button()
        Me.But_Extract = New System.Windows.Forms.Button()
        Me.But_Inject = New System.Windows.Forms.Button()
        Me.List_Parts = New System.Windows.Forms.ListBox()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.OpenYanmDialog = New System.Windows.Forms.OpenFileDialog()
        Me.But_ExtAll = New System.Windows.Forms.Button()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.SuspendLayout()
        '
        'But_Open
        '
        Me.But_Open.Location = New System.Drawing.Point(13, 13)
        Me.But_Open.Name = "But_Open"
        Me.But_Open.Size = New System.Drawing.Size(65, 23)
        Me.But_Open.TabIndex = 0
        Me.But_Open.Text = "Open"
        Me.But_Open.UseVisualStyleBackColor = True
        '
        'But_Extract
        '
        Me.But_Extract.Location = New System.Drawing.Point(83, 13)
        Me.But_Extract.Name = "But_Extract"
        Me.But_Extract.Size = New System.Drawing.Size(65, 23)
        Me.But_Extract.TabIndex = 1
        Me.But_Extract.Text = "Extract"
        Me.But_Extract.UseVisualStyleBackColor = True
        '
        'But_Inject
        '
        Me.But_Inject.Location = New System.Drawing.Point(153, 13)
        Me.But_Inject.Name = "But_Inject"
        Me.But_Inject.Size = New System.Drawing.Size(65, 23)
        Me.But_Inject.TabIndex = 2
        Me.But_Inject.Text = "Inject"
        Me.But_Inject.UseVisualStyleBackColor = True
        '
        'List_Parts
        '
        Me.List_Parts.FormattingEnabled = True
        Me.List_Parts.Location = New System.Drawing.Point(13, 43)
        Me.List_Parts.Name = "List_Parts"
        Me.List_Parts.Size = New System.Drawing.Size(275, 186)
        Me.List_Parts.TabIndex = 3
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.Filter = "All files|*.*"
        Me.OpenFileDialog1.Title = "Select Animation File"
        '
        'SaveFileDialog1
        '
        Me.SaveFileDialog1.Filter = "YANM|*.yanm"
        Me.SaveFileDialog1.Title = "Select Save Location"
        '
        'OpenYanmDialog
        '
        Me.OpenYanmDialog.FileName = "*.yanm"
        Me.OpenYanmDialog.Filter = """YANM|*.yanm|All Files|*.*"""
        Me.OpenYanmDialog.Title = "Select YANM file"
        '
        'But_ExtAll
        '
        Me.But_ExtAll.Location = New System.Drawing.Point(223, 13)
        Me.But_ExtAll.Name = "But_ExtAll"
        Me.But_ExtAll.Size = New System.Drawing.Size(65, 23)
        Me.But_ExtAll.TabIndex = 4
        Me.But_ExtAll.Text = "Extract All"
        Me.But_ExtAll.UseVisualStyleBackColor = True
        '
        'Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(294, 241)
        Me.Controls.Add(Me.But_ExtAll)
        Me.Controls.Add(Me.List_Parts)
        Me.Controls.Add(Me.But_Inject)
        Me.Controls.Add(Me.But_Extract)
        Me.Controls.Add(Me.But_Open)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Main"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Animation Editor"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents But_Open As Button
    Friend WithEvents But_Extract As Button
    Friend WithEvents But_Inject As Button
    Friend WithEvents List_Parts As ListBox
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents SaveFileDialog1 As SaveFileDialog
    Friend WithEvents OpenYanmDialog As OpenFileDialog
    Friend WithEvents But_ExtAll As Button
    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
End Class
