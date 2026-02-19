Public Class frmConvertSTL

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnBrowseASCII_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseASCII.Click
        Try
            With OpenFileDialog1
                .CheckFileExists = True
                .Filter = "STL File (*.stl)|*.stl"
                .FilterIndex = 0
                .ReadOnlyChecked = False
                .ShowDialog()
                If .FileName <> "" Then
                    txtASCIIFilename.Text = .FileName
                    txtBinaryFilename.Text = .FileName
                End If
            End With
        Catch ex As Exception
            MsgBox("Error getting ASCII STL filename.")
        End Try
    End Sub

    Private Sub btnBrowseBinary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseBinary.Click
        Try
            With SaveFileDialog1
                .AddExtension = True
                .Filter = "STL File (*.stl)|*.stl"
                .FilterIndex = 0
                .ShowDialog()
                If .FileName <> "" Then
                    txtBinaryFilename.Text = .FileName
                End If
            End With
        Catch ex As Exception
            MsgBox("Error getting Binary STL filename.")
        End Try
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Try
            ' Validate the filenames.
            If Not System.IO.File.Exists(txtASCIIFilename.Text) Then
                MsgBox("The specified ASCII file does not exist.")
                Exit Sub
            End If

            If System.IO.File.Exists(txtBinaryFilename.Text) Then
                If MsgBox("The specified output file already exists.  Do you want to overwrite it?", MsgBoxStyle.Question And MsgBoxStyle.YesNo, "File Exists") = MsgBoxResult.No Then
                    Exit Sub
                End If
            End If

            ' Check to see if it's a binary or ASCII stl file.
            Dim streamReader As New System.IO.StreamReader(txtASCIIFilename.Text)
            Dim checkString As String = streamReader.ReadLine
            checkString = streamReader.ReadLine
            streamReader.Close()

            If Not checkString.Contains("facet") Then
                MsgBox("The specified ASCII STL file is not a valid ASCII STL file.")
                Exit Sub
            End If

            Dim vertexCoords() As Double = {}
            If ReadASCIISTLFile(txtASCIIFilename.Text, vertexCoords) Then
                If Not WriteBinarySTLFile(txtBinaryFilename.Text, vertexCoords) Then
                    MsgBox("Unexpected error write the binary STL file.")
                End If
            Else
                MsgBox("Unexpected error reading the ASCII STL file.")
            End If

            Me.Close()
        Catch ex As Exception
            MsgBox("Unexpected error converting file.")
        End Try
    End Sub

    Private Sub frmConvertSTL_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class

