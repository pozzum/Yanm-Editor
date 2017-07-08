Imports System.IO    'Files
Imports System
Imports System.Globalization
Imports System.Object
Imports System.Text
Public Class Main
    Dim active_file As String
    Dim header As Byte()
    Dim Individual_Parts(20) As Part_Class
    Dim NumCharacters As String = "0123456789"
    Private Sub But_Open_Click(sender As Object, e As EventArgs) Handles But_Open.Click
        ResetForm()
        If OpenFileDialog1.ShowDialog = System.Windows.Forms.DialogResult.OK Then
            active_file = OpenFileDialog1.FileName
            If File.Exists(active_file) Then
                File.Copy(active_file, active_file & ".org", True)
                ReadCamera(active_file)
            End If
        End If
    End Sub
    Private Sub ResetForm(Optional keepfile As Boolean = False)
        If keepfile = False Then
            active_file = ""
        End If
        header = New Byte() {}
        List_Parts.Items.Clear()
        For i As Integer = 0 To 20
            Individual_Parts(i) = New Part_Class
        Next
    End Sub

    Private Sub ReadCamera(ByVal Source As String)
        header = ReadFile(Source, 0, BitConverter.ToInt32(ReadFile(Source, 0, 4, True), 0) + &H20)
        Dim YANM_Length As Integer = BitConverter.ToInt32(ReadFile(Source, 4, 4, True), 0)
        Dim Head_Index As Integer = 0
        Dim part_count As Integer = 0
        Do While Head_Index < header.Length
            If Head_Index = 0 Then
                Head_Index = &H70
            End If
            'getting the part name
            Individual_Parts(part_count).name = System.Text.Encoding.ASCII.GetString((ReadFile(Source, Head_Index + 4, 8)))
            'getting the start offset
            Individual_Parts(part_count).Start_offset = BitConverter.ToInt32(ReadFile(Source, Head_Index + &H24, 4, True), 0) + header.Length
            'Get the file Length
            If Head_Index + &H20 + &H28 < header.Length Then
                Individual_Parts(part_count).length = BitConverter.ToInt32(ReadFile(Source, Head_Index + &H24 + &H28, 4, True), 0) + header.Length - Individual_Parts(part_count).Start_offset
            Else
                Individual_Parts(part_count).length = YANM_Length - Individual_Parts(part_count).Start_offset + header.Length
            End If
            'Get Animation File 
            Individual_Parts(part_count).Animation_File = ReadFile(Source, Individual_Parts(part_count).Start_offset, Individual_Parts(part_count).length)
            'add to list box
            List_Parts.Items.Add(Individual_Parts(part_count).name)
            'List_Parts.Items.Add(part_count.ToString + 1 & ". " & Individual_Parts(part_count).name &
            '" Start: " & Hex(Individual_Parts(part_count).Start_offset) &
            '" Length: " & Hex(Individual_Parts(part_count).length))
            part_count = part_count + 1
            Head_Index = Head_Index + &H28
        Loop
        If List_Parts.Items.Count > 0 Then
            List_Parts.SetSelected(0, True)
        End If
    End Sub
    Shared Function ReadFile(Filepath As String, pos As Long, requiredbytes As Integer, Optional reverse As Boolean = False) As Byte()
        'Dim pos As Long = 6160
        'Dim requiredBytes As Integer = 12
        Dim value(0 To requiredbytes - 1) As Byte
        Using reader As New BinaryReader(File.Open(Filepath, FileMode.Open))
            ' Loop through length of file.
            Dim fileLength As Long = reader.BaseStream.Length
            Dim byteCount As Integer = 0
            reader.BaseStream.Seek(pos, SeekOrigin.Begin)
            While pos < fileLength And byteCount < requiredbytes
                value(byteCount) = reader.ReadByte()
                pos += 1
                byteCount += 1
            End While
        End Using
        If reverse = True Then
            Array.Reverse(value, 0, value.Length)
        End If
        Return value
    End Function
    Shared Function Write2File(Filepath As String, pos As Long, data As Byte()) As Boolean
        'Dim pos As Long = 6160
        'Dim requiredBytes As Integer = 12
        Using writer As New BinaryWriter(File.Open(Filepath, FileMode.Open))
            ' Loop through length of file.
            Dim fileLength As Long = writer.BaseStream.Length
            writer.BaseStream.Seek(pos, SeekOrigin.Begin)
            writer.Write(data)
        End Using
        Return True
    End Function


    Private Sub But_Extract_Click(sender As Object, e As EventArgs) Handles But_Extract.Click

        If List_Parts.SelectedIndex <> -1 Then
            SaveFileDialog1.FileName = List_Parts.SelectedItem & ".yanm"
            If SaveFileDialog1.ShowDialog = DialogResult.OK Then
                    'My.Computer.FileSystem.WriteAllBytes(SaveFileDialog1.FileName, ReadFile(active_file, Individual_Parts(tempindex).Start_offset, Individual_Parts(tempindex).length), False)
                    'My.Computer.FileSystem.WriteAllBytes(SaveFileDialog1.FileName, Individual_Parts(tempindex).Animation_File, False)
                    Extract_Yanm(SaveFileDialog1.FileName, List_Parts.SelectedItem)
                    MessageBox.Show("File Saved")
                End If

            Else
            MessageBox.Show("No Part Selected")
        End If
    End Sub
    Private Function GetPartByString(Value As String)
        For i As Integer = 0 To Individual_Parts.Length
            If Individual_Parts(i).name = Value Then
                Return i
            End If
        Next
        Return -1
    End Function

    Private Sub But_Inject_Click(sender As Object, e As EventArgs) Handles But_Inject.Click
        Dim Temp_Yanm As String
        If List_Parts.SelectedIndex <> -1 Then
            If OpenYanmDialog.ShowDialog = System.Windows.Forms.DialogResult.OK Then
                Temp_Yanm = OpenYanmDialog.FileName
                File.Copy(active_file, active_file & ".bak", True)
                Dim Total_Array As Byte() = File.ReadAllBytes(active_file)
                Dim Yanm_Array As Byte() = File.ReadAllBytes(Temp_Yanm)
                Dim Temp_Index As Integer = GetPartByString(List_Parts.SelectedItem)
                Dim Length_Diff As Integer = Yanm_Array.Length - Individual_Parts(Temp_Index).length
                'MessageBox.Show(Length_Diff.ToString)
                Dim YANM_Length As Integer = BitConverter.ToInt32({Total_Array(7), Total_Array(6), Total_Array(5), Total_Array(4)}, 0)
                Dim Length_array As Byte() = System.BitConverter.GetBytes(YANM_Length + Length_Diff)
                Array.Reverse(Length_array, 0, Length_array.Length)
                Buffer.BlockCopy(Length_array, 0, Total_Array, 4, 4)
                For i As Integer = Temp_Index + 1 To 20
                    If Individual_Parts(i).Start_offset <> 0 Then
                        Individual_Parts(i).Start_offset = Individual_Parts(i).Start_offset + Length_Diff
                        Dim Offset_Array As Byte() = System.BitConverter.GetBytes(Individual_Parts(i).Start_offset - header.Length)
                        Array.Reverse(Offset_Array, 0, Offset_Array.Length)
                        Buffer.BlockCopy(Offset_Array, 0, Total_Array, &H70 + i * &H28 + &H24, 4)
                    End If
                Next
                Dim Final_Array As Byte()
                Final_Array = New Byte(Total_Array.Length + Length_Diff - 1) {}
                Buffer.BlockCopy(Total_Array, 0, Final_Array, 0, Individual_Parts(Temp_Index).Start_offset)
                Buffer.BlockCopy(Yanm_Array, 0, Final_Array, Individual_Parts(Temp_Index).Start_offset, Yanm_Array.Length)
                If Individual_Parts(Temp_Index + 1).Start_offset <> 0 Then
                    Buffer.BlockCopy(Total_Array,
                                     Individual_Parts(Temp_Index + 1).Start_offset - Length_Diff,
                                     Final_Array,
                                     Individual_Parts(Temp_Index + 1).Start_offset,
                                     Total_Array.Length - (Individual_Parts(Temp_Index + 1).Start_offset - Length_Diff))
                End If
                File.WriteAllBytes(active_file, Final_Array)
                    MessageBox.Show("File Saved")
                End If
            Else
            MessageBox.Show("No Part Selected")
        End If
    End Sub

    Private Sub But_ExtAll_Click(sender As Object, e As EventArgs) Handles But_ExtAll.Click
        FolderBrowserDialog1.SelectedPath = Path.GetDirectoryName(active_file)
        If FolderBrowserDialog1.ShowDialog = DialogResult.OK Then
            For Each AnimPart As String In List_Parts.Items
                Extract_Yanm(FolderBrowserDialog1.SelectedPath & "\" & AnimPart & ".yanm", AnimPart)
            Next
            MessageBox.Show("Mass Extract Finished")
        End If
    End Sub

    Private Sub Extract_Yanm(SavePath As String, AnimPart As String)
        Dim tempindex As Integer = GetPartByString(AnimPart)
        If tempindex <> -1 Then
            My.Computer.FileSystem.WriteAllBytes(SavePath, Individual_Parts(tempindex).Animation_File, False)
        Else
            MessageBox.Show("Error Finding Part " & AnimPart)
        End If
    End Sub

    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub But_Rem_Click(sender As Object, e As EventArgs) Handles But_Rem.Click
        If List_Parts.SelectedIndex <> -1 Then
            File.Copy(active_file, active_file & ".bak", True)
            Dim Total_Array As Byte() = File.ReadAllBytes(active_file)
            Dim Temp_Index As Integer = GetPartByString(List_Parts.SelectedItem)
            Dim Length_Diff As Integer = Individual_Parts(Temp_Index).length
            'MessageBox.Show(Length_Diff.ToString)
            Dim YANM_Length As Integer = BitConverter.ToInt32({Total_Array(7), Total_Array(6), Total_Array(5), Total_Array(4)}, 0)
            Dim Length_array As Byte() = System.BitConverter.GetBytes(YANM_Length - Length_Diff)
            Array.Reverse(Length_array, 0, Length_array.Length)
            Buffer.BlockCopy(Length_array, 0, Total_Array, 4, 4)
            Dim Head_Length As Integer = BitConverter.ToInt32({Total_Array(3), Total_Array(2), Total_Array(1), Total_Array(0)}, 0)
            Dim HeadLength_array As Byte() = System.BitConverter.GetBytes(Head_Length - &H28)
            Array.Reverse(HeadLength_array, 0, HeadLength_array.Length)
            Buffer.BlockCopy(HeadLength_array, 0, Total_Array, 0, 4)
            For i As Integer = Temp_Index + 1 To 20
                If Individual_Parts(i).Start_offset <> 0 Then
                    Individual_Parts(i).Start_offset = Individual_Parts(i).Start_offset - Length_Diff
                    Dim Offset_Array As Byte() = System.BitConverter.GetBytes(Individual_Parts(i).Start_offset - header.Length)
                    Array.Reverse(Offset_Array, 0, Offset_Array.Length)
                    Buffer.BlockCopy(Offset_Array, 0, Total_Array, &H70 + i * &H28 + &H24, 4)
                End If
            Next
            Dim Final_Array As Byte()
            Final_Array = New Byte(Total_Array.Length - Length_Diff - 1 - &H28 + &H1000) {}
            'Header Before the Removed Part
            Buffer.BlockCopy(Total_Array, 0, Final_Array, 0, &H70 + &H28 * Temp_Index)
            'Header After the Removed Part
            Buffer.BlockCopy(Total_Array, &H70 + &H28 * (Temp_Index + 1), Final_Array, &H70 + &H28 * Temp_Index, Individual_Parts(Temp_Index).Start_offset - (&H70 + &H28 * (Temp_Index + 1)))
            'Parts After the Removed Part
            If Individual_Parts(Temp_Index + 1).Start_offset <> 0 Then
                Buffer.BlockCopy(Total_Array,
                                     Individual_Parts(Temp_Index + 1).Start_offset + Length_Diff,
                                     Final_Array,
                                     Individual_Parts(Temp_Index + 1).Start_offset - &H28,
                                     Total_Array.Length - (Individual_Parts(Temp_Index + 1).Start_offset + Length_Diff))
            End If
            File.WriteAllBytes(active_file, Final_Array)
            ResetForm(True)
            ReadCamera(active_file)
            MessageBox.Show("File Saved")
        Else
            MessageBox.Show("No Part Selected")
        End If
    End Sub

    Private Sub List_Parts_SelectedIndexChanged(sender As Object, e As EventArgs) Handles List_Parts.SelectedIndexChanged
        Text_Name.Text = List_Parts.SelectedItem
    End Sub

    Private Sub Text_Name_TextChanged(sender As Object, e As EventArgs) Handles Text_Name.TextChanged
        Dim theText As String = Text_Name.Text
        Dim Letter As String
        Dim SelectionIndex As Integer = Text_Name.SelectionStart
        Dim Change As Integer

        For x As Integer = 0 To Text_Name.Text.Length - 1
            Letter = Text_Name.Text.Substring(x, 1)
            If NumCharacters.Contains(Letter) = False Then
                theText = theText.Replace(Letter, String.Empty)
                Change = 1
            End If
        Next

        Text_Name.Text = theText
        If SelectionIndex <> Nothing Then
            Text_Name.Select(SelectionIndex - Change, 0)
        End If
    End Sub

    Private Sub But_Rename_Click(sender As Object, e As EventArgs) Handles But_Rename.Click
        If List_Parts.SelectedIndex <> -1 Then
            Dim New_Name As String = Text_Name.Text.PadLeft(8, "0")
            File.Copy(active_file, active_file & ".bak", True)
            Dim Total_Array As Byte() = File.ReadAllBytes(active_file)
            Dim Name_array As Byte() = Encoding.ASCII.GetBytes(New_Name)
            Buffer.BlockCopy(Name_array, 0, Total_Array, &H74 + &H28 * List_Parts.SelectedIndex, 8)
            File.WriteAllBytes(active_file, Total_Array)
            ResetForm(True)
            ReadCamera(active_file)
            MessageBox.Show("File Saved")
        Else
            MessageBox.Show("No Part Selected")
        End If
    End Sub

    Private Sub But_Add_Click(sender As Object, e As EventArgs) Handles But_Add.Click
        Dim Temp_Yanm As String
        If OpenYanmDialog.ShowDialog = System.Windows.Forms.DialogResult.OK Then
            Temp_Yanm = OpenYanmDialog.FileName
            File.Copy(active_file, active_file & ".bak", True)
            Dim Total_Array As Byte() = File.ReadAllBytes(active_file)
            Dim Yanm_Array As Byte() = File.ReadAllBytes(Temp_Yanm)
            Dim Length_Diff As Integer = Yanm_Array.Length
            'Changing Header Length
            Dim New_Head_Length As Integer = BitConverter.ToInt32({Total_Array(3), Total_Array(2), Total_Array(1), Total_Array(0)}, 0)
            Dim New_Head_Array As Byte() = System.BitConverter.GetBytes(New_Head_Length + &H28)
            Array.Reverse(New_Head_Array, 0, New_Head_Array.Length)
            Buffer.BlockCopy(New_Head_Array, 0, Total_Array, 0, 4)
            'Changing Yanm Length
            Dim YANM_Length As Integer = BitConverter.ToInt32({Total_Array(7), Total_Array(6), Total_Array(5), Total_Array(4)}, 0)
            Dim Length_array As Byte() = System.BitConverter.GetBytes(YANM_Length + Length_Diff)
            Array.Reverse(Length_array, 0, Length_array.Length)
            Buffer.BlockCopy(Length_array, 0, Total_Array, 4, 4)
            Dim Final_Array As Byte()
            Final_Array = New Byte(Total_Array.Length + Length_Diff - 1 + &H28) {}
            Dim Header_Part As Byte() = New Byte(&H28) {}
            Dim Name_Array As Byte() = {&HFF, &H0, &H0, &H74, &H30, &H30, &H30, &H30, &H30, &H30, &H30, &H30}
            Buffer.BlockCopy(Name_Array, 0, Header_Part, 0, Name_Array.Length)
            Dim Org_Length As Byte() = System.BitConverter.GetBytes(YANM_Length)
            Array.Reverse(Org_Length, 0, Org_Length.Length)
            Buffer.BlockCopy(Org_Length, 0, Header_Part, &H24, &H4)
            'getting the existing header
            Buffer.BlockCopy(Total_Array, 0, Final_Array, 0, header.Length)
            'adding the new header part
            Buffer.BlockCopy(Header_Part, 0, Final_Array, header.Length, Header_Part.Length)
            'getting the existing yanm
            Buffer.BlockCopy(Total_Array, header.Length, Final_Array, header.Length + &H28, YANM_Length)
            'adding the new yanm
            Buffer.BlockCopy(Yanm_Array, 0, Final_Array, header.Length + &H28 + YANM_Length, Yanm_Array.Length - 1)
            File.WriteAllBytes(active_file, Final_Array)
            MessageBox.Show("File Saved")
            ResetForm(True)
            ReadCamera(active_file)
        End If
    End Sub
End Class
