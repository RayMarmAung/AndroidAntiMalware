Imports System.IO
Imports System.ComponentModel
Imports Telecom.Android
Public Class frmCleanOption

    Public adtFactoryReset As Boolean = False
    Public adtClearJunk As Boolean = False
    Public adtClearDalvik As Boolean = False
    Public adtCleanIntSd As Boolean = False
    Public adtCleanExtSd As Boolean = False

    Dim drag As Boolean
    Dim mousex As Integer
    Dim mousey As Integer
    Private Sub PictureBox8_Click(sender As Object, e As EventArgs) Handles PictureBox8.Click
        Me.Close()
    End Sub

#Region "CLOSE BUTTON EVENT"
    Private Sub PictureBox8_MouseLeave(sender As Object, e As EventArgs) Handles PictureBox8.MouseLeave
        PictureBox8.Image = Image.FromStream(New FileStream("Resources\AdbWinD.dll", FileMode.Open, FileAccess.Read))
    End Sub
    Private Sub PictureBox8_MouseMove(sender As Object, e As MouseEventArgs) Handles PictureBox8.MouseMove
        PictureBox8.Image = Image.FromStream(New FileStream("Resources\AdbWinC.dll", FileMode.Open, FileAccess.Read))
    End Sub
#End Region

#Region "FORM MOVE"
    Private Sub A1Panel2_MouseDown(sender As Object, e As MouseEventArgs) Handles A1Panel2.MouseDown, PictureBox11.MouseDown, PictureBox3.MouseDown, Label1.MouseDown
        drag = True
        mousex = Windows.Forms.Cursor.Position.X - Me.Left
        mousey = Windows.Forms.Cursor.Position.Y - Me.Top
    End Sub
    Private Sub A1Panel2_MouseMove(sender As Object, e As MouseEventArgs) Handles A1Panel2.MouseMove, PictureBox11.MouseMove, PictureBox3.MouseMove, Label1.MouseMove
        If drag Then
            Me.Top = Windows.Forms.Cursor.Position.Y - mousey
            Me.Left = Windows.Forms.Cursor.Position.X - mousex
        End If
    End Sub
    Private Sub A1Panel2_MouseUp(sender As Object, e As MouseEventArgs) Handles A1Panel2.MouseUp, PictureBox11.MouseUp, PictureBox3.MouseUp, Label1.MouseUp
        drag = False
    End Sub
    Private Sub frmCleanOption_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim bwkCheckExtSd As New BackgroundWorker
        AddHandler bwkCheckExtSd.DoWork, New DoWorkEventHandler(AddressOf CheckExtSdDoWork)
        bwkCheckExtSd.RunWorkerAsync()

        While bwkCheckExtSd.IsBusy
            Application.DoEvents()
            System.Threading.Thread.Sleep(100)
        End While

        If Not extSdcard = "" Then
            chkExtSd.Enabled = True
            lblExtSd.ForeColor = Color.GreenYellow
        Else
            chkExtSd.Enabled = False
            lblExtSd.ForeColor = Color.White
        End If

    End Sub
    Private Sub CheckExtSdDoWork()

        intSdcard = ""

        Dim Intsd As String = Adb.RunAdbShellCommand(Device, False, "ls -al /")
        Using r As New StringReader(Intsd)
            Dim line As String

            While Not r.Peek = -1
                line = r.ReadLine

                If line.Contains("sdcard -> /") Then
                    Dim spl() As String = line.Split(" ")
                    intSdcard = spl(spl.Length - 1)
                End If

            End While
        End Using

        Dim Storage As String = Adb.RunAdbShellCommand(Device, False, "df")
        Using r As New StringReader(Storage)
            Dim line As String
            While Not r.Peek = -1
                line = r.ReadLine

                If line.StartsWith("/mnt") Or line.StartsWith("/storage") Then
                    Dim spl() As String = line.Split(" ")
                    Dim name As String = spl(0)

                    If name.EndsWith("SdCard") Or name.EndsWith("card1") Then
                        extSdcard = name
                    End If

                End If

            End While

        End Using


    End Sub
    Private Sub chkFactoryReset_CheckedChanged(sender As Object, e As EventArgs) Handles chkFactoryReset.CheckedChanged
        If chkFactoryReset.Checked Then
            chkJunkClean.Checked = False
            chkJunkClean.Enabled = False
            lblCleanJunk.ForeColor = Color.White
            chkDalvik.Checked = False
            chkDalvik.Enabled = False
            lblDalvik.ForeColor = Color.White
        Else
            chkJunkClean.Enabled = True
            lblCleanJunk.ForeColor = Color.GreenYellow
            chkDalvik.Enabled = True
            lblDalvik.ForeColor = Color.GreenYellow
        End If

        checkButton()

    End Sub
    Private Sub lblFactoryReset_Click(sender As Object, e As EventArgs) Handles lblFactoryReset.Click
        If Not lblFactoryReset.ForeColor = Color.White Then
            If chkFactoryReset.Checked Then
                chkFactoryReset.Checked = False
            Else
                chkFactoryReset.Checked = True
            End If

            checkButton()

        End If
    End Sub
    Private Sub lblCleanJunk_Click(sender As Object, e As EventArgs) Handles lblCleanJunk.Click
        If Not lblCleanJunk.ForeColor = Color.White Then
            If chkJunkClean.Checked Then
                chkJunkClean.Checked = False
            Else
                chkJunkClean.Checked = True
            End If

            checkButton()

        End If
    End Sub
    Private Sub lblDalvik_Click(sender As Object, e As EventArgs) Handles lblDalvik.Click
        If Not lblDalvik.ForeColor = Color.White Then
            If chkDalvik.Checked Then
                chkDalvik.Checked = False
            Else
                chkDalvik.Checked = True
            End If

            checkButton()

        End If
    End Sub
    Private Sub lblCleanIntSd_Click(sender As Object, e As EventArgs) Handles lblCleanIntSd.Click
        If Not lblCleanIntSd.ForeColor = Color.White Then
            If chkCleanIntSd.Checked Then
                chkCleanIntSd.Checked = False
            Else
                chkCleanIntSd.Checked = True
            End If

            checkButton()

        End If
    End Sub
    Private Sub lblExtSd_Click(sender As Object, e As EventArgs) Handles lblExtSd.Click
        If lblExtSd.ForeColor = Color.GreenYellow Then
            If chkExtSd.Checked Then
                chkExtSd.Checked = False
            Else
                chkExtSd.Checked = True
            End If

            checkButton()

        End If
    End Sub
    Private Sub checkButton()
        If chkFactoryReset.Checked Or chkJunkClean.Checked Or chkDalvik.Checked Or chkExtSd.Checked Or chkCleanIntSd.Checked Then
            butClean.ForeColor = Color.GreenYellow
            butClean.BorderColor = Color.GreenYellow
        Else
            butClean.ForeColor = Color.White
            butClean.BorderColor = Color.White
        End If
    End Sub
    Private Sub chkJunkClean_CheckedChanged(sender As Object, e As EventArgs) Handles chkJunkClean.CheckedChanged, chkDalvik.CheckedChanged, chkCleanIntSd.CheckedChanged, chkExtSd.CheckedChanged
        checkButton()
    End Sub
    Private Sub butClean_ClickButtonArea(Sender As Object, e As MouseEventArgs) Handles butClean.ClickButtonArea

        If chkFactoryReset.Checked Then
            adtFactoryReset = True
        End If

        If chkJunkClean.Checked Then
            adtClearJunk = True
        End If

        If chkDalvik.Checked Then
            adtClearDalvik = True
        End If

        If chkCleanIntSd.Checked Then
            adtCleanIntSd = True
        End If

        If chkExtSd.Checked Then
            adtCleanExtSd = True
        End If

        Me.Close()

    End Sub
    Private Sub butCancel_ClickButtonArea(Sender As Object, e As MouseEventArgs) Handles butCancel.ClickButtonArea
        Me.Close()
    End Sub

#End Region

End Class