Public Module Startup
    Public Sub Main()
        Try
            ' Check to see if there are command line arguments.
            Dim errorFound As Boolean = False
            If My.Application.CommandLineArgs.Count = 2 Then
                Dim asciiFilename As String = My.Application.CommandLineArgs.Item(0)
                If System.IO.File.Exists(asciiFilename) Then
                    Dim binaryFilename As String = My.Application.CommandLineArgs.Item(1)

                    Dim vertexCoords() As Double = {}
                    If ReadASCIISTLFile(asciiFilename, vertexCoords) Then
                        If Not WriteBinarySTLFile(binaryFilename, vertexCoords) Then
                            errorFound = True
                        Else
                            Dim coords() As Double = {}
                            Dim indices() As Integer = {}
                            ReadBinarySTLFile(binaryFilename, coords, indices)
                        End If
                    Else
                        errorFound = True
                    End If
                Else
                    errorFound = True
                End If
            Else
                errorFound = True
            End If

            If errorFound Then
                Dim convertForm As New frmConvertSTL
                convertForm.ShowDialog()
            End If
        Catch ex As Exception
            MsgBox("Unexpected error running utility.")
        End Try
    End Sub

    Public Function WriteBinarySTLFile(ByVal Filename As String, ByRef vertexCoords() As Double) As Boolean
        Dim fileStream As System.IO.FileStream = Nothing
        Dim binaryWriter As System.IO.BinaryWriter = Nothing
        Try
            ' Create an instance of StreamReader to read from a file.
            fileStream = New System.IO.FileStream(Filename, IO.FileMode.Create, IO.FileAccess.Write)
            binaryWriter = New System.IO.BinaryWriter(fileStream)

            ' Create and write the header.
            Dim header(79) As Char
            header(0) = "s"c
            header(1) = "t"c
            header(2) = "l"c
            header(3) = " "c
            header(4) = "b"c
            header(5) = "i"c
            header(6) = "n"c
            header(7) = "a"c
            header(8) = "r"c
            header(9) = "y"c
            binaryWriter.Write(header)

            Dim facetCount As UInt32 = CType(vertexCoords.Length / 9, UInt32)
            binaryWriter.Write(facetCount)

            ' Write the facet information
            Dim i As Integer
            For i = 0 To CType(facetCount, Integer) - 1
                ' Write the normal
                Dim Zero As Single
                binaryWriter.Write(Zero)
                binaryWriter.Write(Zero)
                binaryWriter.Write(Zero)

                ' Write the coordinates.
                binaryWriter.Write(CType(vertexCoords(i * 9), Single))
                binaryWriter.Write(CType(vertexCoords(i * 9 + 1), Single))
                binaryWriter.Write(CType(vertexCoords(i * 9 + 2), Single))

                binaryWriter.Write(CType(vertexCoords(i * 9 + 3), Single))
                binaryWriter.Write(CType(vertexCoords(i * 9 + 4), Single))
                binaryWriter.Write(CType(vertexCoords(i * 9 + 5), Single))

                binaryWriter.Write(CType(vertexCoords(i * 9 + 6), Single))
                binaryWriter.Write(CType(vertexCoords(i * 9 + 7), Single))
                binaryWriter.Write(CType(vertexCoords(i * 9 + 8), Single))

                ' Write the additional two bytes.
                Dim junkByte As Byte = 0
                binaryWriter.Write(junkByte)
                binaryWriter.Write(junkByte)
            Next

            binaryWriter.Close()

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function


    Public Function ReadBinarySTLFile(ByVal Filename As String, ByRef vertexCoords() As Double, ByRef vertexIndices() As Integer) As Boolean
        Dim fileStream As System.IO.FileStream = Nothing
        Dim binaryReader As System.IO.BinaryReader = Nothing
        Try
            ' Create an instance of StreamReader to read from a file.
            fileStream = New System.IO.FileStream(Filename, IO.FileMode.Open, IO.FileAccess.Read)
            binaryReader = New System.IO.BinaryReader(fileStream)

            Dim lastPercent As Integer = -1

            ' Read the 80 character header.
            Dim junkChars() As Char
            junkChars = binaryReader.ReadChars(80)

            ' Read the number of triangles.
            Dim triangleCount As Int32 = binaryReader.ReadInt32

            Dim vertexCount As Integer = 0
            ReDim vertexCoords(triangleCount * 3 * 3 - 1)
            ReDim vertexIndices(triangleCount * 3 - 1)

            ' Read the triangle data.
            Dim i As Integer
            For i = 1 To triangleCount
                Dim currentPercent As Integer = CInt((fileStream.Position / fileStream.Length) * 100)

                If currentPercent <> lastPercent Then
                    lastPercent = currentPercent
                End If

                ' Read off the normal.
                Dim junk As Single
                junk = binaryReader.ReadSingle
                junk = binaryReader.ReadSingle
                junk = binaryReader.ReadSingle

                ' Read the next three lines, which are the vertices of this triangle.
                vertexCoords(vertexCount * 3) = CDbl(binaryReader.ReadSingle)
                vertexCoords(vertexCount * 3 + 1) = CDbl(binaryReader.ReadSingle)
                vertexCoords(vertexCount * 3 + 2) = CDbl(binaryReader.ReadSingle)
                vertexIndices(vertexCount) = vertexCount + 1
                vertexCount += 1

                vertexCoords(vertexCount * 3) = CDbl(binaryReader.ReadSingle)
                vertexCoords(vertexCount * 3 + 1) = CDbl(binaryReader.ReadSingle)
                vertexCoords(vertexCount * 3 + 2) = CDbl(binaryReader.ReadSingle)
                vertexIndices(vertexCount) = vertexCount + 1
                vertexCount += 1

                vertexCoords(vertexCount * 3) = CDbl(CDbl(binaryReader.ReadSingle))
                vertexCoords(vertexCount * 3 + 1) = CDbl(CDbl(binaryReader.ReadSingle))
                vertexCoords(vertexCount * 3 + 2) = CDbl(CDbl(binaryReader.ReadSingle))
                vertexIndices(vertexCount) = vertexCount + 1
                vertexCount += 1

                ' Read the extra 2 bytes.
                Dim junkByte As Byte = binaryReader.ReadByte
                junkByte = binaryReader.ReadByte
            Next

            Return True
        Catch ex As System.IO.EndOfStreamException
            binaryReader.Close()
            fileStream.Close()
            Return True
        Catch ex As Exception
            If Not binaryReader Is Nothing Then
                binaryReader.Close()
            End If

            If Not fileStream Is Nothing Then
                fileStream.Close()
            End If

            Return False
        End Try
    End Function

    Public Function ReadASCIISTLFile(ByVal Filename As String, ByRef vertexCoords() As Double) As Boolean
        Dim vertexCount As Integer = 0
        Dim maxVertexCount As Integer = 99
        ReDim vertexCoords(maxVertexCount * 3 - 1)

        Try
            Dim fileInfo As New System.IO.FileInfo(Filename)
            Dim fileLength As Long = fileInfo.Length

            ' Create an instance of StreamReader to read from a file.
            Dim streamreader As System.IO.StreamReader = fileInfo.OpenText
            Dim lengthRead As Long = 0
            Dim lastPercent As Integer = -1

            Dim inputLine As String
            Do
                inputLine = streamreader.ReadLine()
                If streamreader.EndOfStream Then
                    lengthRead = fileLength
                Else
                    lengthRead = lengthRead + inputLine.Length
                End If
                Dim currentPercent As Integer = CInt((lengthRead / fileLength) * 100)

                If currentPercent <> lastPercent Then
                    lastPercent = currentPercent
                End If

                If Not inputLine Is Nothing Then
                    Dim astrFields() As String
                    astrFields = GetFields(inputLine)

                    ' Check to see if there is any data.
                    If astrFields(0) <> "" Then
                        ' Save the appropriate data.
                        Select Case astrFields(0).ToLower.Trim
                            Case "outer"
                                ' Make sure the coordinate array is large enough.
                                If vertexCount + 3 > maxVertexCount Then
                                    maxVertexCount = maxVertexCount + 99
                                    ReDim Preserve vertexCoords(maxVertexCount * 3 - 1)
                                End If
                                ' Read the next three lines, which are the vertices of this triangle.
                                inputLine = streamreader.ReadLine()
                                lengthRead = lengthRead + inputLine.Length
                                astrFields = GetFields(inputLine)
                                vertexCoords(vertexCount * 3) = CDbl(astrFields(1))
                                vertexCoords(vertexCount * 3 + 1) = CDbl(astrFields(2))
                                vertexCoords(vertexCount * 3 + 2) = CDbl(astrFields(3))
                                vertexCount += 1

                                inputLine = streamreader.ReadLine()
                                lengthRead = lengthRead + inputLine.Length
                                astrFields = GetFields(inputLine)
                                vertexCoords(vertexCount * 3) = CDbl(astrFields(1))
                                vertexCoords(vertexCount * 3 + 1) = CDbl(astrFields(2))
                                vertexCoords(vertexCount * 3 + 2) = CDbl(astrFields(3))
                                vertexCount += 1

                                inputLine = streamreader.ReadLine()
                                lengthRead = lengthRead + inputLine.Length
                                astrFields = GetFields(inputLine)
                                vertexCoords(vertexCount * 3) = CDbl(astrFields(1))
                                vertexCoords(vertexCount * 3 + 1) = CDbl(astrFields(2))
                                vertexCoords(vertexCount * 3 + 2) = CDbl(astrFields(3))
                                vertexCount += 1
                            Case Else
                        End Select
                    End If
                End If
            Loop While Not streamreader.EndOfStream

            ' Resize the arrays to the exact size needed.
            ReDim Preserve vertexCoords(vertexCount * 3 - 1)

            streamreader.Close()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function GetFields(ByVal InputString As String) As String()
        ' Get the fields as defined by a space.
        Dim astrTemp() As String
        astrTemp = Split(InputString, " ")

        ' Count the number of non-empty strings returned.
        Dim i As Integer
        Dim iFieldCount As Integer
        iFieldCount = 0
        For i = 0 To UBound(astrTemp)
            If astrTemp(i) <> "" Then
                iFieldCount = iFieldCount + 1
            End If
        Next

        Dim astrReturnFields() As String
        If iFieldCount > 0 Then
            ' Allocate an array to hold the non-empty string.
            ReDim astrReturnFields(iFieldCount - 1)

            ' Load the array with the non-empty strings.
            iFieldCount = 0
            For i = 0 To UBound(astrTemp)
                If astrTemp(i) <> "" Then
                    astrReturnFields(iFieldCount) = astrTemp(i)
                    iFieldCount = iFieldCount + 1
                End If
            Next
        Else
            ' It's an empty line so send back an array with a single empty string.
            ReDim astrReturnFields(0)
            astrReturnFields(0) = ""
        End If

        GetFields = astrReturnFields
    End Function
End Module