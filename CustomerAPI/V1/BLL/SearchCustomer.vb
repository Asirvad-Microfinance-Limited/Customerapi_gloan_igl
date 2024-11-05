Imports System.Collections.Generic
Imports System.IO
Imports System.Windows.Forms
Imports System.Net
Imports System.Text.RegularExpressions
Imports System.Drawing.Imaging
Imports System.Drawing
Imports System.Deployment
Imports System.Globalization

Imports System
Imports System.Data
Imports Syntizen.AUA.GatewayLib
Imports System.Web
Imports System.Configuration
Imports System.Runtime.Serialization
'Imports System.Web.Script.Serialization
Imports Newtonsoft.Json
Imports System.Xml
Imports Newtonsoft.Json.Linq
Imports Org.BouncyCastle.Crypto.Paddings
Imports Org.BouncyCastle.Crypto
Imports Org.BouncyCastle.Crypto.Engines
Imports Syntizen.DataVault.UUIDService

''' <summary>
''' 
''' </summary>
Public Class SearchCustomer
    Public Shared ip, serverip, serverurl As String



    Dim myProcess As Process = New Process()
    Public WithEvents thb As Windows.Forms.Button = New System.Windows.Forms.Button
    Public Declare Function SetParent Lib "user32" Alias "SetParent" (ByVal hWndChild As IntPtr, ByVal hWndNewParent As IntPtr) As System.IntPtr
#Region "Global Variable Declaration"
    Dim ModuleID As String
    Dim BranchID As String
    Dim FirmID, UserId As String
    Dim type As String
    Dim sear_type As String
    Dim dtmain, dtsub As New DataTable
    Dim cust_result As New DataTable
    Dim idtype, idno As String
    Dim wchelper As New CHelper.WebCamera.WebCam
    Dim photo(), neftPhoto(), panphoto(), kycPhoto(), licensePhoto(), brokerform() As Byte
    Dim cust() As Byte
    Dim stid, distid As Integer
    Dim neft_status As Integer = 0
    Dim dt As New DataSet
    Dim sql As String
    Dim WHELPER As New WindowsHelper.Windows.DataStore
    'Public Shared brno, fmno, userid As Integer
    Dim BranchName As String = ""
    Dim neftdetails As String = ""
    Dim ddt As New DataTable
    Dim flag As New Integer
    Dim citizen As Integer
    Dim kycStatus As Integer = 0
    Dim photoStatus As Integer = 0
    ' Dim webser_ip As String = "10.0.0.111"

    Dim webser_ip As String = "app.manappuram.net"

    Dim dumdt As New DataTable
    Dim tempPLoane As New DataTable
    Public Shared ss As Boolean = False
    Dim kycDocFile As String
    Dim kycDBFile As String
    Dim addProofID As String
    Dim fromMod As Boolean = False
    Dim custBranchID As String
    Dim isappend As Boolean = False
    Dim isKycDocAvailable As Boolean
    Dim licimg, formimg As String
    Public Shared ModuleOpenDate As Date
    Public Shared code As String

    Public Shared KycRemark As String
    Public Shared RemFlag As Integer
    Public Shared PhotoRemark As String

    Public ekyctrn, servType, Poiname, Poidob, Poigender, Poaco, Poadist, Poahouse, Poaloc, Poapc, Poastate, Poavtc, Poastreet, Poalm, Poasubdist, Poapo As String
    Public Pht() As Byte
    Public Shared rrn_n, UUID As String

    Dim NriFlg, PanFlg, CkycFlag, CtryFlg As String
    Dim UserFlg As Integer = 0
    Dim isactive As Integer = 0
    Dim CuName, FaName As String

#End Region
#Region "Customer Properties"
#Region "Read And Write "
    Public Shared Property ModuleOpen() As Date
        Get
            Return ModuleOpenDate
        End Get
        Set(ByVal value As Date)
            ModuleOpenDate = value
        End Set
    End Property
    Public Property Branch() As String
        Get
            Return BranchID
        End Get
        Set(ByVal value As String)
            BranchID = value
        End Set
    End Property
    Public Property Firm() As String
        Get
            Return FirmID
        End Get
        Set(ByVal value As String)
            FirmID = value
        End Set
    End Property
    Public Property Module_id() As String
        Get
            Return ModuleID
        End Get
        Set(ByVal value As String)
            ModuleID = value
        End Set
    End Property
    Public Property identify_type() As Integer
        Get
            Return type
        End Get
        Set(ByVal value As Integer)
            type = value
        End Set
    End Property
    Public ReadOnly Property customercount() As Integer
        Get
            If Not cust_result Is Nothing Then
                Return cust_result.Rows.Count
            End If
            Return 0
        End Get
    End Property
    Protected ReadOnly Property customerBindingcontext() As Windows.Forms.CurrencyManager
        Get
            If Not cust_result Is Nothing Then
                Return CType(BindingContext(cust_result), Windows.Forms.CurrencyManager)
            Else
                Return Nothing
            End If
        End Get
    End Property
    Public Shared objhashDSA As Hashtable
    Public Shared objhash As Hashtable
    Public Shared objRefdetails As DataTable
    Public Shared Property Address() As Hashtable
        Get
            Return objhash
        End Get
        Set(ByVal value As Hashtable)
            objhash = value
        End Set
    End Property
    Public Shared Property DSA_BA_Users() As Hashtable
        Get
            Return objhashDSA
        End Get
        Set(ByVal value As Hashtable)
            objhashDSA = value
        End Set
    End Property
    Public Shared Property Refdetails() As DataTable
        Get
            Return objRefdetails
        End Get
        Set(ByVal value As DataTable)
            objRefdetails = value
        End Set
    End Property
    'Public Property addneftdtl() As Integer
    '    Get
    '        Return neft_status
    '    End Get
    '    Set(ByVal value As Integer)
    '        neft_status = value
    '    End Set
    'End Property
    Public Property User_id() As String
        Get
            Return UserId
        End Get
        Set(ByVal value As String)
            UserId = value
        End Set
    End Property
    Public Property Branch_Name() As String
        Get
            Return BranchName
        End Get
        Set(ByVal value As String)
            BranchName = value
        End Set
    End Property


    Public Property cust_serv_ip() As String
        Get
            Return webser_ip
        End Get
        Set(ByVal value As String)

            'If value = "" Then
            '    webser_ip = "220.225.200.100"
            'Else
            webser_ip = value
            'End If
        End Set
    End Property
#End Region
#End Region
#Region "Customer search "
    'Private Sub rd_docid_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Me.lbl_searchnm.Text = "Document ID"
    '    Me.txt_search.MaxLength = 16
    'End Sub

    Private Sub rd_custid_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rd_custid.CheckedChanged
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.lbl_searchnm.Text = "Customer ID"
            Me.txt_search.MaxLength = 14
            Me.txt_search.Text = ""
            Dim dt As New DataTable
            dt.Clear()
            Me.dgsearchResult.DataSource = dt
            clear()
            Me.txt_search.Focus()
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub rd_custnm_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rd_custnm.CheckedChanged
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.lbl_searchnm.Text = "Customer Name"
            Me.txt_search.MaxLength = 40
            Dim dt As New DataTable
            dt.Clear()
            Me.dgsearchResult.DataSource = dt
            clear()
            Me.txt_search.Focus()
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub rd_CardId_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rd_CardId.CheckedChanged
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.lbl_searchnm.Text = "Card ID"
            Me.txt_search.MaxLength = 16
            Dim dt As New DataTable
            dt.Clear()
            Me.dgsearchResult.DataSource = dt
            clear()
            Me.txt_search.Focus()
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub txt_search_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_search.GotFocus
        Me.grp2.Visible = False
        Me.grp3.Visible = False
        Me.grp2.Height = 0
        Me.grp3.Height = 0
        Me.dgsearchResult.DataSource = ""
        kycStatus = 0
        neft_status = 0
        txtCustomerid.Text = ""
    End Sub

    Private Sub txt_search_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_search.KeyPress
        e.KeyChar = CChar(e.KeyChar.ToString.ToUpper)
    End Sub

    Private Sub txt_search_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_search.LostFocus
        '  Me.Main_tab.Visible = False
        If Me.rd_custid.Checked = True Then
            If Me.txt_search.Text.Trim <> "" AndAlso Not IsNumeric(Me.txt_search.Text) Then
                MsgBox("Enter Valid Customer ID", MsgBoxStyle.Exclamation)
                Me.txt_search.Text = ""
                'Me.txt_search.Focus()
            ElseIf Me.txt_search.Text.Trim <> "" Then
                Dim len As Integer = Me.txt_search.Text.Length
                If len < 3 Then
                    Me.txt_search.Text = ""
                    'Me.txt_search.Focus()
                    MsgBox("Enter Three character")
                    Exit Sub
                End If
                'If len <= 8 Then
                '    Me.txt_search.Text = CInt(FirmID).ToString.PadLeft(2, CChar("0")) & BranchID.ToString.PadLeft(4, CChar("0")) & Me.txt_search.Text.ToString.PadLeft(8, CChar("0"))
                'End If
            End If
        End If
    End Sub
    Private Sub cmd_search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_search.Click
        ' MsgBox(FirmID & "****" & BranchID & "*****" & ModuleID, MsgBoxStyle.Critical)
        Try
            'Me.Main_tab.TabPages(4).Visible = False
            If Me.txt_search.Text = "" Then
                MsgBox("Search field cannot be null....")
                Me.txt_search.Focus()
                Exit Sub
            End If
            'Me.Main_tab.TabPages(4).Visible = False
            Me.Cursor = Cursors.WaitCursor
            Dim obj As New customerService.customer(set_ip)
            Dim dt As New DataTable
            If Me.rd_custid.Checked = True Then
                sear_type = 1
            ElseIf rd_custnm.Checked = True Then
                sear_type = 2
            ElseIf Me.rd_CardId.Checked = True Then
                sear_type = 3
            ElseIf Me.rdoPan.Checked = True Then
                sear_type = 4
            ElseIf Me.rdoPhone.Checked = True Then
                sear_type = 5
            ElseIf Me.rdoIDNo.Checked = True Then
                sear_type = 6
            End If
            dt = obj.searchCustomer(Me.txt_search.Text, FirmID, BranchID, sear_type).Tables(0)

            obj.CustSearchLog(BranchID, UserId, sear_type, Me.txt_search.Text)

            If dt.Rows.Count > 0 Then
                Me.dgsearchResult.DataSource = dt

                Me.grp2.Height = 211
                Me.grp2.Visible = True
                Me.dgsearchResult.Height = 193
                dtsub = dt
            Else
                If sear_type = 1 Then
                    MsgBox("This Customer is not Present or not belongs to this branch. Enter Correct ID")
                ElseIf sear_type = 2 Then
                    MsgBox("This Customer is not Present or not belongs to this branch. Enter Correct Name")
                ElseIf sear_type = 3 Then
                    MsgBox("This Customer is not Present or not belongs to this branch. Enter Correct Card No")
                ElseIf sear_type = 4 Then
                    MsgBox("This Customer is not Present or not belongs to this branch. Enter Correct Pan No")
                ElseIf sear_type = 5 Then
                    MsgBox("This Customer is not Present or not belongs to this branch. Enter Correct Phone No")
                ElseIf sear_type = 6 Then
                    MsgBox("This Customer is not Present or not belongs to this branch. Enter Correct ID No")
                End If

                Me.txt_search.Text = ""
                Me.txt_search.Focus()
            End If

            'If dt.Rows.Count > 0 Then
            '    Me.grp2.Height = 130
            '    Me.grp2.Visible = True
            '    Me.dgsearchResult.Height = 120
            '    dtsub = dt
            'Else
            '    MsgBox("This Customer is not Present")
            'End If


        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Public Function set_ip() As String
        Dim ip As String
        Dim ipEntry As Net.IPHostEntry = Dns.GetHostEntry(My.Computer.Name.ToString())
        Dim IpAddr As Net.IPAddress() = ipEntry.AddressList
        Dim client_ip As String = ""
        Dim i As Integer
        For i = 0 To IpAddr.Length - 1
            client_ip = IpAddr(i).ToString()
        Next i
        ip = client_ip
        If ip.StartsWith("10.") Then
            If ip.StartsWith("10.0") Then
                webser_ip = webser_ip
            Else
                webser_ip = webser_ip
            End If
        Else
            If ip = "10.0.0.31" Or ip = "10.0.9.201" Then
                webser_ip = webser_ip
            Else
                webser_ip = webser_ip
            End If
        End If
        Return webser_ip
    End Function

    Private Sub SearchCustomer_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        Try
            RemoveTempImageFiles()
        Catch ex As Exception

        End Try
    End Sub


#Region "Remove Temporary File"


    Public Sub RemoveTempFiles()
        Try
            Dim Dir As New DirectoryInfo(Path.GetTempPath)
            If Dir.Exists() Then
                Dim FileList() As FileInfo
                FileList = Dir.GetFiles("*.tmp")
                Dim FileCount As Integer
                For FileCount = 0 To FileList.GetUpperBound(0)
                    Try
                        FileList(FileCount).Delete()
                    Catch ex As Exception

                    End Try
                Next
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "Remove Temporary Image Files"


    Public Sub RemoveTempImageFiles()
        Try
            Dim Dir As New DirectoryInfo(Path.GetTempPath)
            If Dir.Exists() Then
                Dim FileList() As FileInfo
                FileList = Dir.GetFiles("*.tif")
                Dim FileCount As Integer
                For FileCount = 0 To FileList.GetUpperBound(0)
                    Try
                        FileList(FileCount).Delete()
                    Catch ex As Exception

                    End Try
                Next
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region

    Private Sub SearchCustomer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Me.Cursor = Cursors.WaitCursor
            'Me.Br_firm1.url = "app.manappuram.net"
            'Me.Br_firm1.setAll()
            'WHELPER.setData("brid", Me.Br_firm1.branch_id)
            'WHELPER.setData("frmid", Me.Br_firm1.firm_id)
            'WHELPER.setData("brnm", Me.Br_firm1.branch_name)
            'WHELPER.setData("userid", Me.Br_firm1.user_id)
            'BranchID = WHELPER.getData("brid")
            'FirmID = WHELPER.getData("frmid")
            'BranchName = WHELPER.getData("brnm")
            'UserId = WHELPER.getData("userid")
            BtnDwnCnsnt.Visible = False
            set_ip()
            FirmID = Firm()
            BranchID = Branch()
            UserId = User_id()
            BranchName = Branch_Name() 'Me.Br_firm1.branch_name
            'FirmID = Me.Br_firm1.firm_id
            'BranchID = Me.Br_firm1.branch_id
            'UserId = Me.Br_firm1.user_id
            ' BtnDwnCnsnt.Visible = False
            Me.Main_tab.TabPages(0).Enabled = True
            Me.Main_tab.TabPages(1).Enabled = True
            Me.Main_tab.TabPages(4).Visible = False
            Me.grp2.Visible = False
            Me.grp3.Visible = False

            Me.grp2.Height = 0
            Me.grp3.Height = 0

            flag = 0
            Dim sql As String

            ' sql = obj.language_id()
            'Me.DateTimePicker3.Text = Date.Now.Date()
            fillitems()

            loadfill()
            If (cmb_lang.SelectedValue = 1 Or 2 Or 3) Then
                BtnDwnCnsnt.Visible = True
            End If

        Catch ex As Exception ''Added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Sub fillitems()
        Try
            'DateTimePicker2.Text = "01/01/2015"
            'DateTimePicker1.Text = "01/01/2040"

            Dim obj As New customerService.customer(set_ip)
            Dim ds As New DataSet
            Dim sql As String

            '   sql = obj.language_id()
            ds = obj.Combofill(BranchID) 'PopulateData() 
            Dim ds1 As String
            ds1 = obj.branchState(CInt(BranchID))
            Dim ds2 As String
            ds2 = obj.branchDistrict(CInt(BranchID))
            '.Select("CompanyName Like 'A%'")
            'Dim Iddt As DataTable
            Dim dv As DataView = New DataView(Addhead(ds.Tables("identity"), ds.Tables("identity").Columns(1).ColumnName, ds.Tables("identity").Columns(0).ColumnName))
            dv.RowFilter = "IDENTITY_ID IN (0,16)"
            'Iddt = Addhead(ds.Tables("identity"), ds.Tables("identity").Columns(1).ColumnName, ds.Tables("identity").Columns(0).ColumnName)
            Me.cmbId.DataSource = dv
            'Me.cmbId.DataSource = Addhead(ds.Tables("identity"), ds.Tables("identity").Columns(1).ColumnName, ds.Tables("identity").Columns(0).ColumnName) 'Added for req 4829
            Me.cmbId.ValueMember = "IDENTITY_ID"
            Me.cmbId.DisplayMember = "IDENTITY_NAME"
            'Me.cmbId.SelectedValue = 16



            'btnkycc.Visible = True
            'txtACustName.ReadOnly = True
            ''rb_MaleAdd.Enabled = True

            ''rb_FemaleAdd.Enabled = True
            'txtAFatHus.ReadOnly = True
            'txtHouse.ReadOnly = True
            'txtALocation.ReadOnly = True
            'cmbCountry.Enabled = False
            'cmbState.Enabled = False
            'cmbDistrict.Enabled = False

            ''cmbDistrict.Enabled = False
            'Me.DateTimePicker3.Enabled = False
            'Me.txtEmail.ReadOnly = False

            'txtPincode.ReadOnly = True
            'txtAIdno.Focus()






        Catch ex As Exception

        End Try

    End Sub
    Private Sub dgsearchResult_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgsearchResult.CellContentClick
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.grp3.Visible = True
            Me.grp3.Height = 600
            Dim tem_dt As New DataTable
            Dim str As String
            str = Me.dgsearchResult.CurrentRow.Cells(0).Value.ToString
            Dim obj As New customerService.customer(set_ip)
            dtsub = obj.DisplayCustomer(str, BranchID).Tables(0)
            dtmain = dtsub.Clone
            Dim dv As New DataView(dtsub)
            dv.RowFilter = "CUST_ID='" & str & " '"
            Dim dvr As DataRowView
            dvr = dv.Item(0)
            addProofID = dvr.Item("address_proof").ToString
            custBranchID = dvr.Item("branch_id").ToString 'Added by george
            'c.cust_id,c.cust_name,c.fat_hus,c.house_name,c.locality,p.post_office,p.pin_code,c.phone1,c.phone2,d.district_name,s.state_name,cd.country_name 
            Dim dgdtl() As String = {"CUST_ID", "cust_name", "fat_hus", "house_name", "LOCALITY", "post_office", "pin_code", "phone1", "shareflag"}
            For i As Int16 = 0 To 8
                Dim dc As New DataColumn
                dc.ColumnName = dgdtl(i)
                dc.DefaultValue = dvr.Item(i)
                tem_dt.Columns.Add(dc)
            Next
            Dim cp As String = System.IO.Path.GetTempPath()

            Dim sql As String = "select pledge_photo from customer_photo where cust_id='" & str & "'"
            Dim dt As New Data.DataTable
            Dim ws As New customerService.customer(set_ip)
            dt = ws.QueryResult(sql).Tables(0)
            If dt.Rows.Count > 0 Then
                If Not IsDBNull(dt.Rows(0)(0)) Then
                    Dim fnm As String
                    fnm = cp + "show.jpg"
                    Dim fp As New System.IO.FileInfo(fnm)
                    If fp.Exists() Then
                        fp.Delete()
                    End If
                    Dim fs As New System.IO.FileStream(fnm, System.IO.FileMode.Create)
                    Dim bl() As Byte
                    bl = CType(dt.Rows(0)(0), Byte())
                    fs.Write(bl, 0, bl.Length)
                    fs.Close()
                    fs = Nothing
                    Dim file_name As String = fnm.Substring(fnm.IndexOf("show"))
                    Me.picSearchdtl.ImageLocation = cp + file_name
                    Me.picSearchdtl.SizeMode = PictureBoxSizeMode.StretchImage

                Else
                    Me.picSearchdtl.Image = Nothing
                End If
            Else
                Me.picSearchdtl.Image = Nothing
            End If
            'Added by george to indicate customer active status
            If dvr.Item("isactive").ToString = "1" Then
                lblActive.BackColor = Drawing.Color.Green
                lblActive.Text = "Active"
            Else
                lblActive.BackColor = Drawing.Color.Red
                lblActive.Text = "In Active"
            End If

            tem_dt.Rows.Add()
            cust_result = tem_dt
            UpdateBinding()
            photoStatus = 0
            'Me.Main_tab.TabPages(2).Enabled = True
            LoadKYCPhoto(str) 'added by george for kyc requirement
            txtaadhaarnum.Text = ws.ChkExistEKYC(str).ToString()
            dt = ws.ExtCust_KycVerifyDetails(str, "CUSTOMERID")
            If dt.Rows.Count > 0 Then
                If Not IsDBNull(dt.Rows(0)(0)) Then
                    lblcustomerVeri.Text = "THIS CUSTOMER EKYC IS VERIFIED . EKYC DETAILS  -  " + "EKYC ID : " + dt.Rows(0)(0).ToString() + vbCrLf + "  VERIFIED DATE : " + dt.Rows(0)(1).ToString()
                    lblcustomerVeri.ForeColor = Color.Green
                    btnKycMatch.Enabled = False
                    btn_mapKyc.Enabled = False

                Else
                    lblcustomerVeri.Text = "THIS CUSTOMER EKYC NOT VERIFIED "
                    lblcustomerVeri.ForeColor = Color.Red
                    'btnKycMatch.Enabled = True
                    btn_mapKyc.Enabled = True

                End If
            Else
                lblcustomerVeri.Text = "THIS CUSTOMER EKYC NOT VERIFIED "
                lblcustomerVeri.ForeColor = Color.Red
                'btnKycMatch.Enabled = True
                btn_mapKyc.Enabled = True


            End If
            Me.Main_tab.SelectedTab = Me.tb_search
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.grp2.Visible = False
        Me.grp3.Visible = False
        Me.grp2.Height = 0
        Me.grp3.Height = 0
    End Sub
    Sub clear()
        Me.txtCustomerid.Text = ""
        Me.txtcustname.Text = ""
        Me.txtfathus.Text = ""
        Me.txthousename.Text = ""
        Me.txtLocation.Text = ""
        'Me.txtState.Text = ""
        ' Me.txtDistrict.Text = ""
        Me.txtPost.Text = ""
        Me.txtPin.Text = ""
        '  Me.txtcountry.Text = ""
        '  Me.txtphonecode.Text = ""
        Me.txtphoneno.Text = ""
        '  Me.txtIdNo.Text = ""
    End Sub
    Protected Sub UpdateBinding()
        Try
            'Me.txtDocid.DataBindings.Clear()
            Me.txtCustomerid.DataBindings.Clear()
            Me.txtcustname.DataBindings.Clear()
            Me.txtfathus.DataBindings.Clear()
            Me.txthousename.DataBindings.Clear()
            Me.txtLocation.DataBindings.Clear()
            ' Me.txtState.DataBindings.Clear()
            ' Me.txtDistrict.DataBindings.Clear()
            Me.txtPost.DataBindings.Clear()
            Me.txtPin.DataBindings.Clear()
            ' Me.txtcountry.DataBindings.Clear()
            ' Me.txtphonecode.DataBindings.Clear()
            Me.txtphoneno.DataBindings.Clear()
            '  Me.txtIdNo.DataBindings.Clear()
            If Not cust_result Is Nothing Then
                '{"CUST_ID", "cust_name", "fat_hus", "house_name", "LOCALITY", "post_office", "pin_code", "phone1", "district_name", "state", "country"}
                Me.txtCustomerid.DataBindings.Add("Text", cust_result, "CUST_ID")
                Me.txtcustname.DataBindings.Add("Text", cust_result, "cust_name")
                Me.txtfathus.DataBindings.Add("Text", cust_result, "fat_hus")
                Me.txthousename.DataBindings.Add("Text", cust_result, "house_name")
                Me.txtLocation.DataBindings.Add("Text", cust_result, "LOCALITY")
                '  Me.txtState.DataBindings.Add("Text", cust_result, "state_name")
                ' Me.txtDistrict.DataBindings.Add("Text", cust_result, "district_name")
                Me.txtPost.DataBindings.Add("Text", cust_result, "post_office")
                Me.txtPin.DataBindings.Add("Text", cust_result, "pin_code")
                '  Me.txtcountry.DataBindings.Add("Text", cust_result, "country_name")
                Me.txtphoneno.DataBindings.Add("Text", cust_result, "phone1")
                '  Me.txtIdNo.DataBindings.Add("Text", cust_result, "id_number")                
            End If
        Catch ex As Exception 'added for req 4829
            Throw New ApplicationException("UpdateBinding:- " & ex.Message)
        End Try
    End Sub

#End Region
#Region "tab addphoto details"
    Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click
        Try
            wchelper.Container = Me.picAddPhoto
            wchelper.Container.SizeMode = PictureBoxSizeMode.StretchImage
            wchelper.OpenConnection()
            wchelper.Load()
        Catch ex As Exception
            If ex.Message = "Object reference not set to an instance of an object." Then
                MsgBox("Check The web cam")
            End If
        End Try
    End Sub

    Private Sub btnStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStop.Click
        wchelper.Dispose()
    End Sub

    Private Sub Cmd_exit1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cmd_exit1.Click
        Try
            Me.Cursor = Cursors.WaitCursor

            If photoStatus = 1 Then
                wchelper.SaveImage()
                'cust = wchelper.get_data()
                If cust Is Nothing Then
                    Me.picCustPhoto.Image = Nothing
                Else
                    Me.picCustPhoto.Image = Me.picAddPhoto.Image
                End If
                Me.Main_tab.SelectedTab = Me.tb_addcustomer
            Else
                Dim message As String
                Try
                    wchelper.SaveImage()
                    'cust = wchelper.get_data()

                    ''''''''''''''''''''''''''''''''
                    Try
                        Dim ms As MemoryStream = New MemoryStream(cust)
                        Dim NewBitmap As New System.Drawing.Bitmap(New Bitmap(ms))
                        ms.Close()

                        Dim sql As String = "select sysdate from dual"
                        Dim ws As New customerService.customer(set_ip)
                        Dim dt As New Data.DataTable
                        dt = ws.QueryResult(sql).Tables(0)
                        Dim saveval As String
                        NewBitmap = Overlay.ResizeImage(NewBitmap, 320, 240)
                        saveval = "Modified By : " & UserId.ToString() & " on : " & dt.Rows(0)(0).ToString() & " at : " & BranchName.ToString()
                        NewBitmap = Overlay.VBOverlay(NewBitmap, saveval, New Font("Arial", 4, FontStyle.Regular), Color.Black, True, False, ContentAlignment.BottomLeft, 0.2!)

                        Dim file_size As Long
                        Dim desired_size As Long = 10000
                        For compression_level As Integer = 100 To 10 Step -1
                            Using memory_stream As MemoryStream = Overlay.SaveJPGWithCompressionSetting(NewBitmap, compression_level)
                                file_size = memory_stream.Length
                                If file_size <= desired_size Then
                                    cust = memory_stream.ToArray()
                                    Exit For
                                End If
                            End Using
                        Next compression_level

                        'Me.picSearchdtl.Image = NewBitmap
                        'cust = ms1.GetBuffer()
                        '''''''''''''''''''''''''''''''''''''
                    Catch ex As Exception
                        MsgBox(ex.Message.ToString())
                    End Try

                    'If cust Is Nothing Then
                    '    Me.picSearchdtl.Image = Nothing
                    'Else

                    '    Dim ws As New customerService.customer(set_ip)
                    '    'message = ws.AddCustomerPhoto(Me.txtCustomerid.Text, cust, 1)
                    '    'Modified by Tijo For Customer name and other related approval change
                    '    message = ws.AddCustomerPhotoForApproval(Me.txtCustomerid.Text, cust, BranchID, UserId, )
                    '    Me.picSearchdtl.Image = Me.picAddPhoto.Image
                    '    Me.picSearchdtl.SizeMode = PictureBoxSizeMode.StretchImage
                    '    cust = Nothing
                    'End If
                    Me.Main_tab.SelectedTab = Me.tb_search
                    Me.picAddPhoto.Image = Nothing
                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try
            End If
            Me.picAddPhoto.Image = Nothing
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub


#End Region
#Region "tab addkyc details"
    Private Sub btnScan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScan.Click
        Try
            btnUpload.Enabled = False
            'If rdoRegularkyc.Checked AndAlso (cmbId.SelectedValue = 14 OrElse identityID = 14) Then
            '    If cmbAddressProf.SelectedValue = 0 Then
            '        MessageBox.Show("Address proof is mandatory for regular KYC if the customer having only PAN as id proof", "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '        Exit Sub
            '    End If
            'End If
            Dim isAddScan As Boolean = False
            Dim addressFile As String = String.Empty
            Dim tmpPath As String = String.Empty
            'If Not chkIdentity.Checked AndAlso Not chkAddress.Checked Then
            '    MessageBox.Show("Please select ID/Address proof to scan", "Customer", MessageBoxButtons.OK, MessageBoxIcon.Information)
            '    Exit Sub
            'End If

            'If chkAddress.Checked AndAlso Not chkSameAsId.Checked Then
            '    If cmbAddressProf.SelectedValue = 0 Then
            '        MessageBox.Show("Address proof is mandatory for regular KYC, Please select the type of address proof", "Customer", MessageBoxButtons.OK, MessageBoxIcon.Information)
            '        Exit Sub
            '    Else
            '        isAddScan = True
            '    End If
            'End If
            'isappend = False
            'If File.Exists(kycDBFile) Then
            '    If MessageBox.Show("This Customer already have KYC document." & vbCrLf & "Click 'YES' to append with existing document" & vbCrLf & "Clcik 'NO' to proceed with fresh sacnning", "Customer", MessageBoxButtons.YesNo, MessageBoxIcon.Information) = DialogResult.Yes Then
            '        btnAppend_Click(sender, e)
            '        Exit Sub
            '    End If
            'End If

            btnScan.Enabled = False
            If kycStatus <> 1 Then
                btnUpload.Enabled = False
            End If
            btnLoadImage.Enabled = False
            btnAppend.Enabled = False
            Me.Cursor = Cursors.WaitCursor

            kycDocFile = ""
            If isAddScan = True Then
                MessageBox.Show("First scan id proof then scan address proof.", "Customer", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

SCAN_ADDRESS:

            If kycDocFile <> "" Then
                MessageBox.Show("Now scan the address proof.", "Customer", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            If Not ScanImage() Then
                MessageBox.Show("Scanning Aborted", "Customer", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            If isAddScan = True Then
                tmpPath = Path.GetTempFileName & "AS.tif"
                File.Copy(ScanX1.ScannedImagePath, tmpPath)
                kycDocFile = ScanX1.ScannedImagePath
                isAddScan = False
                GoTo SCAN_ADDRESS
            End If

            If tmpPath = "" Then
                kycDocFile = ScanX1.ScannedImagePath
            Else
                ScanX1.AppendFileNew(tmpPath, ScanX1.ScannedImagePath)
                kycDocFile = tmpPath
                ScanX1.ImageFileLoad(kycDocFile)
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
            btnScan.Enabled = True
            btnUpload.Enabled = True
            btnLoadImage.Enabled = True
            btnAppend.Enabled = True
        End Try
    End Sub

    Private Function ScanImage() As Boolean
        Dim err As ActiveScanner.ScanX.ErrorCode
        ScanX1.SetAnnotation("Scanned by " & UserId & " from " & BranchName & " on " & Now.ToString("dd-MMM-yyyy hh:mm tt"), 0, 0, "", 8, False, 0, 0, 0)
        ScanX1.SetFileName("kycdocument")
        If cmbScanMode.Text = "Block & White" Then
            ScanX1.ColorMode = ActiveScanner.ScanX.ColorModeType.BLACK_WHITE
            err = ScanX1.ScanDocument(ActiveScanner.ScanX.ColorModeType.BLACK_WHITE, 0, 100, 0, 0, 0, 0)
        Else
            ScanX1.ColorMode = ActiveScanner.ScanX.ColorModeType.GRAY_HIGH_COMP_LOW_QUALITY
            err = ScanX1.ScanDocument(ActiveScanner.ScanX.ColorModeType.GRAY_HIGH_COMP_LOW_QUALITY, 0, 100, 0, 0, 0, 0)
        End If

        If err = ActiveScanner.ScanX.ErrorCode.SUCCESS Then
            Return True
        Else
            Return False
        End If

    End Function


    Private Sub btSource_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btSource.Click
        ScanX1.SelectSource()
    End Sub

    Private Sub btnUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        If kycStatus = 1 Then
            'wchelper.SaveImage()
            'photo = wchelper.get_data()
            kycPhoto = ConvertImageFiletoBytes(kycDocFile)
            btngetkyc.Visible = True
            btngetkyc.Enabled = True
            Me.Main_tab.SelectedTab = Me.tb_addcustomer
        Else
            Dim message As String
            Try
                If String.IsNullOrEmpty(kycDocFile) OrElse Not File.Exists(kycDocFile) Then
                    MessageBox.Show("Please scan the document before upload", "Customer", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If

                Me.Cursor = Cursors.WaitCursor
                kycPhoto = ConvertImageFiletoBytes(kycDocFile)
                btngetkyc.Visible = True
                If kycPhoto Is Nothing Then
                    'Me.picSearchdtl.Image = Nothing
                    'btngetkyc.Visible = False
                Else
                    Dim ws As New customerService.customer(set_ip)
                    kycPhoto = ConvertImageFiletoBytes(kycDocFile)
                    If Not IsNothing(kycPhoto) Then
                        Dim retval = ws.custinsrt(txtCustomerid.Text, kycPhoto)
                        MsgBox(retval, MsgBoxStyle.OkOnly, "EKYC Customer")
                        kycPhoto = Nothing
                    End If
                End If
                ScanX1.ResetImg()
                'Me.picAddKyc.Image = Nothing
                Me.Main_tab.SelectedTab = Me.tb_search
            Catch ex As Exception 'added for req 4829
                MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
                'Finally
                '    Me.Cursor = Cursors.Default
                '    kycDBFile = kycDocFile
                '    kycDocFile = ""
                '    kycPhoto = Nothing
            End Try
            Me.Cursor = Cursors.Default
        End If
    End Sub
#End Region
#Region "tab neftdetails"

    Private Sub cmbNeftState_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbNeftState.SelectedIndexChanged
        Try
            Me.Cursor = Cursors.WaitCursor
            Dim ws As New customerService.customer(set_ip)
            If Me.cmbNeftState.Focused = True Then
                stid = Me.cmbNeftState.SelectedValue
                Dim dt As New DataTable
                dt = ws.brdistrict(stid, BranchID).Tables(0)
                If dt.Rows.Count > 0 Then
                    Me.cmbNeftDistrict.DataSource = dt
                    Me.cmbNeftDistrict.DisplayMember = dt.Columns(0).ColumnName
                    Me.cmbNeftDistrict.ValueMember = dt.Columns(1).ColumnName
                End If
                distid = Me.cmbNeftDistrict.SelectedValue
                dt = ws.bank(CInt(distid)).Tables(0)
                If dt.Rows.Count > 0 Then
                    Me.cmbBank.DataSource = dt
                    Me.cmbBank.DisplayMember = dt.Columns(1).ColumnName
                    Me.cmbBank.ValueMember = dt.Columns(0).ColumnName
                End If
                Me.txtIFSCCode.Text = Me.cmbBank.SelectedValue.ToString
            End If
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub cmbNeftDistrict_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbNeftDistrict.SelectedIndexChanged
        Try
            Me.Cursor = Cursors.WaitCursor
            If Me.cmbNeftDistrict.Focused = True Then
                Dim ws As New customerService.customer(set_ip)
                distid = Me.cmbNeftDistrict.SelectedValue
                Dim dt As New DataTable
                dt = ws.bank(CInt(distid)).Tables(0)
                If dt.Rows.Count > 0 Then
                    Me.cmbBank.DataSource = dt
                    Me.cmbBank.DisplayMember = dt.Columns(1).ColumnName
                    Me.cmbBank.ValueMember = dt.Columns(0).ColumnName
                End If
            End If
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub cmbBank_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbBank.SelectedIndexChanged
        Me.txtIFSCCode.Text = Me.cmbBank.SelectedValue.ToString
    End Sub

    Private Sub btnNStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNStart.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            'Commented by george for neft scan
            'wchelper.Container = Me.picNeftdtl
            'wchelper.OpenConnection()
            'wchelper.Load()

            If Not ScanNeftImage() Then
                MessageBox.Show("Scanning Aborted", "Customer", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub


    Private Sub btnNExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNExit.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            If neft_status = 0 Then
                Me.Main_tab.SelectedTab = Me.tb_search
            Else
                Me.Main_tab.SelectedTab = Me.tb_addcustomer
            End If
            neftScanCtl.ResetImg()
            neftScanCtl.ScannedImagePath = ""
            neft_status = 0
            'wchelper.SaveImage()
            'neftPhoto = wchelper.get_data()
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub fillneftdtl()
        MsgBox("All the fields are mandatory, kindly update carefully", vbInformation + vbOKOnly)
        Try
            'If neft_status = 1 Then
            Dim ws As New customerService.customer(set_ip)
            Dim dt As New DataTable
            dt = ws.brstate(CInt(BranchID)).Tables(0)
            Me.cmbNeftState.DataSource = dt
            Me.cmbNeftState.DisplayMember = dt.Columns(0).ColumnName
            Me.cmbNeftState.ValueMember = dt.Columns(1).ColumnName

            stid = CInt(ws.branchState(CInt(BranchID)))
            Me.cmbNeftState.SelectedValue = stid
            dt = ws.brdistrict(stid, CInt(BranchID)).Tables(0)
            If dt.Rows.Count > 0 Then
                Me.cmbNeftDistrict.DataSource = dt
                Me.cmbNeftDistrict.DisplayMember = dt.Columns(0).ColumnName
                Me.cmbNeftDistrict.ValueMember = dt.Columns(1).ColumnName
            End If
            distid = CInt(ws.branchDistrict(CInt(BranchID)))
            Me.cmbNeftDistrict.SelectedValue = distid
            Dim dsDist As New DataSet
            dsDist = ws.bank(CInt(distid))
            If dsDist.Tables.Count > 0 AndAlso dsDist.Tables(0).Rows.Count > 0 Then
                Me.cmbBank.DataSource = dsDist.Tables(0)
                Me.cmbBank.DisplayMember = dsDist.Tables(0).Columns(1).ColumnName
                Me.cmbBank.ValueMember = dsDist.Tables(0).Columns(0).ColumnName
                Me.txtIFSCCode.Text = CStr(dsDist.Tables(0).Rows(0)(0))
            End If
            neftScanCtl.ScannedImagePath = ""
            dt = ws.fillcurrentaccount().Tables(0)
            If dt.Rows.Count > 0 Then
                Me.cmbAccount.DataSource = dt
                Me.cmbAccount.DisplayMember = dt.Columns(1).ColumnName
                Me.cmbAccount.ValueMember = dt.Columns(0).ColumnName
            End If
            Me.btnNExit.Enabled = True
            ' Me.btnNStart.Enabled = True
            ' Me.btnNeftSource.Enabled = True
            'dt = ws.bank(CInt(distid)).Tables(0)
            GetNeftDetails()

            '  End If
        Catch ex As Exception 'added for req 4829
            Throw New ApplicationException("fillneftdtl:- " & ex.Message)
        End Try
    End Sub

    Private Sub GetNeftDetails()
        Dim dsOut As New DataSet
        grdneftDetails.Rows.Clear()
        Dim ws As New customerService.customer(set_ip)
        dsOut = ws.QueryResult("select t.ifsc_code,(select s.bankname from neft_bank_mst s where s.ifsc_code=t.ifsc_code) beneficiary_bank, t.beneficiary_branch,t.beneficiary_account,c.account_name,t.bank_id,t.cust_id ,t.serialno,decode(t.verify_status,'T','Approved','F','Pending for Approval') status from neft_customer t,neft_current_account c where t.acc_type=c.acc_type and cust_id='" & Me.txtCustomerid.Text & "'")
        If dsOut.Tables.Count > 0 AndAlso dsOut.Tables(0).Rows.Count > 0 Then
            For i As Integer = 0 To dsOut.Tables(0).Rows.Count - 1
                grdneftDetails.Rows.Add(New Object() {dsOut.Tables(0).Rows(0)("beneficiary_bank").ToString, dsOut.Tables(0).Rows(0)("ifsc_code").ToString, dsOut.Tables(0).Rows(0)("beneficiary_account").ToString, dsOut.Tables(0).Rows(0)("account_name").ToString, dsOut.Tables(0).Rows(0)("status").ToString, dsOut.Tables(0).Rows(0)("serialno").ToString})
                txtIFSCCode.Text = dsOut.Tables(0).Rows(0)("ifsc_code").ToString
                IFSC_Fill()
            Next
        Else
            grdneftDetails.Rows.Clear()
        End If
    End Sub

    Private Sub btnNConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNConfirm.Click
        Try
            Me.Cursor = Cursors.WaitCursor

            If Me.txtPBCustnm.Text.Trim = "" Then
                MsgBox("Please enter customer name as per bank pass book")
                Me.txtPBCustnm.Focus()
                Exit Sub
            End If

            If Me.txtAccountNo.Text.Trim = "" Then
                MsgBox("Please enter bank account number")
                Exit Sub
            End If

            If txtCfmAccountNo.Text.Trim = "" Then
                MsgBox("Please enter confirm bank account number")
                txtCfmAccountNo.Focus()
                Exit Sub
            End If

            If txtAccountNo.Text.Trim <> txtCfmAccountNo.Text.Trim Then
                MsgBox("Confirm Bank account number do not match Bank Account Number")
                txtCfmAccountNo.Clear()
                txtCfmAccountNo.Focus()
                Exit Sub
            End If

            Dim ws As New customerService.customer(set_ip)
            If neft_status = 0 Then
                ' rec_acc_typ*brid*custid*ifsc*accno*custname*fmid**branch_name
                Dim confdata As String
                dt = ws.QueryResult("select branch_name from branch_master where branch_id =" & BranchID & " ")
                BranchName = dt.Tables(0).Rows(0)(0)
                ' wchelper.SaveImage() 'commented by george for neft scan
                neftPhoto = ConvertImageFiletoBytes(neftScanCtl.ScannedImagePath) 'wchelper.get_data() 'Added by george for neft scan
                If neftPhoto Is Nothing Then
                    MsgBox("Scan the Bank statement")
                Else
                    Dim dt As DataTable = ws.GetCustomerBankDetails(txtIFSCCode.Text).Tables(0)
                    If dt.Rows.Count > 0 Then
                        confdata = Me.cmbAccount.SelectedValue & "*" & CInt(BranchID) & "*" & Me.txtNeftCustid.Text & "*" & Me.txtIFSCCode.Text & "*" & Me.txtAccountNo.Text & "*" & Me.txtNeftCustnm.Text & "*" & 1 & "**" & BranchName & "*" & Me.txtPBCustnm.Text
                        Dim obj As String = ws.neft_add(confdata, neftPhoto)
                        neftScanCtl.ResetImg()
                        neftScanCtl.ScannedImagePath = ""
                        MsgBox("Updated")
                        'commented by george for neft scan
                        'Me.btnNStart.Enabled = True
                        'Me.btnNeftSource.Enabled = True
                        'Me.btnNExit.Enabled = True
                        GetNeftDetails()
                        txtPBCustnm.Text = ""
                        txtAccountNo.Text = ""
                        txtCfmAccountNo.Text = ""
                        neftScanCtl.ScannedImagePath = ""
                    Else
                        MsgBox("This IFSC Code Not In Our Database,Please contact Marketing Team", vbCritical + vbOKOnly)
                        Exit Sub
                    End If
                End If
            Else
                ' rec_acc_typ*brid*ifsc*accno*fmid*branch_name
                neftdetails = Me.cmbAccount.SelectedValue & "*" & CInt(BranchID) & "*" & Me.txtIFSCCode.Text & "*" & Me.txtAccountNo.Text & "*" & 1 & "*" & BranchName
                neftPhoto = ConvertImageFiletoBytes(neftScanCtl.ScannedImagePath)  'Added by george for neft scan
                Me.Main_tab.SelectedTab = Me.tb_addcustomer
            End If
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub txtAccountNo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAccountNo.LostFocus
        Try
            Me.Cursor = Cursors.WaitCursor
            Dim ss As New DataTable
            Dim ws As New customerService.customer(set_ip)
            ss = ws.neft_sbacc(Me.txtAccountNo.Text, Me.txtNeftCustid.Text).Tables(0)
            If ss.Rows.Count > 1 Then
                MsgBox("This Account has Already added")
            Else

            End If
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
#End Region
#Region "tab customer modify details"
    Private Sub fill_modi_controls(ByVal ods As DataSet)
        Try
            Me.cmbMcuststat.DataSource = Addhead(ods.Tables("customer_type"), ods.Tables("customer_type").Columns(1).ColumnName, ods.Tables("customer_type").Columns(0).ColumnName)
            Me.cmbMcuststat.ValueMember = ods.Tables("customer_type").Columns(0).ColumnName
            Me.cmbMcuststat.DisplayMember = ods.Tables("customer_type").Columns(1).ColumnName

            If ods.Tables("customer_dt2").Rows(0)("occupation_id").ToString <> "28" Then
                For Each dr As DataRow In ods.Tables("occupation_master").Rows
                    If dr(0) = 28 Then
                        ods.Tables("occupation_master").Rows.Remove(dr)
                        Exit For
                    End If
                Next
            End If
            If Not ods.Tables("customer_dt2").Rows(0)("gender") Is DBNull.Value Then
                If ods.Tables("customer_dt2").Rows(0)("gender").ToString = "0" Then
                    rb_FemaleMod.Checked = True
                ElseIf ods.Tables("customer_dt2").Rows(0)("gender").ToString = "1" Then
                    rb_MaleMod.Checked = True
                Else
                    rb_MaleMod.Checked = False
                    rb_FemaleMod.Checked = False
                End If
            Else
                rb_MaleMod.Checked = False
                rb_FemaleMod.Checked = False
            End If
            Me.cmbMoccu.DataSource = Addhead(ArrangeData(ods.Tables("occupation_master")), ods.Tables("occupation_master").Columns(1).ColumnName, ods.Tables("occupation_master").Columns(0).ColumnName)
            Me.cmbMoccu.ValueMember = ods.Tables("occupation_master").Columns(0).ColumnName
            Me.cmbMoccu.DisplayMember = ods.Tables("occupation_master").Columns(1).ColumnName

            Me.cmbMid.DataSource = Addhead(ods.Tables("identity"), ods.Tables("identity").Columns(1).ColumnName, ods.Tables("identity").Columns(0).ColumnName)
            Me.cmbMid.ValueMember = ods.Tables("identity").Columns(0).ColumnName
            Me.cmbMid.DisplayMember = ods.Tables("identity").Columns(1).ColumnName

            Me.cmbMcountry.DataSource = Addhead(ods.Tables("country_dtl"), ods.Tables("country_dtl").Columns(1).ColumnName, ods.Tables("country_dtl").Columns(0).ColumnName)
            Me.cmbMcountry.ValueMember = ods.Tables("country_dtl").Columns(0).ColumnName
            Me.cmbMcountry.DisplayMember = ods.Tables("country_dtl").Columns(1).ColumnName

            Me.cmbMstate.DataSource = Addhead(ods.Tables("state_master"), ods.Tables("state_master").Columns(1).ColumnName, ods.Tables("state_master").Columns(0).ColumnName)
            Me.cmbMstate.ValueMember = ods.Tables("state_master").Columns(0).ColumnName
            Me.cmbMstate.DisplayMember = ods.Tables("state_master").Columns(1).ColumnName

            'Me.cmbMDistrict.DataSource = ods.Tables("district_master")
            'Me.cmbMDistrict.ValueMember = ods.Tables("district_master").Columns(0).ColumnName
            'Me.cmbMDistrict.DisplayMember = ods.Tables("district_master").Columns(1).ColumnName

            'Me.cmbMPost.DataSource = ods.Tables("post_master")
            'Me.cmbMPost.ValueMember = ods.Tables("post_master").Columns(0).ColumnName
            'Me.cmbMPost.DisplayMember = ods.Tables("post_master").Columns(1).ColumnName

            Dim ods1 As String
            Dim ws As New customerService.customer(set_ip)
            ods1 = ws.branchState(BranchID)

            Dim ods2 As String
            ods2 = ws.branchDistrict(BranchID)
            Me.cmbMcountry.SelectedIndex = Me.cmbMcountry.FindStringExact(ods.Tables("customer_dt1").Rows(0)("country").ToString)
            Me.cmbMcuststat.SelectedIndex = Me.cmbMcuststat.FindStringExact(ods.Tables("customer_dt2").Rows(0)("descr").ToString)
            Me.cmbMoccu.SelectedIndex = Me.cmbMoccu.FindStringExact(ods.Tables("customer_dt2").Rows(0)("occupation_name").ToString)

            If ods.Tables("customer_dt2").Rows(0)("occupation_name").ToString = "TRANSPORT" Then
                VehicleFinance.Mode = "Edit"
                Dim ds As New DataSet
                Dim dt As New DataTable
                Dim cID As String = ods.Tables("customer_dt1").Rows(0)("cust_id").ToString
                Dim sql As String = "select * from TBL_Vehicle_Finance where customer_id=" & cID & ""
                ds = ws.QueryResult(sql)
                If ds.Tables.Count > 0 Then
                    dt = ws.QueryResult(sql).Tables(0)
                    If dt.Rows.Count > 0 Then
                        VehicleFinance.CommVehOwnership = Convert.ToInt32(dt.Rows(0)(2))
                        VehicleFinance.CommOwnershipVehType = dt.Rows(0)(3).ToString
                        VehicleFinance.TruckReq = Convert.ToInt32(dt.Rows(0)(4))
                        VehicleFinance.ReqCommVeh = Convert.ToInt32(dt.Rows(0)(5))
                        VehicleFinance.ReqCommVehicleType = dt.Rows(0)(6).ToString
                        VehicleFinance.ReqOtherVeh = Convert.ToInt32(dt.Rows(0)(7))
                        VehicleFinance.ReqOtherVehDetail = dt.Rows(0)(8).ToString
                        Dim objVeh As New VehicleFinanceSurvey
                        objVeh.Show()
                    End If
                End If
            End If

            If Not IsNothing(cmbMoccu.SelectedValue) AndAlso cmbMoccu.SelectedValue = 28 Then '.Text = "PAWN BROCKER OR MONEY LENDERS"
                cmbMoccu.Enabled = False
            End If

            If Me.cmbMcountry.SelectedIndex = -1 AndAlso Me.cmbMcountry.Items.Count > 0 Then Me.cmbMcountry.SelectedIndex = 0
            If Me.cmbMcuststat.SelectedIndex = -1 AndAlso Me.cmbMcuststat.Items.Count > 0 Then Me.cmbMcuststat.SelectedIndex = 0
            If Me.cmbMoccu.SelectedIndex = -1 AndAlso Me.cmbMoccu.Items.Count > 0 Then Me.cmbMoccu.SelectedIndex = 0

            If ods.Tables("customer_dt1").Rows(0)("state") Is DBNull.Value Then
                Me.cmbMstate.SelectedValue = ods1
                Me.cmbMDistrict.SelectedValue = ods2
                ' Me.cmbMPost.SelectedValue = 1
            Else
                Me.cmbMstate.SelectedValue = CStr(ods.Tables("customer_dt1").Rows(0)("state"))
                '  state_list1(CStr(ods.Tables("customer_dt1").Rows(0)(10)))
                Me.cmbMDistrict.SelectedValue = CStr(ods.Tables("customer_dt1").Rows(0)("district_id"))
                '     district_list1(CStr(ods.Tables("customer_dt1").Rows(0)(8)))
                Me.cmbMPost.SelectedValue = CStr(ods.Tables("customer_dt1").Rows(0)("pincode"))
                Dim pin() As String
                If ods.Tables("post_master").Rows.Count > 0 Then
                    pin = CStr(ods.Tables("customer_dt1").Rows(0)(7)).Split(CChar("@"))
                    Me.txtMPincode.Text = pin(0)
                Else
                    Me.txtMPincode.Text = "0"
                End If
            End If
            Me.cmbMediaType.DataSource = Addhead(ods.Tables("media_type"), ods.Tables("media_type").Columns(1).ColumnName, ods.Tables("media_type").Columns(0).ColumnName)
            Me.cmbMediaType.ValueMember = ods.Tables("media_type").Columns(0).ColumnName
            Me.cmbMediaType.DisplayMember = ods.Tables("media_type").Columns(1).ColumnName

            Me.cmbMedia.DataSource = Addhead(ods.Tables("media_master"), ods.Tables("media_master").Columns(1).ColumnName, ods.Tables("media_master").Columns(0).ColumnName)
            Me.cmbMedia.ValueMember = ods.Tables("media_master").Columns(0).ColumnName
            Me.cmbMedia.DisplayMember = ods.Tables("media_master").Columns(1).ColumnName
            If ods.Tables("identity_values").Rows.Count > 0 Then
                fromMod = True
                If ods.Tables("identity_values").Columns.Contains("KYCOF") Then
                    If ods.Tables("identity_values").Rows(0)("KYCOF").ToString = "2" Then
                        rdoNonIndividualM.Checked = True
                        cmbKycNonIndividualM.Visible = True
                        pnlKycTypeM.Visible = False

                        If cmbKycNonIndividualM.Items.Count = 0 Then
                            Dim objser As New customerService.customer
                            dsnonKyc = objser.GetNonKycType()
                            If dsnonKyc.Tables.Count > 0 Then
                                cmbKycNonIndividualM.DataSource = Addhead(dsnonKyc.Tables(0), dsnonKyc.Tables(0).Columns(1).ColumnName, dsnonKyc.Tables(0).Columns(0).ColumnName)
                                cmbKycNonIndividualM.DisplayMember = "description"
                                cmbKycNonIndividualM.ValueMember = "status_id"
                            End If
                        Else
                            cmbKycNonIndividualM.SelectedIndex = 0
                        End If

                        If Not IsDBNull(ods.Tables("identity_values").Rows(0)(6)) Then
                            If ods.Tables("identity_values").Rows(0)(6) >= 100 And ods.Tables("identity_values").Rows(0)(6) < 200 Then
                                cmbKycNonIndividualM.SelectedValue = 1
                            ElseIf ods.Tables("identity_values").Rows(0)(6) >= 200 And ods.Tables("identity_values").Rows(0)(6) < 300 Then
                                cmbKycNonIndividualM.SelectedValue = 2
                            ElseIf ods.Tables("identity_values").Rows(0)(6) >= 300 And ods.Tables("identity_values").Rows(0)(6) < 400 Then
                                cmbKycNonIndividualM.SelectedValue = 3
                            ElseIf ods.Tables("identity_values").Rows(0)(6) >= 400 And ods.Tables("identity_values").Rows(0)(6) < 500 Then
                                cmbKycNonIndividualM.SelectedValue = 4
                            ElseIf ods.Tables("identity_values").Rows(0)(6) >= 550 And ods.Tables("identity_values").Rows(0)(6) < 600 Then
                                cmbKycNonIndividualM.SelectedValue = 5
                            Else
                                cmbKycNonIndividualM.SelectedValue = ""
                            End If
                        End If
                    Else
                        If (ods.Tables("identity_values").Rows(0)(6) >= 500 And ods.Tables("identity_values").Rows(0)(6) < 550) Then
                            'If ods.Tables("identity_values").Rows(0)("IDENTITY_NAME").ToString = "LOCAL ID" Then

                            rdoIndividualM.Checked = True
                            cmbKycNonIndividualM.Visible = False
                            pnlKycTypeM.Visible = True
                            'rdoIndividualM.Enabled = False
                            rdoNonIndividualM.Enabled = False
                            rdoRegularKYCM.Enabled = False
                            rdoInterimKycM.Enabled = False
                            'rdoMTKyc.Enabled = False
                            'If (ods.Tables("identity_values").Rows(0)(6) >= 500 And ods.Tables("identity_values").Rows(0)(6) < 550) Then
                            rdoMTKyc.Checked = True
                            Dim objser As New customerService.customer
                            Dim dsdet As New DataSet
                            dsdet = objser.GetNonKycDetails(16)
                            If dsdet.Tables.Count > 0 Then
                                Me.cmbMid.DataSource = Addhead(dsdet.Tables(0), dsdet.Tables(0).Columns(1).ColumnName, dsdet.Tables(0).Columns(0).ColumnName) 'Added for req 4829
                                Me.cmbMid.ValueMember = dsdet.Tables(0).Columns(0).ColumnName
                                Me.cmbMid.DisplayMember = dsdet.Tables(0).Columns(1).ColumnName
                            End If
                            'End If
                        Else
                            rdoIndividualM.Checked = True
                            cmbKycNonIndividualM.Visible = False
                            pnlKycTypeM.Visible = True
                            If Not IsDBNull(ods.Tables("identity_values").Rows(0)(6)) Then
                                'Regular kYC
                                If (ods.Tables("identity_values").Rows(0)(6) >= 1 And ods.Tables("identity_values").Rows(0)(6) <= 4) Or ods.Tables("identity_values").Rows(0)(6) = 14 Or ods.Tables("identity_values").Rows(0)(6) = 16 Then
                                    rdoRegularKYCM.Checked = True
                                Else
                                    Dim objser As New customerService.customer
                                    Dim dsdet As New DataSet
                                    dsdet = objser.GetNonKycDetails(20)

                                    If dsdet.Tables.Count > 0 Then
                                        Me.cmbMid.DataSource = Addhead(dsdet.Tables(0), dsdet.Tables(0).Columns(1).ColumnName, dsdet.Tables(0).Columns(0).ColumnName) 'Added for req 4829
                                        Me.cmbMid.ValueMember = dsdet.Tables(0).Columns(0).ColumnName
                                        Me.cmbMid.DisplayMember = dsdet.Tables(0).Columns(1).ColumnName
                                    End If
                                    rdoInterimKycM.Checked = True
                                End If
                            End If
                        End If
                    End If
                End If
                fromMod = False
                'code modified by george to restrict change  kyc
                If ods.Tables("identity_values").Rows(0)(6) Is DBNull.Value Then
                    'Me.txtMIdno.Text = ""
                    idtype = "0"
                    idno = ""
                Else
                    Dim idd As Integer = CInt(ods.Tables("identity_values").Rows(0)(6))
                    Me.cmbMid.SelectedValue = idd
                    idtype = idd.ToString
                    idno = ods.Tables("identity_values").Rows(0)("id_number").ToString
                End If

                If ods.Tables("identity_values").Columns.Contains("exservice_status") AndAlso ods.Tables("identity_values").Rows(0)("exservice_status").ToString = "0" Then
                    CheckBox2.Checked = False
                ElseIf ods.Tables("identity_values").Columns.Contains("exservice_status") AndAlso ods.Tables("identity_values").Rows(0)("exservice_status").ToString = "1" Then
                    CheckBox2.Checked = True
                End If
                If ods.Tables("identity_values").Columns.Contains("pension_order") Then
                    TextBox20.Text = ods.Tables("identity_values").Rows(0)("pension_order").ToString
                Else
                    TextBox20.Text = ""
                End If
            Else
                idtype = "0"
                idno = ""
            End If
            If ods.Tables.Contains("customer_card_dtl") AndAlso ods.Tables("customer_card_dtl").Rows.Count > 0 Then
                Me.txtMCardNo.Text = ods.Tables("customer_card_dtl").Rows(0)("card_no").ToString
            Else
                Me.txtMCardNo.Text = ""
            End If
            Me.CmbLang.DataSource = Addhead(ods.Tables("lang"), ods.Tables("lang").Columns(1).ColumnName, ods.Tables("lang").Columns(0).ColumnName)
            Me.CmbLang.DisplayMember = ods.Tables("lang").Columns(1).ColumnName
            Me.CmbLang.ValueMember = ods.Tables("lang").Columns(0).ColumnName

            Me.CmbMPep.DataSource = ods.Tables("CUST_PEP")
            Me.CmbMPep.DisplayMember = ods.Tables("CUST_PEP").Columns(1).ColumnName
            Me.CmbMPep.ValueMember = ods.Tables("CUST_PEP").Columns(0).ColumnName

            If ods.Tables("CUST_PEP1").Rows(0)(0).ToString() = "" Then
                CmbMPep.SelectedValue = -1
            Else
                CmbMPep.SelectedValue = ods.Tables("CUST_PEP1").Rows(0)(0)
            End If

            fill_modi_text_controls(ods)
        Catch ex As Exception
            Throw New ApplicationException("fill_modi_controls:- " & ex.Message)
        End Try
    End Sub
    Public Sub CheckForOnlineCustomer()
        Dim dtCustOnlineInfo As New DataTable
        Dim dsCustOnlineInfo As New DataSet
        Try
            dsCustOnlineInfo = MyWebsevice.GetOnlineCustomerDetails(txtCustomerid.Text.Trim())
            If dsCustOnlineInfo.Tables.Count > 0 Then
                dtCustOnlineInfo = dsCustOnlineInfo.Tables(0)
                If dtCustOnlineInfo.Rows(0)(0) > 0 Then
                    GroupBox1.Enabled = False
                    grpAddphoto.Enabled = True
                    grpaddkyc.Enabled = True
                    'GroupBox2.Enabled = True
                    GroupBox3.Enabled = True
                    'grppawnbrok.Enabled = True
                    'Button5.Enabled = True
                    btnNConfirm.Enabled = False
                    btnNeftSource.Enabled = False
                    btnNStart.Enabled = False
                Else
                    GroupBox1.Enabled = True
                    grpAddphoto.Enabled = True
                    grpaddkyc.Enabled = True
                    'GroupBox2.Enabled = True
                    GroupBox3.Enabled = True
                    'grppawnbrok.Enabled = True
                    'Button5.Enabled = True
                    btnNConfirm.Enabled = True
                    btnNeftSource.Enabled = True
                    btnNStart.Enabled = True
                End If
            End If
        Catch ex As Exception
            Throw New ApplicationException("fill_modi_controls:- " & ex.Message)
        End Try
    End Sub
    Function fill_modi_text_controls(ByVal ods As DataSet) As Integer
        Try
            Me.txtMCustid.DataBindings.Clear()
            Me.txtMCustnm.DataBindings.Clear()
            Me.txtMFatHus.DataBindings.Clear()
            Me.txtMHno.DataBindings.Clear()
            Me.txtMLocation.DataBindings.Clear()
            Me.txtMPincode.DataBindings.Clear()
            Me.txtMPhoneno.DataBindings.Clear()
            Me.txtMMobno.DataBindings.Clear()
            Me.IssueDt.DataBindings.Clear()
            Me.ExpiryDt.DataBindings.Clear()
            Me.txtIssuePlace.DataBindings.Clear()
            Me.txtMIdno.DataBindings.Clear()
            Me.Dobdt.DataBindings.Clear()
            Me.txtMPanNo.DataBindings.Clear()
            Me.TextBox21.DataBindings.Clear()
            Me.TextBox22.DataBindings.Clear()
            If Not ods Is Nothing Then
                Me.txtMCustid.DataBindings.Add("Text", ods.Tables("customer_dt1"), "cust_id")
                Me.txtMCustnm.DataBindings.Add("Text", ods.Tables("customer_dt1"), "cust_name")
                Me.txtMFatHus.DataBindings.Add("Text", ods.Tables("customer_dt1"), "fat_hus")
                Me.txtMHno.DataBindings.Add("Text", ods.Tables("customer_dt1"), "house_name")
                Me.txtMLocation.DataBindings.Add("Text", ods.Tables("customer_dt1"), "LOCALITY")
                Me.txtMPhoneno.DataBindings.Add("Text", ods.Tables("customer_dt1"), "phone1")
                Me.txtMMobno.DataBindings.Add("Text", ods.Tables("customer_dt1"), "phone2")
                Me.IssueDt.DataBindings.Add("Text", ods.Tables("identity_values"), "issue_dt")
                Me.ExpiryDt.DataBindings.Add("Text", ods.Tables("identity_values"), "exp_dt")
                Me.txtIssuePlace.DataBindings.Add("Text", ods.Tables("identity_values"), "issue_plce")
                Me.txtMIdno.DataBindings.Add("Text", ods.Tables("identity_values"), "id_number")
                Me.Dobdt.DataBindings.Add("Text", ods.Tables("customer_dt2"), "date_of_birth")
                Me.txtMPanNo.DataBindings.Add("Text", ods.Tables("customer_dt2"), "pan")
                If ods.Tables("media_master_list").Rows.Count > 0 Then
                    Me.cmbMediaType.SelectedValue = ods.Tables("media_master_list").Rows(0)(1)
                    If ods.Tables("media_master_list").Rows(0)(1) = 2 AndAlso ods.Tables("media_master_list").Rows(0)(0) = 9 Then
                        Me.cmbMediaType.Enabled = False
                        Me.cmbMedia.Enabled = False
                    Else
                        Me.cmbMediaType.Enabled = True
                        Me.cmbMedia.Enabled = True
                    End If
                    Me.cmbMedia.SelectedValue = ods.Tables("media_master_list").Rows(0)(0)
                End If
                If ods.Tables("media_master_list").Rows.Count > 0 Then
                    If ods.Tables("customer_dt2").Rows(0)("descr").ToString = "EMPLOYEES" Then
                        Me.TextBox21.DataBindings.Add("Text", ods.Tables("customer_dt2"), "emp_code")
                        Label108.Visible = True
                        Me.TextBox22.DataBindings.Add("Text", ods.Tables("customer_dt2"), "emp_name")
                        Label109.Visible = True
                        Me.TextBox21.Visible = True
                        Me.TextBox22.Visible = True
                    Else
                        Label108.Visible = False
                        Label109.Visible = False
                        Me.TextBox21.Visible = False
                        Me.TextBox22.Visible = False
                    End If
                End If
                If ods.Tables("Custlang").Rows.Count > 0 Then
                    CmbLang.SelectedValue = ods.Tables("Custlang").Rows(0)(0)
                End If
                'If ods.Tables("media_master_list").Rows.Count > 0 Then
                '    If ods.Tables("media_master_list").Rows(0)(1) = 31 Then
                '        TextBox21.Enabled = False
                '        TextBox22.Enabled = False
                '    Else
                '        TextBox21.Enabled = True
                '        TextBox22.Enabled = True
                '    End If
                'End If
            End If
        Catch ex As Exception
            Throw New ApplicationException("fill_modi_text_controls:- " & ex.Message)
        End Try
    End Function

    Private Sub cmbMstate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMstate.LostFocus
        Try
            Me.Cursor = Cursors.WaitCursor
            'If Me.cmbMstate.SelectedValue Is Nothing Then
            '    Me.cmbMstate.SelectedIndex = 0
            '    loadfillModify()
            '    MsgBox("Please Select State From The List")
            '    Me.cmbMstate.Focus()
            'End If
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub cmbMstate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMstate.SelectedIndexChanged
        Try
            Me.Cursor = Cursors.WaitCursor
            'If Me.cmbMstate.Focused = True Then
            '    If Me.cmbMstate.Items.Count > 0 Then
            '        Mdistrictfill(Me.cmbMstate.SelectedValue)
            '    Else
            '        Me.cmbMDistrict.DataSource = dumdt
            '        Me.cmbMPost.DataSource = dumdt
            '        Me.txtMPincode.Text = ""
            '    End If
            'End If
            If Me.cmbMstate.SelectedIndex > 0 AndAlso Me.cmbMstate.Items.Count > 0 Then
                Mdistrictfill(Me.cmbMstate.SelectedValue)
            Else
                Me.cmbMDistrict.DataSource = dumdt
                Me.cmbMPost.DataSource = dumdt
                Me.txtMPincode.Text = ""
            End If
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub cmbMDistrict_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMDistrict.LostFocus
        Try
            Me.Cursor = Cursors.WaitCursor
            'If Me.cmbMDistrict.SelectedValue Is Nothing Then
            '    Me.cmbMDistrict.SelectedIndex = 0
            '    loadfillModify()
            '    MsgBox("Please Select District From The List")
            '    Me.cmbMDistrict.Focus()
            'End If
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub cmbMDistrict_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMDistrict.SelectedIndexChanged
        Try
            Me.Cursor = Cursors.WaitCursor
            'If Me.cmbMDistrict.Focused = True Then
            '    If Me.cmbMDistrict.Items.Count > 0 Then
            '        MPostfill(Me.cmbMDistrict.SelectedValue)
            '    Else
            '        Me.cmbMPost.DataSource = dumdt
            '        Me.txtMPincode.Text = ""
            '    End If
            'End If
            If Me.cmbMDistrict.SelectedIndex > 0 AndAlso Me.cmbMDistrict.Items.Count > 0 Then
                MPostfill(Me.cmbMDistrict.SelectedValue)
            Else
                Me.cmbMPost.DataSource = dumdt
                Me.txtMPincode.Text = ""
            End If
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub cmbMPost_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMPost.LostFocus
        Try
            Me.Cursor = Cursors.WaitCursor
            'If Me.cmbMPost.SelectedValue Is Nothing Then
            '    Me.cmbMPost.SelectedIndex = 0
            '    loadfillModify()
            '    MsgBox("Please Select Post From The List")
            '    Me.cmbMPost.Focus()
            'End If
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub cmbMPost_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMPost.SelectedIndexChanged
        Try
            Me.Cursor = Cursors.WaitCursor
            'If Me.cmbMPost.Focused = True Then
            '    If Me.cmbMPost.Items.Count > 0 Then
            '        MPinfill(Me.cmbMPost.SelectedValue)
            '    Else
            '        Me.txtMPincode.Text = ""
            '    End If
            'End If
            If Me.cmbMPost.SelectedIndex > 0 AndAlso Me.cmbMPost.Items.Count > 0 Then
                MPinfill(Me.cmbMPost.SelectedValue)
            Else
                Me.txtMPincode.Text = ""
            End If
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub cmbMediaType_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMediaType.LostFocus
        Try
            Me.Cursor = Cursors.WaitCursor
            'If Me.cmbMediaType.SelectedValue Is Nothing Then
            '    Me.cmbMediaType.SelectedIndex = 0
            '    loadfillModify()
            '    MsgBox("Please Select MediaType From The List")
            '    Me.cmbMediaType.Focus()
            'End If
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub cmbMediaType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMediaType.SelectedIndexChanged
        Try
            Me.Cursor = Cursors.WaitCursor
            'If Me.cmbMediaType.Focused = True Then
            '    If Me.cmbMediaType.Items.Count > 0 Then
            '        MMediafill(Me.cmbMediaType.SelectedValue)
            '    End If
            'End If

            'If Me.cmbMediaType.Focused = True Then
            '    If Me.cmbMediaType.SelectedValue = 2 AndAlso cmbMedia.SelectedValue = 8 OrElse Me.cmbMcuststat.SelectedValue = 4 Then
            '        Me.Label108.Visible = True
            '        Me.TextBox21.Visible = True
            '        Me.Label109.Visible = True
            '        Me.TextBox22.Visible = True
            '        If TextBox21.Text.Length > 0 Then
            '            Me.TextBox21.Enabled = False
            '        Else
            '            Me.TextBox21.Enabled = True
            '        End If
            '        Me.TextBox22.Enabled = False
            '    Else
            '        Me.Label108.Visible = False
            '        Me.TextBox21.Visible = False
            '        Me.Label109.Visible = False
            '        Me.TextBox22.Visible = False
            '    End If
            'End If




            'If Me.cmbMcuststat.Items.Count > 0 Then
            '    If Me.cmbMediaType.SelectedIndex > 0 AndAlso Me.cmbMediaType.SelectedValue = 31 Then
            '        Me.Label108.Visible = True
            '        Me.TextBox21.Visible = True
            '        Me.Label109.Visible = True
            '        Me.TextBox22.Visible = True
            '        Me.TextBox21.Enabled = False
            '        Me.TextBox22.Enabled = False
            '    Else
            '        Me.Label108.Visible = False
            '        Me.TextBox21.Visible = False
            '        Me.Label109.Visible = False
            '        Me.TextBox22.Visible = False
            '    End If
            'End If
            If Me.cmbMediaType.SelectedIndex > 0 AndAlso Me.cmbMediaType.Items.Count > 0 Then
                MMediafill(Me.cmbMediaType.SelectedValue)
            End If

        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub btnMConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMConfirm.Click
        Dim wise As New customerService.customer
        Dim dtCurentDate As Date = Format(wise.getDate(), "dd/MMM/yyyy")
        If dtCurentDate = SearchCustomer.ModuleOpen Then
            If Me.txtMPincode.Text = "" Then
                MsgBox("Please seclect the post")
                Exit Sub
            ElseIf RTrim(LTrim(Me.txtMCustnm.Text)) = "" Then   'Added By Tijo for Customername modification
                MsgBox("Please Enter the Customer Name")
                Me.txtMCustnm.Focus()
                flag = 0
                Exit Sub
            ElseIf Me.rb_MaleMod.Checked = False AndAlso Me.rb_FemaleMod.Checked = False Then
                MsgBox("Please Select Customer Gender")
                flag = 0
                Exit Sub
            ElseIf Me.txtMFatHus.Text = "" Then
                MsgBox("Please enter value in the field *S/o or D/o or W/o:")
                Exit Sub
            ElseIf Me.txtMHno.Text = "" Then
                MsgBox("Please enter value in the field *Name Of House / Apartment  / Door No:")
                Exit Sub
            ElseIf Me.txtMLocation.Text = "" Then
                MsgBox("Please enter value in the field *Location / Land mark:")
                Exit Sub
                'ElseIf Me.txtMIdno.Text = "" Then
                '    MsgBox("Please enter value in field Id no")
                '    Exit Sub
            ElseIf Me.txtMPhoneno.Text = "" Then
                MsgBox("Please enter value in the field *Phone No:")
                Exit Sub
            ElseIf Me.txtMMobno.Text = "" Then
                MsgBox("Please enter value in the field *Mobile No:")
                Exit Sub
            ElseIf ((Me.txtMMobno.TextLength = 10) And (Me.txtMMobno.Text.StartsWith("0"))) Then
                MsgBox("Invalid Mobile number. Please enter mobile number without the prefix zero.")
                Exit Sub
            ElseIf CDate(Me.Dobdt.Text) = #2/17/1753# Then
                MsgBox("Please enter the Date of birth")
                Exit Sub
            ElseIf DateDiff(DateInterval.Day, Date.Now, CDate(Me.Dobdt.Text)) >= 0 Then
                MsgBox("Please enter the Date of birth")
                Exit Sub
            ElseIf Me.cmbMid.SelectedIndex <= 0 Then
                MsgBox("Please select the id type")
                Exit Sub
            ElseIf DateDiff(DateInterval.Day, Date.Now, CDate(Me.IssueDt.Text)) > 0 Then
                MsgBox("Please enter the correct Date of Issue:")
                Exit Sub
            ElseIf DateDiff(DateInterval.Day, CDate(Me.IssueDt.Text), CDate(Me.ExpiryDt.Text)) < 1 Then
                MsgBox("Please enter the correct Id Expiry Date:")
                Exit Sub
            ElseIf Me.txtMIdno.Text = "" Then
                MsgBox("Please enter value in the feild *Id No:")
                Exit Sub
            ElseIf Me.cmbMediaType.SelectedIndex = 0 Then
                MsgBox("Please select the MediaType", MsgBoxStyle.Information, "Check it")
                Me.cmbMediaType.Focus()
                Exit Sub
            ElseIf Me.cmbMedia.SelectedIndex = 0 Then
                MsgBox("Please select the Media", MsgBoxStyle.Information, "Check it")
                Me.cmbMediaType.Focus()
                Exit Sub
            ElseIf Me.cmbMoccu.SelectedIndex = 0 Then
                MsgBox("Please select the business", MsgBoxStyle.Information, "Check it")
                Me.cmbMoccu.Focus()
                Exit Sub
            ElseIf Me.CmbMPep.SelectedValue = 0 Then
                MsgBox("Please select the PEP", MsgBoxStyle.Information, "Check it")
                Me.cmbMoccu.Focus()
                Exit Sub
            ElseIf Me.txtMIdno.Text.Length > 0 Then
                Dim blnIsDuplicateID As Boolean = False
                If Not cmbMPost.SelectedValue() Is Nothing Then
                    Dim objService As New customerService.customer(set_ip)
                    Dim PinSerial() As String = cmbMPost.SelectedValue().ToString.Split("@")
                    If cmbMid.SelectedValue() = 4 OrElse cmbMid.SelectedValue() = 504 OrElse cmbMid.SelectedValue() = 554 Then
                        blnIsDuplicateID = objService.CheckForDuplicateID(txtMIdno.Text.Trim(), 1, Convert.ToInt32(PinSerial(1)), 2, txtMCustid.Text.Trim())
                    Else
                        blnIsDuplicateID = objService.CheckForDuplicateID(txtMIdno.Text.Trim(), 2, Convert.ToInt32(PinSerial(1)), 2, txtMCustid.Text.Trim())
                    End If
                    If blnIsDuplicateID = True Then
                        MsgBox("Customer With Same Id No: Already Exist ...")
                        Me.txtAIdno.Focus()
                        flag = 0
                        Exit Sub
                    End If
                End If
            ElseIf Me.txtMCardNo.Text <> "" Then
                If Me.IssueDt.Text = "" Then
                    MsgBox("Please Enter the Issue Date")
                    Me.IssueDt.Focus()
                    Exit Sub
                End If
                If DateDiff(DateInterval.Day, CDate(Me.IssueDt.Text), CDate(Me.ExpiryDt.Text)) < 1 Then
                    MsgBox("Please enter the correct  Expiry Date:")
                    Me.ExpiryDt.Text = ""
                    Me.ExpiryDt.Focus()
                    Exit Sub
                End If
                If Me.txtIssuePlace.Text = "" Then
                    MsgBox("Please enter Issue Place:")
                    Me.txtIssuePlace.Focus()
                    Exit Sub
                End If
                If Me.txtMMobno.Text = "" Then
                    MsgBox("Please enter the Mobile number")
                    Me.txtMMobno.Focus()
                    Exit Sub
                End If
                If Me.cmbMid.SelectedIndex <= 0 Then
                    MsgBox("Please select the id type")
                    Exit Sub
                End If
                '=========
                Dim obj As New customerService.customer(set_ip)
                Dim st As Integer
                st = obj.cardcheck(Me.txtMCardNo.Text, BranchID)
                If st = 0 Then
                    MsgBox("Already issued")
                    Me.txtMCardNo.Text = ""
                ElseIf st = 1 Then
                ElseIf st = 2 Then
                    MsgBox("Check Cardno")
                    Me.txtMCardNo.Text = ""
                End If
            ElseIf Me.txtMPanNo.Text <> "" Then
                If Me.txtMPanNo.Text.Length = 10 Then
                    Dim hid As String = Me.txtMPanNo.Text.Substring(0, Me.txtMPanNo.Text.Length - 5)
                    Dim hid2 As String = Me.txtMPanNo.Text.Substring(5, Me.txtMPanNo.Text.Length - 6)
                    Dim hid3 As String = Me.txtMPanNo.Text.Substring(9, Me.txtMPanNo.Text.Length - 9)
                    Dim i As Integer
                    For i = 0 To hid.Length - 1
                        CheckAtoZ(hid(i))
                        If ss = False Then
                            MsgBox("Must Enter Correct Format,ie First 5 alphabets,then 4 nuemeric,last i alphabet", MsgBoxStyle.Information, "Check it")
                            Me.txtMPanNo.Text = ""
                            Me.txtMPanNo.Focus()
                            Exit Sub
                        End If
                    Next
                    Dim j As Integer
                    For j = 0 To hid2.Length - 1
                        IsNumeric(hid2(j))
                        If ss = False Then
                            MsgBox("Must Enter Correct Format,ie First 5 alphabets,then 4 nuemeric,last i alphabet", MsgBoxStyle.Information, "Check it")
                            Me.txtMPanNo.Text = ""
                            Me.txtMPanNo.Focus()
                            Exit Sub
                        End If
                    Next
                    CheckAtoZ(hid3)
                    If ss = False Then
                        MsgBox("Must Enter Correct Format,ie First 5 alphabets,then 4 nuemeric,last i alphabet", MsgBoxStyle.Information, "Check it")
                        Me.txtMPanNo.Text = ""
                        Me.txtMPanNo.Focus()
                        Exit Sub
                    End If
                Else
                    MsgBox("Must Enter Correct Format,ie First 5 alphabets,then 4 nuemeric,last i alphabet", MsgBoxStyle.Information, "Check it")
                    Me.txtMPanNo.Text = ""
                    Me.txtMPanNo.Focus()
                    Exit Sub
                End If

            End If

            If Me.cmbMPost.SelectedIndex <= 0 Then
                MsgBox("Please select the Post")
                Exit Sub
            End If

            Dim iskycAuthReq As Boolean = True
            If (idtype = "0" OrElse idtype = "") And idno = "" Then
                iskycAuthReq = False
            End If
            ''''''''''''''''''
            'Dim cust_value(21) As String req 7022
            'Dim cust_value(26) As String
            Dim cust_value(30) As String
            Dim date_var(5) As Date
            Dim modi_flag As String = ""

            cust_value(0) = txtMCustid.Text

            If Me.txtMFatHus.Modified Then
                cust_value(1) = txtMFatHus.Text
                modi_flag &= CStr(1) & "!"
            Else
                modi_flag &= CStr(0) & "!"
                cust_value(1) = txtMFatHus.Text
            End If
            If txtMHno.Modified Then
                cust_value(2) = txtMHno.Text
                modi_flag &= CStr(1) & "!"
            Else
                modi_flag &= CStr(0) & "!"
                cust_value(2) = txtMHno.Text
            End If
            If txtMLocation.Modified = True Then
                cust_value(3) = txtMLocation.Text
                modi_flag &= CStr(1) & "!"
            Else
                modi_flag &= CStr(0) & "!"
                cust_value(3) = txtMLocation.Text
            End If

            cust_value(4) = GetValue(cmbMPost) 'CStr(cmbMPost.SelectedValue)
            modi_flag &= CStr(1) & "!"

            cust_value(5) = GetValue(cmbMcuststat) 'CStr(cmbMcuststat.SelectedValue)
            modi_flag &= CStr(1) & "!"


            cust_value(6) = GetValue(cmbMoccu) 'CStr(cmbMoccu.SelectedValue)
            modi_flag &= CStr(1) & "!"

            If txtMPhoneno.Modified = True Then
                cust_value(7) = txtMPhoneno.Text
                modi_flag &= CStr(1) & "!"
            Else
                cust_value(7) = txtMPhoneno.Text
                modi_flag &= CStr(0) & "!"
            End If
            If txtMMobno.Modified = True Then
                'If (Me.txtMMobno.TextLength = 10) Then
                '    Me.txtMMobno.Text = "0" & txtMMobno.Text
                'End If
                cust_value(8) = txtMMobno.Text
                modi_flag &= CStr(1) & "!"
            Else
                cust_value(8) = txtMMobno.Text
                modi_flag &= CStr(0) & "!"
            End If

            If Me.txtMCardNo.Modified = True Then
                cust_value(9) = Me.txtMCardNo.Text  'email
                modi_flag &= CStr(1) & "!"
            Else
                cust_value(9) = Me.txtMCardNo.Text  'email
                modi_flag &= CStr(0) & "!"
            End If



            cust_value(10) = GetValue(cmbMid) 'CStr(cmbMid.SelectedValue)
            modi_flag &= IIf(iskycAuthReq = True, CStr(0), CStr(1)) & "!"  ' code modified to restrict kyc type directly by george

            If Me.txtMIdno.Modified = True Then
                cust_value(11) = txtMIdno.Text
                'modi_flag &= IIf(iskycAuthReq = True, CStr(0), CStr(1)) & "!"  ' code modified to restrict kyc type directly by george
                modi_flag &= CStr(1) & "!"   'commented the above line and added the line ahead by Tijo for Customer Name related change.
            Else
                modi_flag &= CStr(0) & "!"
                cust_value(11) = txtMIdno.Text
            End If

            date_var(0) = CDate(IssueDt.Text)
            modi_flag &= CStr(1) & "!"

            date_var(1) = CDate(ExpiryDt.Text)
            modi_flag &= CStr(1) & "!"

            If Me.txtMIdno.Modified = True Then
                cust_value(12) = txtIssuePlace.Text
                modi_flag &= CStr(1) & "!"
            Else
                modi_flag &= CStr(0) & "!"
                cust_value(12) = txtIssuePlace.Text
            End If
            date_var(2) = CDate(Dobdt.Text)
            modi_flag &= CStr(1) & "!"

            cust_value(13) = CStr(Me.txtMPanNo.Text) 'descr
            modi_flag &= CStr(0) & "!"


            modi_flag &= CStr(1) & "!"
            cust_value(14) = CStr(txtMLocation.Text)
            modi_flag &= CStr(1) & "!"

            modi_flag &= CStr(1) & "!"
            cust_value(15) = GetValue(cmbMedia) 'CStr(Me.cmbMediaType.SelectedValue)
            modi_flag &= CStr(1) & "!"

            modi_flag &= CStr(1) & "!"
            cust_value(16) = ModuleID
            modi_flag &= CStr(1) & "!"

            If Me.CheckBox2.Checked = True Then
                cust_value(17) = "1"
            Else
                cust_value(17) = "0"
            End If

            cust_value(18) = CStr(Me.TextBox20.Text)

            If Me.TextBox21.Text = "" Then
                cust_value(19) = "0"
            Else
                cust_value(19) = CStr(Me.TextBox21.Text)
            End If

            cust_value(20) = CStr(Me.TextBox22.Text)

            cust_value(21) = GetValue(cmbMediaType)
            'cust_value(22) = Address.Item("housename")
            'cust_value(23) = Address.Item("locality")
            'cust_value(24) = Address.Item("Post")
            If Not IsNothing(Address) AndAlso Address.Count > 0 Then
                cust_value(22) = Address.Item("housename")
                cust_value(23) = Address.Item("locality")
                cust_value(24) = Address.Item("Post")
            Else
                cust_value(22) = ""
                cust_value(23) = ""
                cust_value(24) = "0@0"
            End If

            cust_value(25) = UserId

            If rdoIndividualM.Checked Then
                cust_value(26) = "1"
            Else
                cust_value(26) = "2"
            End If

            If txtMCustnm.Modified Then  'Added By Tijo for Customername modification
                modi_flag &= CStr(1) & "!"
                cust_value(27) = txtMCustnm.Text
            Else
                modi_flag &= CStr(0) & "!"
                cust_value(27) = txtMCustnm.Text
            End If
            If rb_MaleMod.Checked Then
                cust_value(28) = "1"
            Else
                cust_value(28) = "0"
            End If
            Dim lang As Integer
            If GetValue(CmbLang) = "" Then
                lang = 0
            Else
                lang = GetValue(CmbLang)
            End If
            cust_value(29) = lang 'CStr(CmbLang.SelectedValue)
            modi_flag &= CStr(1) & "!"


            cust_value(30) &= CStr(CmbMPep.SelectedValue)

            Dim error_stat As String
            Try
                Me.Cursor = Cursors.WaitCursor
                Dim res As String = ""
                Dim dsRes As DataSet
                Dim dtRes As DataTable
                Dim ws As New customerService.customer(set_ip)
                Dim bID As String = ""
                Dim sql As String = "select Branch_Id from TBL_Vehicle_Finance where customer_id=" & txtMCustid.Text & ""
                dsRes = ws.QueryResult(sql)
                If dsRes.Tables.Count > 0 Then
                    dtRes = ws.QueryResult(sql).Tables(0)
                    If dtRes.Rows.Count > 0 Then
                        bID = dtRes.Rows(0)(0).ToString
                    End If
                End If
                error_stat = ws.customer_modification(cust_value, date_var, 2, modi_flag)
                If Not bID = "" Then
                    res = ws.Vehicle_Finance_Surv_Update(txtMCustid.Text, bID, VehicleFinance.CommVehOwnership, VehicleFinance.CommOwnershipVehType, VehicleFinance.TruckReq, VehicleFinance.ReqCommVeh, VehicleFinance.ReqCommVehicleType, VehicleFinance.ReqOtherVeh, VehicleFinance.ReqOtherVehDetail)
                    VehicleFinance.Mode = "Done"
                End If

                If CDbl(error_stat) = 1 Then
                    MsgBox("Customer has been Modified in the Database")
                    If ReferenceDetails.IsModified = True Then
                        SaveCustomerRefDetails(txtMCustid.Text)
                    End If
                    If MobileOTP.IsModified = True Then
                        SaveCustomerMobOTPDetails(txtMCustid.Text, MobileOTP.strMobileNumOTP, MobileOTP.strOTPValue)
                    End If
                Else
                    MsgBox("Customer has not been Modfied in the Database.please inform IT")
                End If
                'Commented  the following code bt Tijo as the same authorization is implemented in customer name related change
                'If iskycAuthReq = True AndAlso (cmbMid.SelectedValue <> CInt(idtype) OrElse Me.txtMIdno.Modified = True) Then
                '    Dim message As String = ws.AddKycPreAuth(Me.txtCustomerid.Text, BranchID, UserId, GetValue(cmbMid), txtMIdno.Text)
                '    If message <> "Complete" Then
                '        MessageBox.Show(message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Information)
                '    Else
                '        MessageBox.Show("your request to modify kyc is pending for risk deparment approval.", "Customer", MessageBoxButtons.OK, MessageBoxIcon.Information)
                '    End If
                'End If

            Catch ex As Exception 'added for req 4829
                MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                Me.Cursor = Cursors.Default
            End Try
        Else
            MsgBox("Latest Version available....Please Download http://app.manappuram.net/CustomerNew/publish.htm")
            Exit Sub
        End If

        ''''''''''''''''''
    End Sub

    Private Sub btnMExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMExit.Click
        Me.Main_tab.SelectedTab = tb_search
    End Sub

    Private Sub cmbMcountry_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMcountry.SelectedIndexChanged
        Try
            Me.Cursor = Cursors.WaitCursor
            'If Me.cmbMcountry.Focused = True Then
            '    If Me.cmbMcountry.Items.Count > 0 Then
            '        Mstatefill(Me.cmbMcountry.SelectedValue)
            '    Else
            '        Me.cmbMcountry.DataSource = dumdt
            '        Me.cmbMstate.DataSource = dumdt
            '        Me.cmbMDistrict.DataSource = dumdt
            '        Me.cmbMPost.DataSource = dumdt
            '        Me.txtMPincode.Text = ""
            '    End If
            'End If
            If Me.cmbMcountry.SelectedIndex > 0 AndAlso Me.cmbMcountry.Items.Count > 0 Then
                Mstatefill(Me.cmbMcountry.SelectedValue)
            Else
                Me.cmbMstate.DataSource = dumdt
                Me.cmbMDistrict.DataSource = dumdt
                Me.cmbMPost.DataSource = dumdt
                Me.txtMPincode.Text = ""
            End If
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Sub Mstatefill(ByVal countryid As Integer)
        Try
            Me.Cursor = Cursors.WaitCursor
            sql = "select state_id,state_name from state_master where country_id=" & countryid & ""
            combofill(sql, cmbMstate)
            If Me.cmbMstate.Items.Count > 0 Then
                Mdistrictfill(Me.cmbMstate.SelectedValue)
            Else
                Me.cmbMstate.DataSource = dumdt
                Me.cmbMDistrict.DataSource = dumdt
                Me.cmbMPost.DataSource = dumdt
                Me.txtMPincode.Text = ""
            End If
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Sub Astatefill(ByVal countryid As Integer)
        Try
            sql = "select state_id,state_name from state_master where country_id=" & countryid & ""
            combofill(sql, cmbState)
            If Me.cmbState.Items.Count > 0 Then
                adistrictfill(Me.cmbState.SelectedValue)
            Else
                Me.cmbState.DataSource = dumdt
                Me.cmbDistrict.DataSource = dumdt
                Me.cmbPost.DataSource = dumdt
                Me.txtPincode.Text = ""
            End If
        Catch ex As Exception 'added for req 4829
            Throw New ApplicationException("Astatefill:- " & ex.Message)
        End Try
    End Sub

    Private Sub languge_id()
        Dim sql As String
        Dim dt1 As New DataTable
        sql = "select t.language from tbl_ekyconsent t"
        combofill(sql, cmb_lang)
        cmb_lang.DataSource = dt1
    End Sub
    '  End Sub

    Sub Mdistrictfill(ByVal stateid As Integer)
        Try
            sql = "select district_id,district_name from district_master where state_id=" & stateid & ""
            combofill(sql, cmbMDistrict)
            If Me.cmbMDistrict.Items.Count > 0 Then
                MPostfill(Me.cmbMDistrict.SelectedValue)
            Else
                Me.cmbMDistrict.DataSource = dumdt
                Me.cmbMPost.DataSource = dumdt
                Me.txtMPincode.Text = ""
            End If
        Catch ex As Exception 'added for req 4829
            Throw New ApplicationException("Mdistrictfill:- " & ex.Message)
        End Try
    End Sub

    Sub adistrictfill(ByVal stateid As Integer)
        Try
            sql = "select district_id,district_name from district_master where state_id=" & stateid & ""
            combofill(sql, cmbDistrict)
            If Me.cmbDistrict.Items.Count > 0 Then
                APostfill(Me.cmbDistrict.SelectedValue)
            Else
                Me.cmbDistrict.DataSource = dumdt
                Me.cmbPost.DataSource = dumdt
                Me.txtPincode.Text = ""
            End If
        Catch ex As Exception 'added for req 4829
            Throw New ApplicationException("adistrictfill:- " & ex.Message)
        End Try
    End Sub

    Sub MPostfill(ByVal districtid As Integer)
        Try
            sql = "select pin_code ||'@'||sr_number as pincode,post_office from post_master where district_id=" & districtid & " and  status_id = 1 order by post_office"
            combofill(sql, cmbMPost)
            If Me.cmbMPost.Items.Count > 0 Then
                MPinfill(Me.cmbMPost.SelectedValue)
            Else
                Me.cmbMPost.DataSource = dumdt
                Me.txtMPincode.Text = ""
            End If
        Catch ex As Exception 'added for req 4829
            Throw New ApplicationException("MPostfill:- " & ex.Message)
        End Try
    End Sub

    Sub APostfill(ByVal districtid As Integer)
        Try
            sql = "select pin_code ||'@'||sr_number as pincode,post_office from post_master where district_id=" & districtid & " and  status_id = 1 order by post_office"
            combofill(sql, cmbPost)
            If Me.cmbPost.Items.Count > 0 Then
                APinfill(Me.cmbPost.SelectedValue)
            Else
                Me.cmbPost.DataSource = dumdt
                Me.txtPincode.Text = ""
            End If
        Catch ex As Exception 'added for req 4829
            Throw New ApplicationException("APostfill:- " & ex.Message)
        End Try
    End Sub

    Sub MPinfill(ByVal Postid As String)
        Dim pincode() As String
        pincode = Postid.ToString.Split("@")
        Me.txtMPincode.Text = pincode(0)
    End Sub

    Sub APinfill(ByVal Postid As String)
        Dim pincode() As String
        pincode = Postid.ToString.Split("@")
        Me.txtPincode.Text = pincode(0)
    End Sub

    Sub MMediafill(ByVal typeid As Integer)
        sql = "select media_id,media_name from media_master_new where type_id=" & typeid
        combofill(sql, cmbMedia)
    End Sub

    Sub loadfillModify()
        Try
            Dim ws As New customerService.customer(set_ip)
            Dim ds As New DataSet
            ds = ws.Combofill(BranchID)
            Dim ds1 As String
            ds1 = ws.branchState(BranchID)

            Dim ds2 As String
            ds2 = ws.branchDistrict(BranchID)
            Me.cmbMcuststat.DataSource = Addhead(ds.Tables("customer_type"), ds.Tables("customer_type").Columns(1).ColumnName, ds.Tables("customer_type").Columns(0).ColumnName) 'added for req 4829
            Me.cmbMcuststat.ValueMember = ds.Tables("customer_type").Columns(0).ColumnName
            Me.cmbMcuststat.DisplayMember = ds.Tables("customer_type").Columns(1).ColumnName

            Me.cmbMoccu.DataSource = Addhead(ArrangeData(ds.Tables("occupation_master")), ds.Tables("occupation_master").Columns(1).ColumnName, ds.Tables("occupation_master").Columns(0).ColumnName) 'added for req 4829
            Me.cmbMoccu.ValueMember = ds.Tables("occupation_master").Columns(0).ColumnName
            Me.cmbMoccu.DisplayMember = ds.Tables("occupation_master").Columns(1).ColumnName

            Me.cmbMid.DataSource = Addhead(ds.Tables("identity"), ds.Tables("identity").Columns(1).ColumnName, ds.Tables("identity").Columns(0).ColumnName) 'added for req 4829
            Me.cmbMid.ValueMember = ds.Tables("identity").Columns(0).ColumnName
            Me.cmbMid.DisplayMember = ds.Tables("identity").Columns(1).ColumnName

            Me.cmbMcountry.DataSource = Addhead(ds.Tables("country_dtl"), ds.Tables("country_dtl").Columns(1).ColumnName, ds.Tables("country_dtl").Columns(0).ColumnName) 'added for req 4829
            Me.cmbMcountry.ValueMember = ds.Tables("country_dtl").Columns(0).ColumnName
            Me.cmbMcountry.DisplayMember = ds.Tables("country_dtl").Columns(1).ColumnName

            Me.cmbMstate.DataSource = Addhead(ds.Tables("state_master"), ds.Tables("state_master").Columns(1).ColumnName, ds.Tables("state_master").Columns(0).ColumnName) 'added for req 4829
            Me.cmbMstate.ValueMember = ds.Tables("state_master").Columns(0).ColumnName
            Me.cmbMstate.DisplayMember = ds.Tables("state_master").Columns(1).ColumnName
            'Me.cmbMstate.SelectedValue = ds1

            Me.cmbMDistrict.DataSource = Addhead(ds.Tables("district_master"), ds.Tables("district_master").Columns(1).ColumnName, ds.Tables("district_master").Columns(0).ColumnName) 'added for req 4829
            Me.cmbMDistrict.ValueMember = ds.Tables("district_master").Columns(0).ColumnName
            Me.cmbMDistrict.DisplayMember = ds.Tables("district_master").Columns(1).ColumnName
            'Me.cmbMDistrict.SelectedValue = CInt(ds2)

            Me.cmbMPost.DataSource = Addhead(ds.Tables("post_master"), ds.Tables("post_master").Columns(1).ColumnName, ds.Tables("post_master").Columns(0).ColumnName)  'added for req 4829
            Me.cmbMPost.ValueMember = ds.Tables("post_master").Columns(0).ColumnName
            Me.cmbMPost.DisplayMember = ds.Tables("post_master").Columns(1).ColumnName

            Dim pin() As String
            If ds.Tables("post_master").Rows.Count > 0 Then
                pin = ds.Tables("post_master").Rows(0)(0).ToString.Split(CChar("@"))
                Me.txtMPincode.Text = pin(0)
            Else
                Me.txtMPincode.Text = "0"
            End If

            Me.cmbMediaType.DataSource = Addhead(ds.Tables("media_type"), ds.Tables("media_type").Columns(1).ColumnName, ds.Tables("media_type").Columns(0).ColumnName) 'added for req 4829
            Me.cmbMediaType.ValueMember = ds.Tables("media_type").Columns(0).ColumnName
            Me.cmbMediaType.DisplayMember = ds.Tables("media_type").Columns(1).ColumnName

            Me.cmbMedia.DataSource = Addhead(ds.Tables("media_master"), ds.Tables("media_master").Columns(1).ColumnName, ds.Tables("media_master").Columns(0).ColumnName) 'added for req 4829
            Me.cmbMedia.ValueMember = ds.Tables("media_master").Columns(0).ColumnName
            Me.cmbMedia.DisplayMember = ds.Tables("media_master").Columns(1).ColumnName


        Catch ex As Exception 'added for req 4829
            Throw New ApplicationException("loadfillModify:- " & ex.Message)
        End Try
    End Sub

#End Region
#Region "tab add share details"

    Private Sub btnssexit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Main_tab.SelectedTab = Me.tb_search
    End Sub

    'Private Sub btnSsconfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Try
    '        Me.Cursor = Cursors.WaitCursor
    '        Dim confResult As String
    '        Dim obj As New customerService.customer(set_ip)
    '        confResult = obj.AddShare(FirmID, BranchID, Me.txtScustid.Text, Me.txtScustnm.Text, UserId)
    '        Dim shareResult() As String = confResult.Split("+")
    '        If shareResult(1) = "0" Then
    '            MsgBox(shareResult(0))
    '            'new_vcc(shareResult(2), 2, BranchID)
    '        Else
    '            MsgBox("Share added already")
    '        End If
    '    Catch ex As Exception 'added for req 4829
    '        MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    Finally
    '        Me.Cursor = Cursors.Default
    '    End Try
    'End Sub

    'Protected Sub ShareUpdateBinding()
    '    Try
    '        Me.txtScustid.DataBindings.Clear()
    '        Me.txtScustnm.DataBindings.Clear()
    '        Me.txtSfathus.DataBindings.Clear()
    '        Me.txtShouse.DataBindings.Clear()
    '        Me.txtSlocation.DataBindings.Clear()
    '        Me.txtSstate.DataBindings.Clear()
    '        Me.txtSdistrict.DataBindings.Clear()
    '        Me.txtSpost.DataBindings.Clear()
    '        Me.txtSpin.DataBindings.Clear()
    '        Me.txtScountry.DataBindings.Clear()
    '        Me.txtScode.DataBindings.Clear()
    '        Me.txtSphone.DataBindings.Clear()
    '        Me.txtSidno.DataBindings.Clear()
    '        Me.txtScardno.DataBindings.Clear()
    '        If Not cust_result Is Nothing Then
    '            '{"CUST_ID", "cust_name", "fat_hus", "house_name", "LOCALITY", "post_office", "pin_code", "phone1", "district_name", "state", "country"}
    '            Me.txtScustid.DataBindings.Add("Text", cust_result, "CUST_ID")
    '            '  Me.txtScustnm.DataBindings.Add("Text", cust_result, "cust_name")
    '            Me.txtScustnm.DataBindings.Add("Text", cust_result, "name")
    '            Me.txtSfathus.DataBindings.Add("Text", cust_result, "fat_hus")
    '            Me.txtShouse.DataBindings.Add("Text", cust_result, "house_name")
    '            Me.txtSlocation.DataBindings.Add("Text", cust_result, "LOCALITY")
    '            Me.txtSstate.DataBindings.Add("Text", cust_result, "state_name")
    '            Me.txtSdistrict.DataBindings.Add("Text", cust_result, "district_name")
    '            Me.txtSpost.DataBindings.Add("Text", cust_result, "post_office")
    '            Me.txtSpin.DataBindings.Add("Text", cust_result, "pin_code")
    '            Me.txtScountry.DataBindings.Add("Text", cust_result, "country_name")
    '            Me.txtSphone.DataBindings.Add("Text", cust_result, "phone1")
    '            Me.txtSidno.DataBindings.Add("Text", cust_result, "id_number")
    '            Me.txtSsamt.Text = 11
    '        End If
    '    Catch ex As Exception 'added for req 4829
    '        Throw New ApplicationException("ShareUpdateBinding" & ex.Message)
    '    End Try
    'End Sub
#End Region
#Region "tab tabclick details"
    Private Sub tb_customermodify_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles tb_customermodify.Enter
        Try
            CheckForOnlineCustomer()
            Me.Cursor = Cursors.WaitCursor
            If Me.txtCustomerid.Text.Trim <> "" Then
                Dim count, EmpPost As Integer
                Dim wsnew As New customerService.customer(set_ip)
                'count = wsnew.GetModiStatus(txtCustomerid.Text)
                'If (count = 0) Then
                If (custBranchID <> BranchID) Then
                    txtMCustnm.ReadOnly = True
                    txtMHno.ReadOnly = True
                    txtMLocation.ReadOnly = True
                    cmbMPost.Enabled = False
                    txtMLocation.ReadOnly = True
                    txtMPhoneno.ReadOnly = True
                    txtMMobno.ReadOnly = True
                    btn_ChangeMob.Enabled = False
                    bn_ModifyReference.Enabled = False
                    cmbMoccu.Enabled = False
                Else
                    If (custBranchID <> 0) Then
                        txtMCustnm.ReadOnly = False
                        txtMHno.ReadOnly = False
                        txtMLocation.ReadOnly = False
                        cmbMPost.Enabled = True
                        txtMLocation.ReadOnly = False
                        txtMPhoneno.ReadOnly = False
                        txtMMobno.ReadOnly = False
                        cmbMoccu.Enabled = True
                        btn_ChangeMob.Enabled = True
                        bn_ModifyReference.Enabled = True

                        EmpPost = wsnew.GetEmpPostID(UserId)
                        If ((EmpPost = 10) Or (EmpPost = 198)) Then
                            cmbMoccu.Enabled = False
                            txtMCustnm.ReadOnly = True
                        End If
                    Else
                        count = wsnew.GetRiskMgmtPostID(UserId)
                        If (count > 0) Then
                            txtMCustnm.ReadOnly = False
                            txtMHno.ReadOnly = False
                            txtMLocation.ReadOnly = False
                            cmbMPost.Enabled = True
                            txtMLocation.ReadOnly = False
                            txtMPhoneno.ReadOnly = False
                            txtMMobno.ReadOnly = False
                            cmbMoccu.Enabled = True
                        Else
                            txtMCustnm.ReadOnly = True
                            txtMHno.ReadOnly = True
                            txtMLocation.ReadOnly = True
                            cmbMPost.Enabled = False
                            txtMLocation.ReadOnly = True
                            txtMPhoneno.ReadOnly = True
                            txtMMobno.ReadOnly = True
                            cmbMoccu.Enabled = False
                        End If
                    End If
                End If


                Me.Main_tab.TabPages(1).Enabled = True
                Me.tb_customermodify.BackColor = Drawing.Color.LightSteelBlue
                If Me.picSearchdtl.Image IsNot Nothing Then
                    Me.picModify.Image = Me.picSearchdtl.Image
                    Me.picModify.SizeMode = PictureBoxSizeMode.StretchImage
                Else
                    Me.picModify.Image = Nothing
                End If
                Adress2.custid = txtCustomerid.Text

                If Not IsNothing(SearchCustomer.Address) AndAlso SearchCustomer.Address.Count > 0 Then
                    SearchCustomer.Address.Clear()
                End If
                Dim ws As New customerService.customer(set_ip)
                dt = ws.customermodiifyDisplaydata(txtCustomerid.Text, BranchID, FirmID)
                fill_modi_controls(dt)
                'Else
                '    MessageBox.Show("Customer modification approval pending for the  selected customer.", "Customer", MessageBoxButtons.OK, MessageBoxIcon.Information)
                '    Me.Main_tab.SelectedTab = Me.tb_search
                'End If
            Else
                MsgBox("Search Customer Details")
                Me.Main_tab.SelectedTab = Me.tb_search
            End If
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    'Private Sub tb_addShare_Enter(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Try
    '        CheckForOnlineCustomer()
    '        Me.Cursor = Cursors.WaitCursor
    '        Dim obj As New customerService.customer(set_ip)
    '        'Dim dt As New DataTable
    '        'dt = obj.QueryResult("select count(*) from active_firms where branch_id=" & BranchID & " and firm_id=2").Tables(0)
    '        'If dt.Rows(0)(0) > 0 Then
    '        If Me.txtCustomerid.Text <> "" Then

    '            cust_result = obj.DisplayCustomer(Me.txtCustomerid.Text, BranchID).Tables(0)
    '            If IsDBNull(cust_result.Rows(0)(14)) Then
    '                'Me.tb_addShare.BackColor = Drawing.Color.LightSteelBlue
    '                'ShareUpdateBinding()
    '                If Me.picSearchdtl.Image Is Nothing Then
    '                    Me.PictureBox2.Image = Nothing
    '                Else
    '                    Me.PictureBox2.Image = Me.picSearchdtl.Image
    '                    Me.PictureBox2.SizeMode = PictureBoxSizeMode.StretchImage
    '                End If
    '                Me.Main_tab.TabPages(5).Enabled = True
    '            Else
    '                MsgBox("This Customer Already  have a share")
    '                Me.Main_tab.SelectedTab = Me.tb_search
    '            End If
    '        Else
    '            MsgBox("Search Customer Details")
    '            Me.Main_tab.SelectedTab = Me.tb_search
    '        End If
    '        'Else
    '        'MsgBox("Only Maben Branches")
    '        'Me.Main_tab.SelectedTab = Me.tb_search
    '        'End If
    '    Catch ex As Exception 'added for req 4829
    '        MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    Finally
    '        Me.Cursor = Cursors.Default
    '    End Try
    'End Sub

    Private Sub tb_neftdtl_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles tb_neftdtl.Enter
        Try
            CheckForOnlineCustomer()
            neftScanCtl.Height = neftScanCtl.Height + 10
            neftScanCtl.Width = neftScanCtl.Width + 10
            neftScanCtl.AutoSize = False
            Me.neftScanCtl.Size = New System.Drawing.Size(594, 325)

            Me.Cursor = Cursors.WaitCursor
            Me.txtAccountNo.Text = ""
            Me.txtCfmAccountNo.Text = ""
            Me.txtPBCustnm.Text = ""
            If neft_status = 1 Then

                Me.Main_tab.TabPages(4).Enabled = True
                Me.tb_neftdtl.BackColor = Drawing.Color.LightSteelBlue
                grdneftDetails.Rows.Clear()
                fillneftdtl()
                Me.txtNeftCustid.Text = ""
                Me.txtNeftCustnm.Text = ""
                neftScanCtl.ResetImg()
                neftScanCtl.ScannedImagePath = ""
            Else
                If Me.txtCustomerid.Text <> "" Then
                    Me.Main_tab.TabPages(4).Enabled = True
                    fillneftdtl()
                    Me.txtNeftCustid.Text = Me.txtCustomerid.Text
                    Me.txtNeftCustnm.Text = Me.txtcustname.Text
                    neftScanCtl.ResetImg()
                    neftScanCtl.ScannedImagePath = ""
                    LoadNEFTPhoto(Me.txtCustomerid.Text)
                Else
                    MsgBox("Search Customer Details")
                    Me.Main_tab.SelectedTab = Me.tb_search
                End If
            End If
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub tb_addPhoto_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles tb_addPhoto.Enter
        'photoStatus = 0
        CheckForOnlineCustomer()

        Dim EmpPost As Integer
        Dim objService As New customerService.customer(set_ip)
        EmpPost = objService.GetEmpPostID(UserId)
        If photoStatus = 1 Then
            'photoStatus = 1
            If (custBranchID <> BranchID) Then
                MessageBox.Show("Can't modify other branch customer's Photo", "Customer", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.Main_tab.SelectedTab = Me.tb_search
            ElseIf ((EmpPost = 10) Or (EmpPost = 198)) Then
                MessageBox.Show("BH/BM cannot modify customer photo", "Customer", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.Main_tab.SelectedTab = Me.tb_search
            Else
                Me.btnStart.Visible = True
                Me.btnStop.Visible = True
                Me.Cmd_exit1.Visible = True
                Me.tb_addPhoto.BackColor = Drawing.Color.LightSteelBlue
                Me.Main_tab.TabPages(2).Enabled = True
            End If
        Else
            If txtCustomerid.Text = "" Then
                MsgBox("Search Customer Details")
                Me.Main_tab.SelectedTab = Me.tb_search
            Else
                If (custBranchID <> BranchID) Then
                    MessageBox.Show("Can't modify other branch customer's Photo", "Customer", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Me.Main_tab.SelectedTab = Me.tb_search
                ElseIf ((EmpPost = 10) Or (EmpPost = 198)) Then
                    MessageBox.Show("BH/BM cannot modify customer photo", "Customer", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Me.Main_tab.SelectedTab = Me.tb_search
                Else
                    Me.btnStart.Visible = True
                    Me.btnStop.Visible = True
                    Me.Cmd_exit1.Visible = True
                    Me.Main_tab.TabPages(2).Enabled = True
                End If
            End If
        End If
        Me.picAddPhoto.Image = Nothing
    End Sub

    Private Sub tb_addKyc_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles tb_addKyc.Enter
        Try
            CheckForOnlineCustomer()
            ScanX1.Height = ScanX1.Height + 10
            ScanX1.Width = ScanX1.Width + 10
            ScanX1.AutoSize = False
            Me.ScanX1.Size = New System.Drawing.Size(775, 373)


            Me.Cursor = Cursors.WaitCursor
            If kycStatus = 1 Then
                'Me.btnStartKyc.Enabled = True
                'Me.btnStopKyc.Enabled = True
                'Me.btnExitKyc.Enabled = True
                Me.tb_addKyc.BackColor = Drawing.Color.LightSteelBlue
                Me.Main_tab.TabPages(2).Enabled = True
                kycDBFile = ""
                EnableScan(True)
            Else
                If txtCustomerid.Text = "" Then
                    MsgBox("Search Customer Details")
                    Me.Main_tab.SelectedTab = Me.tb_search
                    Exit Sub
                Else
                    'Me.btnStartKyc.Enabled = True
                    'Me.btnStopKyc.Enabled = True
                    'Me.btnExitKyc.Enabled = True
                    'Me.Main_tab.TabPages(3).Enabled = True
                    'If custBranchID <> BranchID Then
                    '    'MessageBox.Show("Can't modify other branch customer's KYC document", "Customer", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    '    Dim result = MessageBox.Show("You are trying to modify other branch customer's KYC document..It will Require SRM Approval. Really want to Modify?", "Confirm", MessageBoxButtons.YesNo)
                    '    If result = DialogResult.Yes Then
                    '        EnableScan(True)
                    '    Else
                    '        EnableScan(False)
                    '    End If
                    'Else
                    '    EnableScan(True)
                    'End If
                    'MessageBox.Show("Uploading consent after customer creation is not allowed..!!", "Customer", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    'Me.Main_tab.SelectedTab = Me.tb_search
                    Exit Sub
                End If

            End If

            Dim objService As New customerService.customer(set_ip)
            Dim ds As New DataSet
            ds = objService.GetAddressProof()
            If (ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0) Then
                cmbAddressProf.DisplayMember = "description"
                cmbAddressProf.ValueMember = "status_id"
                cmbAddressProf.DataSource = ds.Tables(0)
            End If

            If cmbScanMode.Items.Count = 0 Then
                cmbScanMode.Items.Add("Gray Scale")
                cmbScanMode.Items.Add("Block & White")
                cmbScanMode.SelectedIndex = 0
            End If

            If kycStatus <> 1 Then
                If Convert.ToString(addProofID) = "" Then
                    chkIdentity.Checked = True
                    chkAddress.Checked = False
                    chkSameAsId.Checked = False
                    chkSameAsId.Visible = False
                    lblAddressProof.Visible = False
                    cmbAddressProf.Visible = False
                ElseIf Convert.ToString(addProofID) = "0" Then
                    chkIdentity.Checked = True
                    chkAddress.Checked = True
                    chkSameAsId.Checked = True
                    chkSameAsId.Visible = True
                    cmbAddressProf.Enabled = False
                ElseIf CInt(addProofID) > 0 Then
                    chkIdentity.Checked = True
                    chkAddress.Checked = True
                    chkSameAsId.Checked = False
                    chkSameAsId.Visible = True
                    cmbAddressProf.SelectedValue = CInt(addProofID)
                    cmbAddressProf.Enabled = True
                End If
            End If

            If File.Exists(kycDBFile) Then
                ScanX1.ImageFileLoad(kycDBFile)
                btnAppend.Enabled = True
            Else
                ScanX1.ResetImg()
                btnAppend.Enabled = False

            End If

        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub EnableScan(ByVal bol As Boolean)
        btSource.Enabled = bol
        btnScan.Enabled = bol
        btnUpload.Enabled = bol
        btnAppend.Enabled = bol
        btnReset.Enabled = bol
        btnInstruction.Enabled = bol
        btnLoadImage.Enabled = bol
        cmdAddMoreKyc.Enabled = bol
    End Sub

    Private Sub tb_addcustomer_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles tb_addcustomer.Enter
        Try
            'Me.Main_tab.TabPages(2).Enabled = False
            Main_tab.TabPages.Remove(tb_customermodify)
            Main_tab.TabPages.Remove(tb_addPhoto)
            'Main_tab.TabPages.Remove(tb_addKyc)
            Main_tab.TabPages.Remove(tb_neftdtl)
            Main_tab.TabPages.Remove(tab_Pan)
            cmbId.SelectedValue = 16
            Me.Cursor = Cursors.WaitCursor
            btnkycc.Visible = False
            txtACustName.ReadOnly = True
            txtAFatHus.ReadOnly = True
            txtHouse.ReadOnly = True
            txtALocation.ReadOnly = True
            cmbCountry.Enabled = False
            cmbState.Enabled = False
            cmbDistrict.Enabled = False
            Me.txtdob.Enabled = False
            Me.txtEmail.ReadOnly = False
            txtPincode.ReadOnly = True
            txtAIdno.Focus()
            loadfill()
            If Not IsNothing(SearchCustomer.Address) AndAlso SearchCustomer.Address.Count > 0 Then
                SearchCustomer.Address.Clear()
            End If
            If Me.txtCustomerid.Text = "" Then
                Me.tb_addcustomer.BackColor = Drawing.Color.AliceBlue
                Me.Main_tab.TabPages(0).Enabled = True
                flag = 0
                'loadfill()
                '  prenameFill()
                '  careoffnameFill()
                '    genderFill()
                MsgBox("The copies of ID Proof and Address Proof provided by the customer shall be verified with the originals; the details like ‘KYC Number’, ‘Customer Name’, ‘Date of Birth’, etc. shall be as per the loan application form and are to be cross verified with the proofs provided; and the ‘KYC Type’ to be selected is the ID proof provided by the customer.", , "Attention")
            End If
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region
#Region "tab New customer Add"

    'Sub prenameFill()
    '    Dim dt As New DataTable
    '    Dim dc1 As New DataColumn
    '    Dim dc2 As New DataColumn
    '    dt.Columns.Add(dc1)
    '    dt.Columns.Add(dc2)
    '    Dim dr As DataRow = dt.NewRow
    '    dr.Item(0) = "0"
    '    dr.Item(1) = "Mr:"
    '    Dim dr1 As DataRow = dt.NewRow
    '    dr1.Item(0) = "1"
    '    dr1.Item(1) = "Mrs:"
    '    Dim dr2 As DataRow = dt.NewRow
    '    dr2.Item(0) = "2"
    '    dr2.Item(1) = "Miss:"
    '    Dim dr3 As DataRow = dt.NewRow
    '    dr3.Item(0) = "3"
    '    dr3.Item(1) = "Baby:"
    '    Dim dr4 As DataRow = dt.NewRow
    '    dr4.Item(0) = "4"
    '    dr4.Item(1) = "Master:"
    '    dt.Rows.Add(dr)
    '    dt.Rows.Add(dr1)
    '    dt.Rows.Add(dr2)
    '    dt.Rows.Add(dr3)
    '    dt.Rows.Add(dr4)
    '    Me.cmbPre.DataSource = dt
    '    Me.cmbPre.DisplayMember = dt.Columns(1).ColumnName
    '    Me.cmbPre.ValueMember = dt.Columns(0).ColumnName
    'End Sub

    'Sub genderFill()
    '    Dim dt As New DataTable
    '    Dim dc1 As New DataColumn
    '    Dim dc2 As New DataColumn
    '    dt.Columns.Add(dc1)
    '    dt.Columns.Add(dc2)
    '    Dim dr As DataRow = dt.NewRow
    '    dr.Item(0) = "1"
    '    dr.Item(1) = "MALE"
    '    Dim dr1 As DataRow = dt.NewRow
    '    dr1.Item(0) = "0"
    '    dr1.Item(1) = "FEMALE"
    '    dt.Rows.Add(dr)
    '    dt.Rows.Add(dr1)
    '    Me.cmbGender.DataSource = dt
    '    Me.cmbGender.DisplayMember = dt.Columns(1).ColumnName
    '    Me.cmbGender.ValueMember = dt.Columns(0).ColumnName
    'End Sub

    'Sub careoffnameFill()
    '    Dim dt As New DataTable
    '    Dim dc1 As New DataColumn
    '    Dim dc2 As New DataColumn
    '    dt.Columns.Add(dc1)
    '    dt.Columns.Add(dc2)
    '    Dim dr As DataRow = dt.NewRow
    '    dr.Item(0) = "0"
    '    dr.Item(1) = "S/o:"
    '    Dim dr1 As DataRow = dt.NewRow
    '    dr1.Item(0) = "1"
    '    dr1.Item(1) = "D/o:"
    '    Dim dr2 As DataRow = dt.NewRow
    '    dr2.Item(0) = "2"
    '    dr2.Item(1) = "W/o:"
    '    Dim dr3 As DataRow = dt.NewRow
    '    dr3.Item(0) = "3"
    '    dr3.Item(1) = "F/o:"
    '    dt.Rows.Add(dr)
    '    dt.Rows.Add(dr1)
    '    dt.Rows.Add(dr2)
    '    dt.Rows.Add(dr3)
    '    Me.cmbiden.DataSource = dt
    '    Me.cmbiden.DisplayMember = dt.Columns(1).ColumnName
    '    Me.cmbiden.ValueMember = dt.Columns(0).ColumnName
    'End Sub
    Private Function PopulateData() As DataSet
        Dim obj As New customerService.customer(set_ip)
        Dim dsOut As New DataSet
        Dim xmlPath As String = AppDomain.CurrentDomain.BaseDirectory & "\XML\LoadData.xml"

        Try
            If File.Exists(xmlPath) Then
                Dim filein As New FileInfo(xmlPath)
                If (Now.ToString("dd-MMM-yyyy") <> filein.CreationTime.ToString("dd-MMM-yyyy")) Then
                    filein.Delete()
                End If
            Else
                If Not Directory.Exists(AppDomain.CurrentDomain.BaseDirectory & "\XML\") Then
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory & "\XML\")
                End If
            End If

            If File.Exists(xmlPath) Then
                dsOut.ReadXml(xmlPath, XmlReadMode.Auto)
                Return dsOut
            Else
                dsOut = obj.Combofill(BranchID)
                dsOut.WriteXml(xmlPath, XmlWriteMode.IgnoreSchema)
                Return dsOut
            End If
        Catch ex As Exception
            Return obj.Combofill(BranchID)
        End Try
    End Function
    Sub loadfill()
        Try
            Dim obj As New customerService.customer(set_ip)
            Dim ds, ds3 As New DataSet
            ' Dim ds3 As DataTable
            ds3 = obj.Combofill(BranchID)
            ds = obj.Combofill(BranchID) 'PopulateData() 
            Dim ds1 As String
            ds1 = obj.branchState(CInt(BranchID))
            Dim ds2 As String
            ds2 = obj.branchDistrict(CInt(BranchID))
            Me.cmbCustStatus.DataSource = Addhead(ds.Tables("customer_type"), ds.Tables("customer_type").Columns(1).ColumnName, ds.Tables("customer_type").Columns(0).ColumnName) 'Added for req 4829
            Me.cmbCustStatus.ValueMember = ds.Tables("customer_type").Columns(0).ColumnName
            Me.cmbCustStatus.DisplayMember = ds.Tables("customer_type").Columns(1).ColumnName

            Me.cmb_lang.DataSource = Addhead(ds.Tables("CONSENTLANG"), ds.Tables("CONSENTLANG").Columns(1).ColumnName, ds.Tables("CONSENTLANG").Columns(0).ColumnName) 'Added for req 4829
            Me.cmb_lang.ValueMember = ds.Tables("CONSENTLANG").Columns(0).ColumnName
            Me.cmb_lang.DisplayMember = ds.Tables("CONSENTLANG").Columns(1).ColumnName


            Me.cmbOccupation.DataSource = Addhead(ArrangeData(ds.Tables("occupation_master")), ds.Tables("occupation_master").Columns(1).ColumnName, ds.Tables("occupation_master").Columns(0).ColumnName) 'Added for req 4829
            Me.cmbOccupation.ValueMember = ds.Tables("occupation_master").Columns(0).ColumnName
            Me.cmbOccupation.DisplayMember = ds.Tables("occupation_master").Columns(1).ColumnName

            'Me.cmbId.DataSource = Addhead(ds.Tables("identity"), ds.Tables("identity").Columns(1).ColumnName, ds.Tables("identity").Columns(0).ColumnName) 'Added for req 4829
            'Me.cmbId.ValueMember = ds.Tables("identity").Columns(0).ColumnName
            'Me.cmbId.DisplayMember = ds.Tables("identity").Columns(1).ColumnName

            Me.cmbCountry.DataSource = Addhead(ds.Tables("country_dtl"), ds.Tables("country_dtl").Columns(1).ColumnName, ds.Tables("country_dtl").Columns(0).ColumnName) 'Added for req 4829
            Me.cmbCountry.ValueMember = ds.Tables("country_dtl").Columns(0).ColumnName
            Me.cmbCountry.DisplayMember = ds.Tables("country_dtl").Columns(1).ColumnName

            'Me.cmbState.DataSource = Addhead(ds.Tables("state_master"), ds.Tables("state_master").Columns(1).ColumnName, ds.Tables("state_master").Columns(0).ColumnName) 'Added for req 4829
            'Me.cmbState.ValueMember = ds.Tables("state_master").Columns(0).ColumnName
            'Me.cmbState.DisplayMember = ds.Tables("state_master").Columns(1).ColumnName
            ''Me.cmbState.SelectedValue = ds1
            'Me.cmbState.SelectedIndex = 0

            'Me.cmbDistrict.DataSource = Addhead(ds.Tables("district_master"), ds.Tables("district_master").Columns(1).ColumnName, ds.Tables("district_master").Columns(0).ColumnName) 'Added for req 4829
            'Me.cmbDistrict.ValueMember = ds.Tables("district_master").Columns(0).ColumnName
            'Me.cmbDistrict.DisplayMember = ds.Tables("district_master").Columns(1).ColumnName
            ''Me.cmbDistrict.SelectedValue = CInt(ds2)
            'Me.cmbDistrict.SelectedIndex = 0

            'Me.cmbPost.DataSource = Addhead(ds.Tables("post_master"), ds.Tables("post_master").Columns(1).ColumnName, ds.Tables("post_master").Columns(0).ColumnName) 'Added for req 4829
            'Me.cmbPost.ValueMember = ds.Tables("post_master").Columns(0).ColumnName
            'Me.cmbPost.DisplayMember = ds.Tables("post_master").Columns(1).ColumnName
            'Me.cmbPost.SelectedIndex = 0

            Me.cmb_landHldtl.DataSource = Addhead(ds.Tables("cmb_landHldtl"), ds.Tables("cmb_landHldtl").Columns(1).ColumnName, ds.Tables("cmb_landHldtl").Columns(0).ColumnName) 'Added for req 4829
            Me.cmb_landHldtl.DisplayMember = ds.Tables("cmb_landHldtl").Columns(1).ColumnName
            Me.cmb_landHldtl.ValueMember = ds.Tables("cmb_landHldtl").Columns(0).ColumnName

            'Me.ComboBox1.DataSource = Addhead(ds.Tables("cmb_cerDtls"), ds.Tables("cmb_cerDtls").Columns(1).ColumnName, ds.Tables("cmb_cerDtls").Columns(0).ColumnName) 'Added for req 4829
            'Me.ComboBox1.DisplayMember = ds.Tables("cmb_cerDtls").Columns(1).ColumnName
            'Me.ComboBox1.ValueMember = ds.Tables("cmb_cerDtls").Columns(0).ColumnName

            'Dim pin() As String
            'If ds.Tables("post_master").Rows.Count > 0 Then
            '    pin = ds.Tables("post_master").Rows(0)(0).ToString.Split(CChar("@"))
            '    Me.txtPincode.Text = pin(0)
            'Else
            '    Me.txtPincode.Text = "0"
            'End If

            Me.cmbCMediatype.DataSource = Addhead(ds.Tables("media_type"), ds.Tables("media_type").Columns(1).ColumnName, ds.Tables("media_type").Columns(0).ColumnName) 'Added for req 4829
            Me.cmbCMediatype.ValueMember = ds.Tables("media_type").Columns(0).ColumnName
            Me.cmbCMediatype.DisplayMember = ds.Tables("media_type").Columns(1).ColumnName

            'Me.cmbCMedia.DataSource = Addhead(ds.Tables("media_master"), ds.Tables("media_master").Columns(1).ColumnName, ds.Tables("media_master").Columns(0).ColumnName) 'Added for req 4829
            'Me.cmbCMedia.ValueMember = ds.Tables("media_master").Columns(0).ColumnName
            'Me.cmbCMedia.DisplayMember = ds.Tables("media_master").Columns(1).ColumnName

            Me.cmbReligion.DataSource = Addhead(ds.Tables("religion_master"), ds.Tables("religion_master").Columns(1).ColumnName, ds.Tables("religion_master").Columns(0).ColumnName) 'Added for req 4829
            Me.cmbReligion.ValueMember = ds.Tables("religion_master").Columns(0).ColumnName
            Me.cmbReligion.DisplayMember = ds.Tables("religion_master").Columns(1).ColumnName

            'Me.cmbCaste.DataSource = Addhead(ds.Tables("caste_master"), ds.Tables("caste_master").Columns(1).ColumnName, ds.Tables("caste_master").Columns(0).ColumnName)  'Added for req 4829
            'Me.cmbCaste.ValueMember = ds.Tables("caste_master").Columns(0).ColumnName
            'Me.cmbCaste.DisplayMember = ds.Tables("caste_master").Columns(1).ColumnName

            Me.cmbPre.DataSource = ds.Tables("prename_master")
            Me.cmbPre.DisplayMember = ds.Tables("prename_master").Columns(1).ColumnName
            Me.cmbPre.ValueMember = ds.Tables("prename_master").Columns(0).ColumnName


            Me.cmbpep.DataSource = ds.Tables("CUST_PEP")
            Me.cmbpep.DisplayMember = ds.Tables("CUST_PEP").Columns(1).ColumnName
            Me.cmbpep.ValueMember = ds.Tables("CUST_PEP").Columns(0).ColumnName

            Me.CmbMarital.DataSource = Addhead(ds.Tables("MARITAL"), ds.Tables("MARITAL").Columns(1).ColumnName, ds.Tables("MARITAL").Columns(0).ColumnName)
            Me.CmbMarital.DisplayMember = ds.Tables("MARITAL").Columns(1).ColumnName
            Me.CmbMarital.ValueMember = ds.Tables("MARITAL").Columns(0).ColumnName

            Me.CmbResident.DataSource = Addhead(ds.Tables("RESIDENT"), ds.Tables("RESIDENT").Columns(1).ColumnName, ds.Tables("RESIDENT").Columns(0).ColumnName)
            Me.CmbResident.DisplayMember = ds.Tables("RESIDENT").Columns(1).ColumnName
            Me.CmbResident.ValueMember = ds.Tables("RESIDENT").Columns(0).ColumnName


            '### SEBIN006 ----------- REMOVE THIS CODE -------------------
            'Dim dtSearch As DataTable = ds.Tables("relation_dtl")
            'dtSearch.DefaultView.RowFilter = "relation_id=1"
            'Me.cmbiden.DataSource = Addhead(dtSearch, ds.Tables("relation_dtl").Columns(1).ColumnName, ds.Tables("relation_dtl").Columns(0).ColumnName) 'Added for req 4829
            ''dtSearch.DefaultView.RowFilter = "relation_id"
            ''Me.cmbiden.DataSource = dtSearch
            'Me.cmbiden.DisplayMember = dtSearch.Columns(1).ColumnName
            'Me.cmbiden.ValueMember = dtSearch.Columns(0).ColumnName
            '------------------------------------------------------------------------------------------

            'Dim dtSearch As DataTable = dtCustomer

            'grvCustomer.DataSource = dtSearch

            Me.CmbPrefLang.DataSource = Addhead(ds.Tables("lang"), ds.Tables("lang").Columns(1).ColumnName, ds.Tables("lang").Columns(0).ColumnName)
            Me.CmbPrefLang.DisplayMember = ds.Tables("lang").Columns(1).ColumnName
            Me.CmbPrefLang.ValueMember = ds.Tables("lang").Columns(0).ColumnName

            tempPLoane = ds.Tables("loan_purpose").Copy
            'Me.cmbAPurposeofloan.DataSource = Addhead(ds.Tables("loan_purpose"), ds.Tables("loan_purpose").Columns(1).ColumnName, ds.Tables("loan_purpose").Columns(0).ColumnName) 'Added for req 4829
            'Me.cmbAPurposeofloan.DisplayMember = ds.Tables("loan_purpose").Columns(1).ColumnName
            'Me.cmbAPurposeofloan.ValueMember = ds.Tables("loan_purpose").Columns(0).ColumnName

            Dim Custds As New DataSet
            Dim custType As New DataTable("Customertype")
            custType.Columns.Add(New DataColumn("Type"))
            custType.Columns.Add(New DataColumn("Id"))
            custType.Rows.Add("Gold Loan", 1)
            custType.Rows.Add("NCD", 2)
            custType.Rows.Add("Forex", 3)
            custType.Rows.Add("Money Transfer", 4)
            custType.Rows.Add("Makash Wallet", 5)
            Custds.Tables.Add(custType)
            Me.cmbCustStatusnew.DataSource = Addhead(Custds.Tables("Customertype"), Custds.Tables("Customertype").Columns(0).ColumnName, Custds.Tables("Customertype").Columns(1).ColumnName) 'Added for req 4829
            Me.cmbCustStatusnew.DisplayMember = "Type"
            Me.cmbCustStatusnew.ValueMember = "Id"

        Catch ex As Exception 'added for req 4829
            Throw New ApplicationException("loadfill:- " & ex.Message)
        End Try
    End Sub

    Private Sub btnConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfirm.Click

        btngetkyc.Visible = False
        If (cmbCMedia.SelectedValue = 14 Or cmbCMedia.SelectedValue = 29) And code = "" Then

            MessageBox.Show("Please Enter BA/DSA Code or Select Other Media.")

        Else
            btnConfirm.Enabled = False
            Dim strGender As String = String.Empty
            Dim wise As New customerService.customer
            Dim dtCurentDate As Date = Format(wise.getDate(), "dd/MMM/yyyy")
            If dtCurentDate = SearchCustomer.ModuleOpen Then
                Try
                    Me.Cursor = Cursors.WaitCursor
                    checkControls()
                    If flag = 1 Then
                        Dim gender() As String
                        gender = Me.cmbPre.SelectedValue.ToString.Split("~")
                        If rb_MaleAdd.Checked = True Then
                            strGender = "1"
                        Else
                            strGender = "0"
                        End If
                        Dim confstr As String = ""
                        confstr = Me.txtACustName.Text.ToString + "/" + strGender
                        confstr += "¥" + Me.cmbiden.SelectedText + "" + Me.txtAFatHus.Text
                        confstr += "¥" + Me.txtHouse.Text
                        confstr += "¥" + Me.txtALocation.Text 'Me.txtStreet.Text
                        confstr += "¥" + Me.txtALocation.Text
                        confstr += "¥" + GetValue(Me.cmbCountry) 'Me.cmbCountry.SelectedValue.ToString
                        If Me.cmbCountry.SelectedValue <> 1 Then
                            confstr += "¥" + "0"
                            confstr += "¥" + "0"
                            confstr += "¥" + "0@0"
                            confstr += "¥" + "0"
                        Else
                            confstr += "¥" + GetValue(Me.cmbState) 'Me.cmbState.SelectedValue.ToString
                            confstr += "¥" + GetValue(Me.cmbDistrict) 'Me.cmbDistrict.SelectedValue.ToString
                            confstr += "¥" + GetValue(Me.cmbPost) 'Me.cmbPost.SelectedValue.ToString
                            confstr += "¥" + Me.txtPincode.Text
                        End If
                        confstr += "¥" + GetValue(Me.cmbCustStatus) 'Me.cmbCustStatus.SelectedValue.ToString
                        confstr += "¥" + GetValue(Me.cmbOccupation) 'Me.cmbOccupation.SelectedValue.ToString
                        confstr += "¥" + Me.txtPhcode.Text + Me.txtAPhoneno.Text
                        confstr += "¥" + Me.txtMobileNo.Text
                        confstr += "¥" + Me.txtEmail.Text
                        confstr += "¥" + GetValue(Me.cmbId) 'Me.cmbId.SelectedValue.ToString
                        confstr += "¥" + Me.txtAIdno.Text.ToString
                        confstr += "¥" + Me.DateTimePicker2.Text
                        confstr += "¥" + Me.DateTimePicker1.Text
                        confstr += "¥" + Me.txtAIssuePlace.Text
                        confstr += "¥" + txtdob.Text.ToString()
                        'Dim dates As String = Format(Convert.ToDateTime(txtdob.Text), "dd/MMM/yyyy").ToString()

                        'confstr += "¥" + dates
                        confstr += "¥" + strGender
                        confstr += "¥" + GetValue(Me.cmbCMediatype) 'Me.cmbCMediatype.SelectedValue.ToString
                        confstr += "¥" + GetValue(Me.cmbCMedia) 'Me.cmbCMedia.SelectedValue.ToString
                        confstr += "¥" + "0"
                        confstr += "¥" + GetValue(Me.cmbReligion) 'Me.cmbReligion.SelectedValue.ToString
                        confstr += "¥" + GetValue(Me.cmbCaste) 'Me.cmbCaste.SelectedValue.ToString
                        confstr += "¥" + Me.txtACardNo.Text
                        Dim share As String
                        share = "F"
                        confstr += "¥" + share.ToString
                        confstr += "¥" + Me.txtPanNo.Text
                        confstr += "¥" + GetValue(Me.cmb_landHldtl) 'CStr(Me.cmb_landHldtl.SelectedValue)
                        If Me.CheckBox1.Checked = True Then
                            confstr += "¥" + "1"
                            confstr += "¥" + CStr(Me.TextBox19.Text)
                        Else
                            confstr += "¥" + "0"
                            confstr += "¥" + ""
                        End If
                        confstr += "¥" + GetValue(Me.cmbAPurposeofloan)

                        If Not IsNothing(Address) AndAlso Address.Count > 0 Then
                            confstr += "¥" + Address.Item("housename")
                            confstr += "¥" + Address.Item("locality")
                            confstr += "¥" + Address.Item("Post")
                        Else
                            confstr += "¥" + ""
                            confstr += "¥" + ""
                            confstr += "¥" + "0"
                        End If

                        If rdoIndividual.Checked Then
                            confstr += "¥" + "1"
                        Else
                            confstr += "¥" + "2"
                        End If

                        If rbtMtKyc.Checked Then
                            confstr += "¥" + "1"
                        Else
                            confstr += "¥" + "0"
                        End If

                        'If Not IsNothing(Refdetails) AndAlso Refdetails.Rows.Count > 0 Then
                        'Else
                        '    MessageBox.Show("Add atleast one additional contact..!!")
                        '    Exit Sub
                        'End If

                        Dim uid As String
                        If Me.cmbCustStatus.SelectedValue = 4 Then
                            uid = User_id & "*" & Me.txtEmpCode.Text
                        Else
                            uid = User_id
                        End If
                        ' wchelper.SaveImage()
                        'cust = wchelper.get_data()
                        If cust Is Nothing Then
                            Me.picCustPhoto.Image = Nothing

                        Else
                            Me.picCustPhoto.Image = Me.picCustPhoto.Image
                        End If
                        '''''''''''''''''''''''''''''''''Start of Adding Customer Creation details to customer Photo
                        If cust IsNot Nothing Then
                            Try
                                Dim ms As MemoryStream = New MemoryStream(cust)
                                Dim NewBitmap As New System.Drawing.Bitmap(New Bitmap(ms))
                                ms.Close()
                                Dim sql As String = "select sysdate from dual"
                                Dim ws As New customerService.customer(set_ip)
                                Dim dt As New Data.DataTable
                                dt = ws.QueryResult(sql).Tables(0)
                                Dim saveval As String
                                NewBitmap = Overlay.ResizeImage(NewBitmap, 320, 240)
                                saveval = "Created By : " & UserId.ToString() & " on : " & dt.Rows(0)(0).ToString() & " at : " & BranchName.ToString()
                                NewBitmap = Overlay.VBOverlay(NewBitmap, saveval, New Font("Arial", 4, FontStyle.Regular), Color.Black, True, False, ContentAlignment.BottomLeft, 0.2!)

                                Dim file_size As Long
                                Dim desired_size As Long = 10000
                                For compression_level As Integer = 100 To 10 Step -1
                                    Using memory_stream As MemoryStream = Overlay.SaveJPGWithCompressionSetting(NewBitmap, compression_level)
                                        file_size = memory_stream.Length
                                        If file_size <= desired_size Then
                                            cust = memory_stream.ToArray()
                                            Exit For
                                        End If
                                    End Using
                                Next compression_level

                                'Me.picSearchdtl.Image = NewBitmap
                                'cust = ms1.GetBuffer()

                            Catch ex As Exception
                                MsgBox(ex.Message.ToString())
                            End Try
                        End If

                        ''''''''''''''''''''' End of Adding Customer Creation details to customer Photo  ''''''''''''''''''''''''''''''''''''''''

                        Dim obj As New customerService.customer(set_ip)
                        Dim exisitngCustomerDB As New DataSet

                        exisitngCustomerDB = obj.CustomerFilter("Group1", txtACustName.Text, txtMobileNo.Text, cmbiden.SelectedText & txtAFatHus.Text, txtHouse.Text, txtALocation.Text, txtPincode.Text, txtALocation.Text, txtdob.Text, txtAIdno.Text)
                        If exisitngCustomerDB.Tables.Count > 0 AndAlso exisitngCustomerDB.Tables(0).Rows.Count > 0 Then
                            MatchingCustomerList.AadhaaRid = txtAIdno.Text

                            Dim dcl As New MatchingCustomerList(exisitngCustomerDB.Tables(0))
                            If dcl.ShowDialog() = Windows.Forms.DialogResult.Ignore Then

                                confstr += "¥" + "2"
                                isactive = 2
                            ElseIf dcl.ShowDialog() = Windows.Forms.DialogResult.Cancel Then
                                MessageBox.Show("Customer creation cancelled.")
                                clear_confirm()
                                btnConfirm.Enabled = True
                                Exit Sub

                            End If
                        Else
                            If cmbiden.SelectedValue = 23 Or CmbResident.SelectedValue = 2 Or cmbCountry.SelectedValue <> 1 Then
                                confstr += "¥" + "8"
                                isactive = 8
                            Else
                                confstr += "¥" + "1"
                                isactive = 1
                            End If
                        End If
                        confstr += "¥" + GetValue(Me.cmbCustStatusnew)
                        'DSA_BA_Users(0).ToString()

                        If code <> "" Then

                            confstr += "¥" + code
                        Else

                            confstr += "¥" + "0"
                        End If

                        confstr += "¥" + CmbPrefLang.SelectedValue.ToString()

                        '_____________________________SACHIN__________________________________
                        'If kycPhoto Is Nothing Then
                        '    Dim result As Integer = MessageBox.Show("KYC documents not uploaded!!  Do you want to create Customer ID without KYC documents.??", "Confirm??", MessageBoxButtons.OKCancel)
                        '    If result = DialogResult.OK Then
                        '        RemFlag = 1
                        '        Dim KycRem As New FrmKycRem()
                        '        If KycRem.ShowDialog() = DialogResult.Yes Then
                        '        Else
                        '            Exit Sub
                        '        End If
                        '    ElseIf result = DialogResult.Cancel Then
                        '        Exit Sub
                        '    End If
                        'End If

                        If KycRemark = "" Then
                            KycRemark = "©©"
                        End If

                        confstr += "¥" + KycRemark

                        'If cust Is Nothing Then
                        '    Dim result As Integer = MessageBox.Show("Are you sure want to confirm without Customer Photo..??", "Confirm??", MessageBoxButtons.OKCancel)
                        '    If result = DialogResult.OK Then
                        '        RemFlag = 2
                        '        Dim KycRem As New FrmKycRem()
                        '        If KycRem.ShowDialog() = DialogResult.Yes Then
                        '        Else
                        '            Exit Sub
                        '        End If
                        '    ElseIf result = DialogResult.Cancel Then
                        '        Exit Sub
                        '    End If
                        'End If

                        If PhotoRemark = "" Then
                            PhotoRemark = "©©"
                        End If

                        confstr += "¥" + PhotoRemark
                        '-------------------------------- sebin ----------------------------------------------------------
                        confstr += "¥" + cmbpep.SelectedValue.ToString()
                        '-------------------------------------------------------------------------------------------------------

                        '  confstr += "¥" + TxtMom.Text

                        confstr += "¥" + ""
                        confstr += "¥" + "0"
                        confstr += "¥" + "0"
                        confstr += "¥" + CmbResident.SelectedValue.ToString()
                        confstr += "¥" + CmbMarital.SelectedValue.ToString()
                        confstr += "¥" + Me.cmbiden.SelectedValue.ToString()
                        confstr += "¥" + gender(0)

                        If Not IsNothing(Address) AndAlso Not IsNothing(Address.Item("Flg")) Then
                            confstr += "¥" + Address.Item("Flg").ToString()
                        Else
                            confstr += "¥" + "0"
                        End If
                        confstr += "¥" + "0"
                        confstr += "¥" + "0"
                        confstr += "¥" + "0"
                        confstr += "¥" + "0"

                        Dim result1 As Integer = MessageBox.Show("I have verified the originals of ID Proof and Address Proof provided by the customer;  the ‘KYC Number’, ‘Customer Name’, ‘Date of Birth’, etc entered are as per the loan application form and are cross verified with the proofs provided; and the  ‘KYC Type’ selected is correct.", "Declaration", MessageBoxButtons.YesNo)
                        If result1 = DialogResult.No Then
                            btnConfirm.Enabled = True
                            Exit Sub
                        End If
                        ' _ _ _ _ _ _ _ __ ____ ___ __ __ __ ___ __ ___ __ ___ __ ___ ___ ___ __ __ _ __ __ _ __ __ __ __ __ __ __ _ __ __ __ ___ ___ _ ____ __ _ ______________()


                        Dim intCustType As Integer = cmbCustStatusnew.SelectedIndex
                        If confstr <> "" Then
                            If servType = "BIO" Then
                                servType = "2"
                            ElseIf servType = "OTP" Then
                                servType = "3"
                            End If
                            Dim result As String = obj.add_customer(confstr, FirmID, BranchID, uid, neftdetails, rrn_n, UUID, servType)
                            Dim resultArr() As String = result.Split("+")
                            If cmbOccupation.SelectedIndex = 40 Then
                                Dim res As String = ""
                                If Not resultArr(2) = "" Then
                                    res = obj.Vehicle_Finance_Survey(resultArr(2), BranchID, VehicleFinance.CommVehOwnership, VehicleFinance.CommOwnershipVehType, VehicleFinance.TruckReq, VehicleFinance.ReqCommVeh, VehicleFinance.ReqCommVehicleType, VehicleFinance.ReqOtherVeh, VehicleFinance.ReqOtherVehDetail)
                                End If
                            End If
                            Dim custlen As Integer
                            Dim retval As String
                            Dim retval2 As String
                            If resultArr(1) = "1" Then
                                custlen = Len(resultArr(2))
                                If custlen <= 14 Then
                                    If cust Is Nothing And neftPhoto Is Nothing And kycPhoto Is Nothing Then
                                    Else
                                        If Not IsNothing(kycPhoto) Then
                                            retval = obj.custinsrt(resultArr(2), kycPhoto)
                                        End If
                                        result = obj.NewcustomeraddPhoto(resultArr(2), cust, Nothing, Nothing, neftPhoto) ' neftid param added
                                        'If chkAddress.Checked AndAlso cmbAddressProf.Visible = True Then
                                        '    obj.UpdateKYCInfo("UPDADDPROOFID", resultArr(2), Convert.ToString(cmbAddressProf.SelectedValue), "", User_id)
                                        'ElseIf chkAddress.Checked AndAlso chkSameAsId.Checked Then
                                        '    obj.UpdateKYCInfo("UPDADDPROOFID", resultArr(2), "0", "", User_id)
                                        'Else
                                        '    obj.UpdateKYCInfo("UPDADDPROOFID", resultArr(2), "", "", User_id)
                                        'End If

                                        End If
                                        MsgBox(resultArr(0))
                                        kycPhoto = Nothing
                                        cust = Nothing
                                        Me.txtCustId.Text = resultArr(2)
                                        txt_pan_custname.Text = txtACustName.Text
                                        obj.Matching_ExtCust_KycVerify(resultArr(2), UUID, rrn_n)

                                        If Me.txtCustId.Text.Trim().Length = 14 Then
                                            SaveCustomerMobOTPDetails(Me.txtCustId.Text, MobileOTP.strMobileNumOTP, MobileOTP.strOTPValue)
                                            SaveCustomerRefDetails(Me.txtCustId.Text)
                                            '****************************** Welcome Call Start****************************
                                            ' '' '' '' '' '' '' ''Dim url As String = "http://api.dialstreet.com/v1/?api_key=Adc367ae31c20af9d5cdeaf7e291767a6&method=dial.click2call&output=xml&caller=" & txtMobileNo.Text & "&receiver=ivr:6323&delay=1800"
                                            ' '' '' '' '' '' '' ''Dim wrGETURL As WebRequest
                                            ' '' '' '' '' '' '' ''wrGETURL = WebRequest.Create(url)
                                            ' '' '' '' '' '' '' ''Dim objStream As Stream
                                            ' '' '' '' '' '' '' ''objStream = wrGETURL.GetResponse.GetResponseStream()
                                            'Dim objReader As New StreamReader(objStream)
                                            'Dim sLine As String = ""
                                            'Dim i As Integer = 0

                                            'Do While Not sLine Is Nothing
                                            '    i += 1
                                            '    sLine = objReader.ReadLine
                                            '    If Not sLine Is Nothing Then
                                            '        ' Console.WriteLine("{0}:{1}", i, sLine)
                                            '    End If
                                            'Loop
                                            '****************************** Welcome Call End****************************
                                            If intCustType = 1 Then
                                                Customer_CheckforEmpRef()
                                            End If
                                        End If
                                        'customer insert in ekyc module code dwtails
                                        CuName = txtACustName.Text
                                        FaName = txtAFatHus.Text
                                        clear_confirm()
                                        confstr = ""
                                        'MsgBox(isactive)
                                        If isactive = 8 Then
                                            ' MsgBox(isactive)
                                            CkycFlag = 1
                                            Me.Main_tab.SelectedIndex = 3
                                            'Me.Main_tab.SelectedTab = Me.TabCustDtl
                                        End If
                                        'Dim TransId As Integer
                                        'Dim transdt As New DataTable
                                        'If share = "T" Then
                                        '    transdt = obj.QueryResult("select count(*) from transaction_detail where ref_id='" & resultArr(2) & "' and firm_id=2 and branch_id=" & BranchID & "").Tables(0)
                                        '    If transdt.Rows(0)(0) > 0 Then
                                        '        transdt = obj.QueryResult("select distinct trans_id from transaction_detail where ref_id='" & resultArr(2) & "' and firm_id=2 and branch_id=" & BranchID & "").Tables(0)
                                        '        TransId = transdt.Rows(0)(0)
                                        '        new_vcc(TransId, 2, BranchID)
                                        '    End If
                                        'End If
                                Else

                                        MsgBox(resultArr(0))
                                        txt_pan_custname.Text = txtACustName.Text
                                        clear_confirm()
                                        confstr = ""
                                End If

                            Else
                                MsgBox(resultArr(0))
                            End If
                        Else
                            MsgBox("Enter Data .............")
                            btnConfirm.Enabled = True
                            Exit Sub
                        End If
                    Else
                        MsgBox("Enter All Data Carefully..............")
                        btnConfirm.Enabled = True
                        Exit Sub
                    End If
                    If Not IsNothing(Address) AndAlso Address.Count > 0 Then
                        SearchCustomer.Address.Clear()
                    End If
                Catch ex As Exception 'added for req 4829
                    MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Finally
                    Me.Cursor = Cursors.Default
                    kycStatus = 0
                End Try
                btnConfirm.Enabled = True
            Else
                MsgBox("Latest Version available....Please Download http://app.manappuram.net/CustomerNew/publish.htm")
                Exit Sub
            End If
        End If
    End Sub
    Sub Customer_CheckforEmpRef()
        Dim objService As New customerService.customer(set_ip)
        Dim strResult As String = String.Empty
        If MobileOTP.strMobileNumOTP.Length > 0 AndAlso txtCustId.Text.Length > 0 Then
            strResult = objService.CheckForEmpReference(MobileOTP.strMobileNumOTP, txtCustId.Text)
        End If
        If strResult.Length > 0 Then
            MsgBox("This Customer has been referred by Employee:-" & strResult)
        End If
    End Sub
    Sub clear_confirm()
        Try
            Me.txtACardNo.Text = ""
            Me.txtACustName.Text = ""
            Me.rb_FemaleAdd.Checked = False
            Me.rb_MaleAdd.Checked = False
            Me.txtHouse.Text = ""
            Me.txtAFatHus.Text = ""
            Me.txtAIdno.Text = ""
            Me.txtAIssuePlace.Text = ""
            Me.txtALocation.Text = ""
            Me.txtAPhoneno.Text = ""
            Me.txtEmail.Text = ""
            Me.txtPhcode.Text = ""
            Me.txtMobileNo.Text = ""
            Me.picCustPhoto.Image = Nothing
            Me.txtEmpCode.Text = ""
            Me.txtEmpName.Text = ""
            Me.txtEmpCode.Visible = False
            Me.txtEmpName.Visible = False
            Me.Label75.Visible = False

            TextBox19.Text = ""
            CheckBox1.Checked = False
            rdoIndividual.Checked = True
            rdoRegularKYC.Checked = True
            loadfill()
            code = ""
            KycRemark = ""
            PhotoRemark = ""

            'DateTimePicker3.ResetText()
            DateTimePicker2.ResetText()
            DateTimePicker1.ResetText()

            rrn_n = ""
            UUID = ""

            btngetkyc.Visible = False
            btnAddKyc.Visible = False


        Catch ex As Exception 'added for req 4829
            Throw New ApplicationException("clear_confirm:- " & ex.Message)
        End Try
    End Sub
    Public Sub new_vcc(ByVal trid As Integer, ByVal fmid As Integer, ByVal branch_id As Integer)
        Dim obj As New customerService.customer(set_ip)
        Try
            Me.Cursor = Cursors.WaitCursor
            Dim df As New DataTable
            Dim dg As New DataTable
            df = obj.normal_vouch(trid, fmid, branch_id).Tables(0)
            dg = obj.cash_vouch(trid, fmid, branch_id).Tables(0)
            'Dim sd() As String
            'sd = dg.Split("*")
            Dim nars As String = "BEING SHARE APPLN MONEY RCVD FROM  " & Me.txtCustId.Text 'sd(0).Split(" ^ ")"
            Dim rpt_obj As New nm_vouch
            rpt_obj.Database.Tables("vouch_tab").SetDataSource(df)
            rpt_obj.SetParameterValue("cash_id", dg.Rows(0)(1))
            rpt_obj.SetParameterValue("brdt", obj.getDate())
            rpt_obj.SetParameterValue("id_dtl", nars)
            rpt_obj.PrintOptions.PrinterName = "\\10.5.2.125\pledge" 'login.vch_print_name
            rpt_obj.PrintOptions.PaperSource = CrystalDecisions.Shared.PaperSource.Lower
            rpt_obj.PrintToPrinter(1, True, 1, 1)
            MsgBox("Voucher Printing is over..", MsgBoxStyle.Information, "Thanks")
            'Me.cmd_ok.Enabled = True
        Catch ex As Exception
            MsgBox("Error in Printing ..", MsgBoxStyle.Critical, "Error")
            MsgBox(ex.ToString)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Sub combofill(ByVal query As String, ByVal control As Windows.Forms.ComboBox)
        Try
            Dim obj As New customerService.customer(set_ip)
            Dim maindt As New DataTable
            maindt = obj.QueryResult(query).Tables(0)
            control.DataSource = Addhead(maindt, maindt.Columns(1).ColumnName, maindt.Columns(0).ColumnName) 'added for req 4829
            control.DisplayMember = maindt.Columns(1).ColumnName
            control.ValueMember = maindt.Columns(0).ColumnName
        Catch ex As Exception 'added for req 4829
            Throw New ApplicationException("combofill:- " & ex.Message)
        End Try
    End Sub

    Sub statefill(ByVal countryid As Integer)
        Try
            sql = "select state_id,state_name from state_master where country_id=" & countryid & ""
            combofill(sql, cmbState)
            If Me.cmbState.Items.Count > 0 Then
                districtfill(Me.cmbState.SelectedValue)
            End If
        Catch ex As Exception 'added for req 4829
            Throw New ApplicationException("statefill:- " & ex.Message)
        End Try
    End Sub

    Sub districtfill(ByVal stateid As Integer)
        Try
            sql = "select district_id,district_name from district_master where state_id=" & stateid & ""
            combofill(sql, cmbDistrict)
            If Me.cmbDistrict.Items.Count > 0 Then
                Postfill(Me.cmbDistrict.SelectedValue)
            End If
        Catch ex As Exception 'added for req 4829
            Throw New ApplicationException("districtfill:- " & ex.Message)
        End Try
    End Sub

    Sub Postfill(ByVal districtid As Integer)
        Try
            sql = "select pin_code ||'@'||sr_number as pincode,post_office from post_master where district_id=" & districtid & " and status_id=1"
            combofill(sql, cmbPost)
            If Me.cmbPost.Items.Count > 0 Then
                Pinfill(Me.cmbPost.SelectedValue)
            End If
        Catch ex As Exception 'added for req 4829
            Throw New ApplicationException("Postfill:- " & ex.Message)
        End Try
    End Sub

    Sub Pinfill(ByVal Postid As String)
        Dim pincode() As String
        pincode = Postid.ToString.Split("@")
        Me.txtPincode.Text = pincode(0)
    End Sub

    Sub Mediafill(ByVal typeid As Integer)
        Try
            sql = "select media_id,media_name from media_master_new where type_id=" & typeid
            combofill(sql, cmbCMedia)
        Catch ex As Exception 'added for req 4829
            Throw New ApplicationException("Mediafill:- " & ex.Message)
        End Try
    End Sub

    Sub castefill(ByVal religioid As Integer)
        Try
            sql = "select caste_id,caste_name from caste_master where religion_id=" & religioid & ""
            combofill(sql, cmbCaste)
        Catch ex As Exception 'added for req 4829
            Throw New ApplicationException("castefill:- " & ex.Message)
        End Try
    End Sub

    Public Function checkForNumeric(ByRef KeyCode As String, ByRef Control As TextBox) As Boolean
        ' MsgBox(KeyCode)

        If ((KeyCode > 95 AndAlso KeyCode < 106) OrElse (KeyCode > 47 AndAlso KeyCode < 58) OrElse (KeyCode = 8)) Then
            Control.Text = Control.Text
        Else
            Control.Text = ""
        End If
    End Function

    Public Function checkForNumerical(ByRef KeyCode As String, ByRef Control As TextBox) As Boolean
        ' MsgBox(KeyCode)

        If ((KeyCode > 95 AndAlso KeyCode < 106) OrElse (KeyCode > 47 AndAlso KeyCode < 58) OrElse (KeyCode = 8)) Then
            Control.Text = Control.Text
        Else
            Control.Text = "0"
        End If
    End Function

    Public Function checkForString(ByRef KeyCode As String, ByRef Control As TextBox) As Boolean
        If (KeyCode > 64 AndAlso KeyCode < 91) OrElse (KeyCode > 105 AndAlso KeyCode < 123) OrElse KeyCode = 190 OrElse KeyCode = 32 OrElse (KeyCode = 8) Then
            Control.Text = Control.Text
        Else
            Control.Text = ""
        End If

    End Function

    Private Sub txtACustName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtACustName.KeyPress
        If Me.cmbPre.SelectedValue Is Nothing Or Me.cmbPre.SelectedValue = "-1~0" Then
            MsgBox("Please Select the title(MR,MRS,MISS")
            Me.cmbPre.Focus()
            flag = 0
            txtACustName.Text = ""
            Exit Sub
        End If


        e.KeyChar = CChar(e.KeyChar.ToString.ToUpper)

        If Not (Asc(e.KeyChar) = 8) Then
            If Not e.KeyChar = " " Then
                Dim allowedChars As String = "abcdefghijklmnopqrstuvwxyz"
                If Not allowedChars.Contains(e.KeyChar.ToString.ToLower) Then
                    e.KeyChar = ChrW(0)
                    e.Handled = True
                End If
            End If
        End If
    End Sub
    Private Sub txtAFatHus_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAFatHus.KeyPress
        If Me.cmbiden.SelectedValue Is Nothing Or Me.cmbiden.SelectedValue = 0 Then
            MsgBox("Please Select the title (S/O,D/O,W/O)")
            Me.cmbiden.Focus()
            flag = 0
            txtAFatHus.Text = ""
            Exit Sub
        End If

        e.KeyChar = CChar(e.KeyChar.ToString.ToUpper)
        If Not (Asc(e.KeyChar) = 8) Then
            If Not e.KeyChar = " " Then
                Dim allowedChars As String = "abcdefghijklmnopqrstuvwxyz"
                If Not allowedChars.Contains(e.KeyChar.ToString.ToLower) Then
                    e.KeyChar = ChrW(0)
                    e.Handled = True
                End If
            End If
        End If
    End Sub

    Public Function checkForNumberandCharacter(ByRef KeyCode As String, ByRef Control As TextBox) As Boolean
        If (KeyCode > 64 AndAlso KeyCode < 91) OrElse (KeyCode > 105 AndAlso KeyCode < 123) OrElse KeyCode = 190 OrElse KeyCode = 32 OrElse (KeyCode = 8) OrElse ((KeyCode > 95 AndAlso KeyCode < 106) OrElse (KeyCode > 47 AndAlso KeyCode < 58)) Then
            Control.Text = Control.Text
        Else
            Control.Text = ""
        End If
    End Function

    Private Sub txtACustName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtACustName.KeyUp
        'checkForString(e.KeyCode, Me.txtACustName)
    End Sub

    Private Sub txtAFatHus_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtAFatHus.KeyUp
        checkForString(e.KeyCode, Me.txtAFatHus)
    End Sub

    Private Sub txtAIdno_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtAIdno.KeyUp
        'checkForNumberandCharacter(e.KeyCode, Me.txtAIdno)

        'If e.KeyCode = Keys.Enter Then
        '    'Do something
        '    Dim EYC As New EKYC()
        '    EYC.ShowDialog()
        'ElseIf e.KeyCode = Keys.Tab Then
        '    'Do something
        '    Dim EYC As New EKYC()
        '    EYC.ShowDialog()
        'End If


    End Sub

    Private Sub txtAIssuePlace_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtAIssuePlace.KeyUp
        checkForString(e.KeyCode, Me.txtAIssuePlace)
    End Sub

    Private Sub txtALocation_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtALocation.KeyUp
        'checkForString(e.KeyCode, Me.txtALocation)
    End Sub

    Private Sub txtAPhoneno_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtAPhoneno.KeyUp
        checkForNumeric(e.KeyCode, Me.txtAPhoneno)
        If txtPhcode.Text.Length = 3 Then
            txtAPhoneno.MaxLength = 8
        ElseIf txtPhcode.Text.Length = 4 Then
            txtAPhoneno.MaxLength = 7
        End If
        If txtAPhoneno.Text.Length = 8 Then
            txtPhcode.MaxLength = 3
        ElseIf txtAPhoneno.Text.Length < 8 Then
            txtPhcode.MaxLength = 4
        End If


    End Sub

    'Private Sub txtAIssuePlace_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAIssuePlace.GotFocus
    '    If DateDiff(DateInterval.Day, CDate(Me.DateTimePicker2.Text), CDate(Me.DateTimePicker1.Text)) < 1 Then
    '        Me.DateTimePicker1.Text = Date.Now
    '        Me.DateTimePicker1.Focus()
    '        MsgBox("Please enter the correct Id Expiry Date:")
    '        Exit Sub
    '    End If
    '    If DateDiff(DateInterval.Day, CDate(Me.DateTimePicker1.Text), Date.Now) > 0 Then
    '        Me.DateTimePicker1.Text = Date.Now
    '        MsgBox("Please enter the correct Id Expiry Date:")
    '        Me.DateTimePicker1.Focus()
    '        Exit Sub
    '    End If
    'End Sub

    'Private Sub txtMobileNo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMobileNo.KeyUp
    '    checkForNumeric(e.KeyCode, Me.txtMobileNo)
    '    checkMobileno(e.KeyCode, Me.txtMobileNo)
    'End Sub

    Private Sub txtPhcode_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPhcode.KeyUp
        checkForNumeric(e.KeyCode, Me.txtPhcode)
        If cmbCountry.SelectedValue = "1" Then
            checkSTDCode(e.KeyCode, Me.txtPhcode)
        ElseIf cmbCountry.SelectedValue = "0" Then
            Me.txtPhcode.Text = String.Empty
        End If
    End Sub
    Private Sub txtMPhoneno_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMPhoneno.KeyUp
        checkForNumeric(e.KeyCode, Me.txtPhcode)
        'checkSTDCode(e.KeyCode, Me.txtMPhoneno)
    End Sub

    'Private Sub txtStreet_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    '    e.KeyChar = CChar(e.KeyChar.ToString.ToUpper)
    'End Sub

    'Private Sub txtStreet_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    '    checkForString(e.KeyCode, Me.txtStreet)
    'End Sub

    'Private Sub txtACustName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtACustName.KeyPress
    '    e.KeyChar = CChar(e.KeyChar.ToString.ToUpper)
    'End Sub

    'Private Sub txtAFatHus_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAFatHus.KeyPress
    '    e.KeyChar = CChar(e.KeyChar.ToString.ToUpper)
    'End Sub

    Private Sub txtAIdno_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAIdno.KeyPress
        e.KeyChar = CChar(e.KeyChar.ToString.ToUpper)
        If Not (Asc(e.KeyChar) = 8) Then

            Dim allowedChars As String = "abcdefghijklmnopqrstuvwxyz0123456789/-"
            If Not allowedChars.Contains(e.KeyChar.ToString.ToLower) Then
                e.KeyChar = ChrW(0)
                e.Handled = True

            End If
        End If
    End Sub

    Private Sub txtAIssuePlace_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAIssuePlace.KeyPress
        e.KeyChar = CChar(e.KeyChar.ToString.ToUpper)
    End Sub

    Private Sub txtALocation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtALocation.KeyPress
        e.KeyChar = CChar(e.KeyChar.ToString.ToUpper)
        If Not (Asc(e.KeyChar) = 8) Then
            If Not e.KeyChar = " " Then
                Dim allowedChars As String = "abcdefghijklmnopqrstuvwxyz0123456789/-"
                If Not allowedChars.Contains(e.KeyChar.ToString.ToLower) Then
                    e.KeyChar = ChrW(0)
                    e.Handled = True
                End If
            End If
        End If
    End Sub

    Private Sub txtAPhoneno_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAPhoneno.KeyPress
        e.KeyChar = CChar(e.KeyChar.ToString.ToUpper)
        If Not (Asc(e.KeyChar) = 8) Then

            Dim allowedChars As String = "0123456789"
            If Not allowedChars.Contains(e.KeyChar.ToString.ToLower) Then
                e.KeyChar = ChrW(0)
                e.Handled = True
            End If


        End If

    End Sub

    Private Sub txtHouse_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtHouse.KeyPress
        e.KeyChar = CChar(e.KeyChar.ToString.ToUpper)
        If Not (Asc(e.KeyChar) = 8) Then
            If Not e.KeyChar = " " Then
                Dim allowedChars As String = "abcdefghijklmnopqrstuvwxyz0123456789/-"
                If Not allowedChars.Contains(e.KeyChar.ToString.ToLower) Then
                    e.KeyChar = ChrW(0)
                    e.Handled = True
                End If
            End If

        End If
    End Sub

    Private Sub txtHouse_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtHouse.KeyUp
        'checkForString(e.KeyCode, Me.txtHouse)
    End Sub

    Sub checkControls()
        Try

            Dim obj As New customerService.customer(set_ip)
            flag = 1
            'wchelper.SaveImage()
            ' cust = wchelper.get_data()
            cust = Nothing
            Try
                cust = wchelper.SaveImage()
            Catch ex As Exception
                cust = Nothing
                MsgBox(ex.Message.ToString())
            End Try
            If cust Is Nothing Then
                MsgBox("Webcam Not Connected..!!", MsgBoxStyle.OkOnly, "Alert..!!")
            End If

            'If kycPhoto Is Nothing Then
            '    MsgBox("Please scan consent..!!")
            '    flag = 0
            '    Exit Sub
            'End If

            '###------ SEBIN001 -----------------------------------------------------------
            If Me.cmbPre.SelectedValue Is Nothing Or Me.cmbPre.SelectedValue = "-1~0" Then
                MsgBox("Please Select the title(MR,MRS,MISS)")
                Me.cmbPre.Focus()
                flag = 0
                Exit Sub
                '------------------------------------------------------------------------------
            ElseIf RTrim(LTrim(Me.txtACustName.Text)) = "" Then
                MsgBox("Please Enter the Customer Name")
                Me.txtACustName.Focus()
                flag = 0
                Exit Sub

            ElseIf IsNothing(Address) Or Address Is Nothing Then
                MsgBox("Please Enter the Communication Address ")
                Me.txtACustName.Focus()
                flag = 0
                Exit Sub



            ElseIf Me.rb_MaleAdd.Checked = False AndAlso Me.rb_FemaleAdd.Checked = False Then
                MsgBox("Please Select Customer Gender")
                flag = 0
                Exit Sub

            ElseIf Me.cmbiden.SelectedValue Is Nothing Or Me.cmbiden.SelectedValue = 0 Then
                MsgBox("Please Select the title (S/O,D/O,W/O)")
                Me.cmbiden.Focus()
                flag = 0
                Exit Sub
            ElseIf Me.txtAFatHus.Text = "" Then
                MsgBox("Please Enter the Fathers Name")
                Me.txtAFatHus.Focus()
                flag = 0
                Exit Sub
            ElseIf cust Is Nothing Then
                If MessageBox.Show("Photo Catpturing can be skipped only for entering manual Pledge Customer.Click ok to take Photo or Cancel to confirm 'For Manual Pledge Entry'", "Photo Info", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = DialogResult.OK Then
                    'MsgBox("Please click on AddPhoto button to add customer photo")
                    Me.btnAddPhoto.Focus()
                    flag = 0
                    Exit Sub
                Else
                    If Not obj.isBackDtPldge(Me.BranchID) Then
                        MsgBox("No Approved BackDate Pledges are available to Use this Option")
                        Me.btnAddPhoto.Focus()
                        flag = 0
                        Exit Sub
                    End If
                End If
            ElseIf Me.txtHouse.Text = "" Then
                MsgBox("Please Enter the address")
                Me.txtHouse.Focus()
                flag = 0
                Exit Sub
                'ElseIf Me.txtStreet.Text = "" Then
                '    MsgBox("Please Enter the Street Name")
                '    Me.txtStreet.Focus()
                '    flag = 0
                '    Exit Sub
            ElseIf Me.txtALocation.Text = "" Then
                MsgBox("Please Enter the Locality")
                Me.txtALocation.Focus()
                flag = 0
                Exit Sub
                '###SEBIN002-----------------------------------------------------------------------
            ElseIf Me.cmbCountry.SelectedValue Is Nothing Or Me.cmbCountry.SelectedValue = 0 Then
                MsgBox("Please Select the Country")
                Me.cmbCountry.Focus()
                flag = 0
                Exit Sub
            ElseIf Me.txtPincode.Text = "" Then
                MsgBox("Please Select the post")
                Me.txtPincode.Focus()
                flag = 0
                Exit Sub
            ElseIf Me.cmbCustStatus.SelectedValue = "4" AndAlso (Me.txtEmpCode.Text.Trim = "") Then
                'If (Me.txtEmpCode.Text = "") Then
                MsgBox("Please enter the empcode")
                Me.txtEmpCode.Focus()
                flag = 0
                Exit Sub
            ElseIf Me.CmbResident.SelectedValue <= 0 Then
                MsgBox("Please Select Residential Status")
                Me.CmbResident.Focus()
                flag = 0
                Exit Sub
            ElseIf Me.CmbMarital.SelectedValue = 0 Then
                MsgBox("Please Select Marital Status")
                Me.CmbMarital.Focus()
                flag = 0
                Exit Sub
            ElseIf txtEmail.Text.Trim <> "" Then
                Dim rex As Match = Regex.Match(Trim(txtEmail.Text), "^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,3})$", RegexOptions.IgnoreCase)
                If rex.Success = False Then
                    MessageBox.Show("Please Enter a valid Email-Address ")
                    txtEmail.Focus()
                    flag = 0
                    Exit Sub


                End If
                'ElseIf txtMobileNo.Text <> "" And txtMobileNo.Text IsNot Nothing Then
                '    Dim cnt As Integer = obj.GetOnlineCustomerMobile(txtMobileNo.Text).Tables(0).Rows(0)(0)
                '    If cnt > 0 Then
                '        MessageBox.Show("OGL Customer With Same Mobile Number Already Exist..!!", "Warning", MessageBoxButtons.OK)
                '        txtMobileNo.Text = ""
                '        'txtMobileNo.Text = ""
                '        txtMobileNo.Focus()
                '        flag = 0
                '        Exit Sub
                '    End If

                'Else
                '    flag = 1
                'End If    
                'ElseIf Me.cmbCMediatype.SelectedValue = 2 AndAlso Me.cmbCMedia.SelectedValue = 8 AndAlso (Me.txtEmpCode.Text.Trim = "") Then
                '    'If (Me.txtEmpCode.Text = "") Then
                '    MsgBox("Please enter the empcode")
                '    Me.txtEmpCode.Focus()
                '    flag = 0
                '    Exit Sub
                '    'Else
                '    '    flag = 1
                '    'End If 
                ' ###SEBIN -------------------------------------------------------------------------------------
                'ElseIf Me.txtAPhoneno.Text = "" Then
                '    MsgBox("Please enter the phone number")
                '    Me.txtAPhoneno.Focus()
                '    flag = 0
                '    Exit Sub
                ' ------------------------------------------------------------------------------------------
            ElseIf Me.txtMobileNo.Text = "" Or Me.txtMobileNo.Text.Length <> 10 Then
                MsgBox("Please enter valid Mobile number")
                Me.txtMobileNo.Focus()
                flag = 0
                Exit Sub
                'ElseIf Me.txtAPhoneno.Text.Length > 0 Then
                '    If Me.txtPhcode.Text = "" OrElse Me.txtPhcode.Text = "0" Then
                '        MsgBox("Please enter the phonecode")
                '        Me.txtPhcode.Focus()
                '        flag = 0
                '        Exit Sub
                '    End If
                '###SEBIN --------------------------------------------------------    
            ElseIf Me.cmbCustStatus.SelectedIndex <= 0 Then
                MsgBox("Please select the customer status")
                flag = 0
                Exit Sub
                '----------------------------------------------------------------
            ElseIf Me.cmbId.SelectedIndex <= 0 Then
                MsgBox("Please select the id type")
                flag = 0
                Exit Sub

                'ElseIf Me.DateTimePicker2.Text = "" Then
                '    MsgBox("Please Enter the Issue Date")
                '    Me.DateTimePicker2.Focus()
                '    flag = 0
                '    Exit Sub
                'ElseIf CDate(Me.txtdob.Text) = "" Then
                '    MsgBox("Please enter the Date of birth")
                '    txtdob.Focus()
                '    flag = 0
                '    Exit Sub
                'ElseIf DateDiff(DateInterval.Day, Date.Now, CDate(Me.txtdob.Text)) >= 0 Then
                '    MsgBox("Please enter the Date of birth")
                '    Me.txtdob.Focus()
                '    Me.txtdob.Text = ""
                '    flag = 0
                '    Exit Sub
                'ElseIf DateDiff(DateInterval.Day, Date.Now, CDate(Me.DateTimePicker2.Text)) > 0 Then
                '    MsgBox("Please enter the correct Date of Issue:")
                '    Me.DateTimePicker2.Text = ""
                '    Me.DateTimePicker2.Focus()
                '    flag = 0
                '    Exit Sub
                'ElseIf DateDiff(DateInterval.Day, CDate(Me.DateTimePicker2.Text), CDate(Me.DateTimePicker1.Text)) < 1 Then
                '    MsgBox("Please enter the correct Id Expiry Date:")
                '    Me.DateTimePicker1.Text = ""
                '    Me.DateTimePicker1.Focus()
                '    flag = 0
                '    Exit Sub
            ElseIf Me.txtACardNo.Text <> "" Then
                'If Me.DateTimePicker2.Text = "" Then
                '    MsgBox("Please Enter the Issue Date")
                '    Me.DateTimePicker2.Focus()
                '    flag = 0
                '    Exit Sub
                'End If
                'If DateDiff(DateInterval.Day, CDate(Me.DateTimePicker2.Text), CDate(Me.DateTimePicker1.Text)) < 1 Then
                '    MsgBox("Please enter the correct  Expiry Date:")
                '    Me.DateTimePicker1.Text = ""
                '    Me.DateTimePicker1.Focus()
                '    flag = 0
                '    Exit Sub
                'End If
                '###SEBIN ------------------------------------------------------------
                'If Me.txtAIssuePlace.Text = "" Then
                '    MsgBox("Please enter Issue Place:")
                '    Me.txtAIssuePlace.Focus()
                '    flag = 0
                '    Exit Sub
                'End If
                '----------------------------------------------------
                If Me.txtMobileNo.Text = "" Then
                    MsgBox("Please enter the Mobile number")
                    Me.txtMobileNo.Focus()
                    flag = 0
                    Exit Sub
                End If
                If Me.cmbId.SelectedIndex <= 0 Then
                    MsgBox("Please select the id type")
                    flag = 0
                    Exit Sub
                End If
                '=========
                'Dim obj As New customerService.customer(set_ip)
                Dim st As Integer
                st = obj.cardcheck(Me.txtACardNo.Text, BranchID)
                If st = 0 Then
                    MsgBox("Card Already issued")
                    Me.txtACardNo.Text = ""
                    flag = 0
                    Exit Sub
                    'ElseIf st = 1 Then
                    '    flag = 1
                ElseIf st = 2 Then
                    MsgBox("Check Cardno")
                    Me.txtACardNo.Text = ""
                    flag = 0
                    Exit Sub
                End If
            ElseIf cmbCMediatype.Items.Count > 0 AndAlso cmbCMediatype.SelectedIndex = 0 Then
                MsgBox("Please select the MediaType")
                cmbCMediatype.Focus()
                flag = 0
                Exit Sub
            ElseIf cmbCMediatype.Items.Count > 0 AndAlso cmbCMediatype.SelectedIndex > 0 AndAlso cmbCMedia.SelectedIndex = 0 Then
                MsgBox("Please select the media")
                flag = 0
                Exit Sub
            ElseIf cmbOccupation.Items.Count > 0 AndAlso cmbOccupation.SelectedIndex = 0 Then
                MsgBox("Please select the business")
                cmbOccupation.Focus()
                flag = 0
                Exit Sub
            ElseIf (cmbCustStatusnew.SelectedIndex = 0 Or cmbCustStatusnew.SelectedValue Is Nothing) Then
                MsgBox("Please select the Customer type")
                cmbCustStatusnew.Focus()
                flag = 0
                Exit Sub
            ElseIf Me.cmbpep.SelectedValue Is Nothing Or Me.cmbpep.SelectedValue = "-1" Then
                MsgBox("Please Select the PEP")
                Me.cmbCountry.Focus()
                flag = 0
                Exit Sub





            ElseIf (((GetCurrentAge(txtdob.Text) < 18) Or (GetCurrentAge(txtdob.Text) > 90)) And ((cmbCustStatusnew.SelectedIndex = 1) Or (cmbCustStatusnew.SelectedIndex = 3) Or (cmbCustStatusnew.SelectedIndex = 4))) Then
                If (GetCurrentAge(txtdob.Text) > 90) Then
                    Dim result = MessageBox.Show("Customer Age is above 90. Do you want to continue", "Confirm", MessageBoxButtons.YesNo)
                    If result = DialogResult.No Then
                        txtdob.Focus()
                        flag = 0
                        Exit Sub
                    End If
                Else
                    flag = 0
                    MsgBox("Customer Age cannot be below 18 Years for Goldloan/Forex/Money Transfer Customer")
                End If
                txtdob.Focus()
                Exit Sub
            ElseIf ((cmbCustStatusnew.SelectedIndex = 2) And (GetCurrentAge(txtdob.Text) < 18)) Then
                Dim result = MessageBox.Show("Customer is a Minor. Do you want to continue", "Confirm", MessageBoxButtons.YesNo)
                If result = DialogResult.No Then
                    txtdob.Focus()
                    flag = 0
                    Exit Sub
                End If
            ElseIf ((GetCurrentAge(txtdob.Text) > 90) And (cmbCustStatusnew.SelectedIndex = 2)) Then
                Dim result = MessageBox.Show("Customer Age is above 90. Do you want to continue", "Confirm", MessageBoxButtons.YesNo)
                If result = DialogResult.No Then
                    txtdob.Focus()
                    flag = 0
                    Exit Sub
                End If
            Else
                flag = 1
            End If




            'SEBIN###----------------------------------------------------------------------------
            If Me.cmbCountry.SelectedValue = 1 Then

                If Me.cmbCountry.SelectedValue Is Nothing Or Me.cmbCountry.SelectedValue = 0 Then
                    MsgBox("Please Select the Country")
                    Me.cmbCountry.Focus()
                    flag = 0
                    Exit Sub
                ElseIf Me.cmbState.SelectedValue Is Nothing Or Me.cmbState.SelectedValue = 0 Then
                    MsgBox("Please Select the State")
                    Me.cmbState.Focus()
                    flag = 0
                    Exit Sub
                ElseIf Me.cmbDistrict.SelectedValue Is Nothing Or Me.cmbDistrict.SelectedValue = 0 Then
                    MsgBox("Please Select the District")
                    Me.cmbDistrict.Focus()
                    flag = 0
                    Exit Sub
                    'Dim aa As String
                    'aa = Me.cmbPost.SelectedValue.ToString()
                ElseIf Me.cmbPost.Text = "--Select--" Or Me.cmbPost.Text Is Nothing Then
                    MsgBox("Please Select the Post Office")
                    Me.cmbPost.Focus()
                    flag = 0
                    Exit Sub

                ElseIf Me.txtPincode.Text = "" Then
                    MsgBox("Please Select the Pincode")
                    Me.txtPincode.Focus()
                    flag = 0
                    Exit Sub
                End If
            End If
            '###----------------------------------------------------------------------------

            'If txtEmail.Text <> "" Or txtEmail.Text IsNot Nothing Then

            '    'EmailValidation()

            'End If
            'Dim txtemailvali As Boolean = EmailValidation(txtEmail)
            'If txtemailvali = False Then
            '    Exit Sub
            'End If


            ' -------------------------------------------------------------------------------
            If Me.txtAIdno.Text = "" Then
                MsgBox("Please enter Id No:")
                Me.txtAIdno.Focus()
                flag = 0
                Exit Sub
            Else
                Dim blnIsDuplicateID As Boolean = False
                Dim custkyctype As Integer = 0
                If rbtMtKyc.Checked Or rdoRegularKYC.Checked AndAlso cmbCustStatusnew.SelectedValue = "4" Then
                    custkyctype = 1
                End If
                If rbtMtKyc.Checked AndAlso cmbCustStatusnew.SelectedValue <> "4" Then
                    MsgBox("Selected Customer Type is " & cmbCustStatusnew.Text & " and KYC type is Money Transfer.Is Not Allowed ")
                    flag = 0
                    Exit Sub
                ElseIf cmbCustStatusnew.SelectedValue = "4" AndAlso rbtMtKyc.Checked = False AndAlso rdoRegularKYC.Checked = False Then
                    MsgBox("Selected Customer Type is " & cmbCustStatusnew.Text & " and KYC type is not Money Transfer or Regular. Is Not Allowed ")
                    flag = 0
                    Exit Sub
                Else
                    custkyctype = 0
                End If
                Dim objService As New customerService.customer(set_ip)
                If Not cmbPost.SelectedValue() Is Nothing Then
                    Dim PinSerial() As String = cmbPost.SelectedValue().ToString.Split("@")
                    If cmbId.SelectedValue() = 4 OrElse cmbId.SelectedValue() = 504 OrElse cmbId.SelectedValue() = 554 Then
                        blnIsDuplicateID = objService.CheckForDuplicateID(txtAIdno.Text.Trim(), 1, Convert.ToInt32(PinSerial(1)), custkyctype, "")
                    Else
                        blnIsDuplicateID = objService.CheckForDuplicateID(txtAIdno.Text.Trim(), 2, Convert.ToInt32(PinSerial(1)), custkyctype, "")
                    End If
                    If blnIsDuplicateID = True Then
                        MsgBox("Customer With Same Id No: Already Exist ...")
                        Me.txtAIdno.Focus()
                        flag = 0
                        Exit Sub
                    End If
                End If
            End If
            'If String.IsNullOrEmpty(kycDocFile) Then
            '    MsgBox("Scan and upload KYC Details")
            '    flag = 0
            '    Me.Main_tab.SelectedTab = Me.tb_addKyc
            '    Exit Sub
            'End If
        Catch ex As Exception 'added for req 4829
            Throw New ApplicationException("checkControls:- " & ex.Message)
        End Try
    End Sub

    'Shared Function EmailValidation(ByVal ctxt As System.Windows.Forms.TextBox) As Boolean
    '    If ctxt.Text.Trim <> "" Then
    '        Dim rex As Match = Regex.Match(Trim(ctxt.Text), "^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,3})$", RegexOptions.IgnoreCase)
    '        If rex.Success = False Then
    '            MessageBox.Show("Please Enter a valid Email-Address ")
    '            ctxt.Focus()
    '            Return False

    '        Else
    '            Return True
    '        End If
    '    Else
    '        Return True
    '    End If
    'End Function
    'Public Function ValidateEmailId(ByVal emailId As String) As Integer
    '    'Regular Expressions for email id

    '    Dim rEMail As New System.Text.RegularExpressions.Regex("^[a-zA-Z][\w\.-]{2,28}[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$")
    '    If emailId.Length > 0 Then
    '        If Not rEMail.IsMatch(emailId) Then
    '            Return 0
    '        Else
    '            Return 1
    '        End If
    '    End If
    '    Return 2
    'End Function
    Public Function GetCurrentAge(ByVal dob As Date) As Double
        Dim age As Double
        Dim nDiff As TimeSpan
        'age = Today.Year - dob.Year
        'If (dob > Today.AddYears(-age)) Then age -= 1

        nDiff = DateTime.Now.Subtract(dob)
        age = Format(nDiff.Days / 365)
        Return age

    End Function

    Public Sub align_mid()
        Dim aln As New Exception
        Try
            Dim mid As Integer = Me.Width / 2
            Dim midl As Integer = Me.GroupBox5.Width / 2
            Dim dp As Drawing.Point
            dp.X = 34
            dp.Y = GroupBox5.Location.Y
            GroupBox5.Width = Me.Width - 68

            ' Me.Panel2.Location = dp
        Catch aln
            MsgBox("aln" + aln.Message)
        End Try
    End Sub

    Private Sub cmbCMedia_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMedia.LostFocus
        Try
            Me.Cursor = Cursors.WaitCursor
            'If Me.cmbCMedia.SelectedValue Is Nothing Then
            '    Me.cmbCMedia.SelectedIndex = 0
            '    loadfill()
            '    MsgBox("Please Select Media From The List")
            '    Me.cmbCMedia.Focus()
            'End If
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Dim _droppedDown As Boolean = False
    Dim _prevDrawIndex As Integer = -1
    Private Sub cmbOccupation_DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles cmbOccupation.DrawItem
        Try

            Dim item As Object

            If e.Index >= 0 Then
                If e.Index < cmbOccupation.Items.Count Then
                    item = cmbOccupation.Items(e.Index)
                Else
                    item = Me.Text
                End If

                Dim bounds As Rectangle = e.Bounds
                Dim border As Integer = 1
                Dim height As Integer = Font.Height + 2 * border

                Dim textBounds As New Rectangle(bounds.X + (border * 2), bounds.Y, bounds.Width - (border * 2), bounds.Height)
                If RightToLeft = RightToLeft.Yes Then
                    textBounds.X = bounds.X
                End If
                Dim text As String = cmbOccupation.GetItemText(cmbOccupation.Items.Item(e.Index))
                Dim backColor__1 As Color = cmbOccupation.BackColor
                Dim foreColor__2 As Color = cmbOccupation.ForeColor
                If Not cmbOccupation.Enabled Then
                    foreColor__2 = SystemColors.GrayText
                End If
                Dim font__3 As Font = cmbOccupation.Font
                If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
                    backColor__1 = SystemColors.Highlight
                    foreColor__2 = SystemColors.HighlightText
                End If
                Using b As Brush = New SolidBrush(backColor__1)
                    e.Graphics.FillRectangle(b, textBounds)
                End Using

                Dim stringBounds As New Rectangle(textBounds.X + 1, textBounds.Y + border, textBounds.Width - 1, textBounds.Height - border * 2)

                Using format As New StringFormat()
                    If RightToLeft = RightToLeft.Yes Then
                        format.FormatFlags = format.FormatFlags Or StringFormatFlags.DirectionRightToLeft
                    End If

                    format.FormatFlags = format.FormatFlags Or StringFormatFlags.NoWrap
                    Using brush As New SolidBrush(foreColor__2)
                        e.Graphics.DrawString(text, font__3, brush, stringBounds, format)
                    End Using
                End Using
                If (e.State And DrawItemState.Focus) = DrawItemState.Focus AndAlso (e.State And DrawItemState.NoFocusRect) <> DrawItemState.NoFocusRect Then
                    ControlPaint.DrawFocusRectangle(e.Graphics, textBounds, foreColor__2, backColor__1)
                End If
            End If


            If Me.DesignMode = False AndAlso e.Index <> -1 AndAlso ((e.State And DrawItemState.Selected) = DrawItemState.Selected) Then
                If _droppedDown = True AndAlso _prevDrawIndex <> e.Index Then
                    tooltip.RemoveAll()
                    tooltip.AutomaticDelay = 0
                    tooltip.AutoPopDelay = 10000
                    tooltip.InitialDelay = 10
                    tooltip.IsBalloon = True
                    tooltip.ReshowDelay = 10
                    tooltip.Show(cmbOccupation.GetItemText(cmbOccupation.Items.Item(e.Index)), Me.cmbOccupation)
                    _prevDrawIndex = e.Index
                End If
            End If

        Catch ex As Exception

        End Try
    End Sub
    Private Sub cmbOccupation_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbOccupation.DropDown
        _droppedDown = True
    End Sub
    Private Sub cmbOccupation_DropDownClosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbOccupation.DropDownClosed
        _droppedDown = False
        tooltip.Hide(cmbOccupation)
    End Sub
    Private Sub cmbMoccu_DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles cmbMoccu.DrawItem
        Try

            Dim item As Object

            If e.Index >= 0 Then
                If e.Index < cmbMoccu.Items.Count Then
                    item = cmbMoccu.Items(e.Index)
                Else
                    item = Me.Text
                End If

                Dim bounds As Rectangle = e.Bounds
                Dim border As Integer = 1
                Dim height As Integer = Font.Height + 2 * border

                Dim textBounds As New Rectangle(bounds.X + (border * 2), bounds.Y, bounds.Width - (border * 2), bounds.Height)
                If RightToLeft = RightToLeft.Yes Then
                    textBounds.X = bounds.X
                End If
                Dim text As String = cmbMoccu.GetItemText(cmbMoccu.Items.Item(e.Index))
                Dim backColor__1 As Color = cmbMoccu.BackColor
                Dim foreColor__2 As Color = cmbMoccu.ForeColor
                If Not cmbMoccu.Enabled Then
                    foreColor__2 = SystemColors.GrayText
                End If
                Dim font__3 As Font = cmbMoccu.Font
                If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
                    backColor__1 = SystemColors.Highlight
                    foreColor__2 = SystemColors.HighlightText
                End If
                Using b As Brush = New SolidBrush(backColor__1)
                    e.Graphics.FillRectangle(b, textBounds)
                End Using

                Dim stringBounds As New Rectangle(textBounds.X + 1, textBounds.Y + border, textBounds.Width - 1, textBounds.Height - border * 2)

                Using format As New StringFormat()
                    If RightToLeft = RightToLeft.Yes Then
                        format.FormatFlags = format.FormatFlags Or StringFormatFlags.DirectionRightToLeft
                    End If

                    format.FormatFlags = format.FormatFlags Or StringFormatFlags.NoWrap
                    Using brush As New SolidBrush(foreColor__2)
                        e.Graphics.DrawString(text, font__3, brush, stringBounds, format)
                    End Using
                End Using
                If (e.State And DrawItemState.Focus) = DrawItemState.Focus AndAlso (e.State And DrawItemState.NoFocusRect) <> DrawItemState.NoFocusRect Then
                    ControlPaint.DrawFocusRectangle(e.Graphics, textBounds, foreColor__2, backColor__1)
                End If
            End If


            If Me.DesignMode = False AndAlso e.Index <> -1 AndAlso ((e.State And DrawItemState.Selected) = DrawItemState.Selected) Then
                If _droppedDown = True AndAlso _prevDrawIndex <> e.Index Then
                    tooltip.RemoveAll()
                    tooltip.AutomaticDelay = 0
                    tooltip.AutoPopDelay = 10000
                    tooltip.InitialDelay = 10
                    tooltip.IsBalloon = True
                    tooltip.ReshowDelay = 10
                    tooltip.Show(cmbMoccu.GetItemText(cmbMoccu.Items.Item(e.Index)), Me.cmbMoccu)
                    _prevDrawIndex = e.Index
                End If
            End If

        Catch ex As Exception

        End Try
    End Sub
    Private Sub cmbMoccu_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMoccu.DropDown
        _droppedDown = True
    End Sub
    Private Sub cmbMoccu_DropDownClosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMoccu.DropDownClosed
        _droppedDown = False
        tooltip.Hide(cmbMoccu)
    End Sub

    Private Sub cmbOccupation_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbOccupation.LostFocus
        Try
            Me.Cursor = Cursors.WaitCursor
            'If Me.cmbOccupation.SelectedValue Is Nothing Then
            '    Me.cmbOccupation.SelectedIndex = 0
            '    loadfill()
            '    MsgBox("Please Select Occupation From The List")
            '    Me.cmbOccupation.Focus()
            'End If
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub cmbCustStatus_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCustStatus.LostFocus
        Try
            Me.Cursor = Cursors.WaitCursor
            'If Me.cmbCustStatus.SelectedValue Is Nothing Then
            '    Me.cmbCustStatus.SelectedIndex = 0
            '    loadfill()
            '    MsgBox("Please Select Customer Status From The List")
            '    Me.cmbCustStatus.Focus()
            'End If
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub DateTimePicker1_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles DateTimePicker1.GotFocus
        If Me.DateTimePicker1.Focused = True Then
            If DateDiff(DateInterval.Day, Date.Now, CDate(Me.DateTimePicker2.Text)) > 0 Then
                Me.DateTimePicker2.Text = Date.Now.Date()
                Me.DateTimePicker2.Focus()
                MsgBox("Please enter the correct Date of Issue:")
            End If
        End If
    End Sub

    Private Sub txtACardNo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.txtACardNo.Focused = True Then
            If Me.txtMobileNo.Text = "" Then
                MsgBox("Please enter the mobile number")
                Me.txtMobileNo.Focus()
                Exit Sub
            End If
            If Me.txtIssuePlace.Text = "" Then
                MsgBox("Please enter the issuePlace")
                Me.txtIssuePlace.Focus()
                Exit Sub
            End If
            If Me.txtAIdno.Text = "" Then
                MsgBox("Please enter the Id no")
                Me.txtIssuePlace.Focus()
                Exit Sub
            End If
            If DateDiff(DateInterval.Day, CDate(Me.IssueDt.Text), CDate(Me.ExpiryDt.Text)) < 1 Then
                MsgBox("Please enter the correct  Expiry Date:")
                Me.ExpiryDt.Text = ""
                Me.ExpiryDt.Focus()
                Exit Sub
            End If
            If DateDiff(DateInterval.Day, CDate(Me.IssueDt.Text), Date.Now) > 0 Then
                MsgBox("Please enter the correct Date of Issue:")
                Me.IssueDt.Text = ""
                Me.IssueDt.Focus()
                Exit Sub
            End If
        End If

    End Sub

    Private Sub txtCardNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) <> 13 Then
            MsgBox("Read the card No through Barcode Reader")
        End If

    End Sub

    Private Sub btnAddKyc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddKyc.Click
        txt_search_GotFocus(sender, e)
        kycStatus = 1
        Me.Main_tab.TabPages(2).Enabled = True
        'Me.btnStartKyc.Enabled = True
        'Me.btnStopKyc.Enabled = True
        'Me.btnExitKyc.Enabled = True
        kycPhoto = Nothing
        Me.Main_tab.SelectedTab = Me.tb_addKyc
    End Sub

    Private Sub cmbReligion_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbReligion.SelectedIndexChanged
        Try
            Me.Cursor = Cursors.WaitCursor
            If Me.cmbReligion.Focused = True Then
                If Me.cmbReligion.Items.Count > 0 Then
                    castefill(Me.cmbReligion.SelectedValue)
                End If
            End If
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub btnAddPhoto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddPhoto.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            wchelper.Dispose()

            wchelper.Container = Me.picCustPhoto
            wchelper.Container.SizeMode = PictureBoxSizeMode.StretchImage
            wchelper.OpenConnection()
            wchelper.Load()
        Catch ex As Exception
            If ex.Message = "Object reference not set to an instance of an object." Then
                MsgBox("Check The web cam")
            End If
        Finally
            Me.Cursor = Cursors.Default
        End Try
        'photoStatus = 1
        'Me.Main_tab.SelectedTab = Me.tb_addPhoto
        'Me.picCustPhoto.Image = Nothing
    End Sub

    Private Sub cmbCMediatype_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCMediatype.SelectedIndexChanged
        Try
            code = ""
            Me.Cursor = Cursors.WaitCursor
            If Me.cmbCMediatype.Focused = True Then
                If Me.cmbCMediatype.Items.Count > 0 Then
                    Mediafill(Me.cmbCMediatype.SelectedValue)
                End If
            Else
                If Me.cmbCMediatype.SelectedIndex = 0 Then
                    Mediafill("0")
                End If
            End If
            'If Me.cmbCMediatype.Focused = True Then
            '    If Me.cmbCMediatype.SelectedValue = 2 AndAlso cmbCMedia.SelectedValue = 8 OrElse Me.cmbCustStatus.SelectedValue = 4 Then
            '        Me.Label75.Visible = True
            '        Me.txtEmpCode.Visible = True
            '        'Me.Label82.Visible = True
            '        Me.txtEmpName.Visible = True
            '    Else
            '        Me.Label75.Visible = False
            '        Me.txtEmpCode.Visible = False
            '        Me.txtEmpCode.Text = ""
            '        'Me.Label82.Visible = False
            '        Me.txtEmpName.Visible = False
            '    End If
            'End If
            'If Me.cmbCMediatype.Focused = True Then
            '    If Me.cmbCMediatype.SelectedValue = 31 Or Me.cmbCustStatus.SelectedValue = 4 Then
            '        Me.Label75.Visible = True
            '        Me.txtEmpCode.Visible = True
            '        'Me.Label82.Visible = True
            '        Me.txtEmpName.Visible = True
            '    Else
            '        Me.Label75.Visible = False
            '        Me.txtEmpCode.Visible = False
            '        Me.txtEmpCode.Text = ""
            '        'Me.Label82.Visible = False
            '        Me.txtEmpName.Visible = False
            '    End If
            'End If
            'If Me.cmbCMediatype.Focused = True Then
            '    If Me.cmbCMediatype.SelectedValue = 2 Or Me.cmbCustStatus.SelectedValue = 4 Then
            '        Me.Label75.Visible = True
            '        Me.txtEmpCode.Visible = True
            '        'Me.Label82.Visible = True
            '        Me.txtEmpName.Visible = True
            '    Else
            '        Me.Label75.Visible = False
            '        Me.txtEmpCode.Visible = False
            '        Me.txtEmpCode.Text = ""
            '        'Me.Label82.Visible = False
            '        Me.txtEmpName.Visible = False
            '    End If
            'End If
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub btnNeft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNeft.Click
        Try
            txt_search_GotFocus(sender, e)
            Me.Cursor = Cursors.WaitCursor
            neft_status = 1
            'Me.txtNeftCustid.Visible = False
            'Me.txtNeftCustnm.Visible = False
            Me.txtNeftCustid.Enabled = False
            Me.txtNeftCustnm.Enabled = False
            'Me.Label19.Visible = False
            'Me.Label46.Visible = False
            Me.btnStart.Visible = True
            Me.btnNeftSource.Visible = True
            Me.btnNeftSource.Visible = True
            fillneftdtl()
            Me.Main_tab.SelectedTab = Me.tb_neftdtl
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub cmbCustStatus_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCustStatus.SelectedIndexChanged
        If Me.cmbCustStatus.Focused = True Then
            If Me.cmbCustStatus.SelectedValue = 4 Then
                Me.Label75.Visible = True
                Me.txtEmpCode.Visible = True
                'Me.Label82.Visible = True
                Me.txtEmpName.Visible = True
            Else
                Me.Label75.Visible = False
                Me.txtEmpCode.Visible = False
                Me.txtEmpCode.Text = ""
                'Me.Label82.Visible = False
                Me.txtEmpName.Visible = False
                Me.txtEmpName.Text = ""
            End If
        End If
    End Sub

    Private Sub txtEmpCode_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtEmpCode.GotFocus
        Me.txtEmpCode.Text = ""
        Me.txtEmpName.Text = ""
        Me.txtEmpCode.Focus()
    End Sub

    Private Sub txtEmpCode_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtEmpCode.LostFocus
        Try
            Me.Cursor = Cursors.WaitCursor
            Dim result As String
            Dim obj As New customerService.customer(set_ip)
            Me.txtEmpName.Text = ""
            If Me.txtEmpCode.Text = "" Then
                Me.txtEmpCode.Focus()
                '   MsgBox("Enter Correct Employee Code")
            Else
                result = obj.checkEmployee(Me.txtEmpCode.Text)
                Dim resultarr() As String = result.Split("*")
                If resultarr(0) = 0 Then
                    Me.txtEmpName.Text = resultarr(1)
                Else
                    Me.txtEmpCode.Text = ""
                    Me.txtEmpName.Text = ""
                    Me.txtEmpCode.Focus()
                    MsgBox("Enter Correct Employee Code")
                End If
            End If
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub


    Private Sub TextBox21_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox21.LostFocus
        Try
            Me.Cursor = Cursors.WaitCursor
            Dim result As String
            Dim obj As New customerService.customer(set_ip)
            If Me.TextBox21.Text = "" Then
                Me.TextBox21.Focus()
                '   MsgBox("Enter Correct Employee Code")
            Else
                result = obj.checkEmployee(Me.TextBox21.Text)
                Dim resultarr() As String = result.Split("*")
                If resultarr(0) = 0 Then
                    Me.TextBox22.Text = resultarr(1)
                Else
                    Me.TextBox21.Text = ""
                    Me.TextBox22.Text = ""
                    Me.TextBox21.Focus()
                    MsgBox("Enter Correct Employee Code")
                End If
            End If
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub


#End Region
    Private Sub cmbState_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbState.LostFocus
        Try
            Me.Cursor = Cursors.WaitCursor
            'If Me.cmbState.SelectedValue Is Nothing Then
            '    Me.cmbState.SelectedIndex = -1
            '    loadfillModify()
            '    MsgBox("Please Select State From The List")
            '    Me.cmbState.Focus()
            'End If
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub cmbState_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbState.SelectedIndexChanged
        Try
            Me.Cursor = Cursors.WaitCursor
            'If Me.cmbState.Focused = True Then
            If Me.cmbState.SelectedIndex > 0 AndAlso Me.cmbState.Items.Count > 0 Then
                adistrictfill(Me.cmbState.SelectedValue)
            Else
                Me.cmbDistrict.DataSource = dumdt
                Me.cmbPost.DataSource = dumdt
                Me.txtPincode.Text = ""
            End If
            'End If


        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub cmbDistrict_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDistrict.SelectedIndexChanged
        Try
            Me.Cursor = Cursors.WaitCursor
            'If Me.cmbDistrict.Focused = True Then
            If Me.cmbDistrict.SelectedIndex > 0 AndAlso Me.cmbDistrict.Items.Count > 0 Then
                APostfill(Me.cmbDistrict.SelectedValue)
            Else
                Me.cmbPost.DataSource = dumdt
                Me.txtPincode.Text = ""
            End If
            'End If
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub cmbPost_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPost.LostFocus
        Try
            Me.Cursor = Cursors.WaitCursor
            'If cmbPost.SelectedValue Is Nothing Then
            '    MsgBox("Select a post from the list")
            '    Me.txtPincode.Text = ""
            '    APostfill(Me.cmbDistrict.SelectedValue)
            'End If
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub cmbPost_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbPost.SelectedIndexChanged
        Try
            Me.Cursor = Cursors.WaitCursor
            'If Me.cmbPost.Focused = True Then
            If Me.cmbPost.SelectedIndex > 0 AndAlso Me.cmbPost.Items.Count > 0 Then
                APinfill(Me.cmbPost.SelectedValue)
            Else
                Me.txtPincode.Text = ""
            End If
            'End If
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub cmbCountry_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCountry.SelectedIndexChanged
        Try
            Me.Cursor = Cursors.WaitCursor
            'If Me.cmbCountry.Focused = True Then
            '    If Me.cmbCountry.Items.Count > 0 Then
            '        If Me.cmbCountry.SelectedIndex = 1 Then
            '            Astatefill(Me.cmbCountry.SelectedValue)
            '            Me.cmbState.Enabled = True
            '            Me.cmbDistrict.Enabled = True
            '            Me.cmbPost.Enabled = True
            '        Else
            '            'dumdt.Clear()
            '            Me.cmbState.SelectedValue = 0
            '            Me.cmbDistrict.SelectedValue = 0
            '            Me.cmbPost.SelectedValue = 0

            '            Me.cmbState.Enabled = False
            '            Me.cmbDistrict.Enabled = False
            '            Me.cmbPost.Enabled = False
            '            Me.txtPincode.Text = 0
            '        End If

            '    Else
            '        Me.cmbState.DataSource = dumdt
            '        Me.cmbDistrict.DataSource = dumdt
            '        Me.cmbPost.DataSource = dumdt
            '        Me.txtPincode.Text = ""
            '    End If
            'End If
            'Me.cmbState.Enabled = True
            'Me.cmbDistrict.Enabled = True
            Me.cmbPost.Enabled = True

            If cmbCountry.SelectedIndex > 0 AndAlso Me.cmbCountry.Items.Count > 0 Then
                Astatefill(Me.cmbCountry.SelectedValue)

                If Me.cmbCountry.SelectedIndex > 1 Then
                    Me.cmbState.Enabled = False
                    Me.cmbDistrict.Enabled = False
                    Me.cmbPost.Enabled = False
                    Me.txtPincode.Text = "0"
                Else
                    If cmbId.SelectedValue <> 16 Then
                        Me.cmbState.Enabled = True
                        Me.cmbDistrict.Enabled = True
                        Me.cmbPost.Enabled = True
                    Else
                        Me.cmbState.Enabled = False
                        Me.cmbDistrict.Enabled = False
                        'Me.cmbPost.Enabled = False
                    End If

                End If
            Else
                Me.cmbState.DataSource = dumdt
                Me.cmbDistrict.DataSource = dumdt
                Me.cmbPost.DataSource = dumdt
                Me.txtPincode.Text = ""
            End If


        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
        Me.txtPhcode.Text = String.Empty

    End Sub
    Private Sub txtPanNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPanNo.KeyPress
        e.KeyChar = CChar(e.KeyChar.ToString.ToUpper)
    End Sub
    Private Sub txtPanNo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPanNo.LostFocus
        'If Me.txtPanNo.Text.Length = 10 Then
        '    Dim hid As String = Me.txtPanNo.Text.Substring(0, Me.txtPanNo.Text.Length - 5)
        '    Dim hid2 As String = Me.txtPanNo.Text.Substring(5, Me.txtPanNo.Text.Length - 6)
        '    Dim hid3 As String = Me.txtPanNo.Text.Substring(9, Me.txtPanNo.Text.Length - 9)
        '    Dim i As Integer
        '    For i = 0 To hid.Length - 1
        '        CheckAtoZ(hid(i))
        '        If ss = False Then
        '            MsgBox("Must Enter Correct Format,ie First 5 alphabets,then 4 nuemeric,last i alphabet", MsgBoxStyle.Information, "Check it")
        '            Me.txtPanNo.Text = ""
        '            Me.txtPanNo.Focus()
        '            Exit Sub
        '        End If
        '    Next
        '    Dim j As Integer
        '    For j = 0 To hid2.Length - 1
        '        IsNumeric(hid2(j))
        '        If ss = False Then
        '            MsgBox("Must Enter Correct Format,ie First 5 alphabets,then 4 nuemeric,last i alphabet", MsgBoxStyle.Information, "Check it")
        '            Me.txtPanNo.Text = ""
        '            Me.txtPanNo.Focus()
        '            Exit Sub
        '        End If
        '    Next
        '    CheckAtoZ(hid3)
        '    If ss = False Then
        '        MsgBox("Must Enter Correct Format,ie First 5 alphabets,then 4 nuemeric,last i alphabet", MsgBoxStyle.Information, "Check it")
        '        Me.txtPanNo.Text = ""
        '        Me.txtPanNo.Focus()
        '        Exit Sub
        '    End If
        'Else
        '    MsgBox("Must Enter Correct Format,ie First 5 alphabets,then 4 nuemeric,last i alphabet", MsgBoxStyle.Information, "Check it")
        '    Me.txtPanNo.Text = ""
        '    Me.txtPanNo.Focus()
        '    Exit Sub
        'End If
    End Sub
    Function CheckAtoZ(ByVal chr1) As Boolean
        ss = True
        If ((Asc(chr1) < 65) Or (Asc(chr1) > 90)) Then
            ss = False
            Exit Function
        End If
    End Function
    Private Sub txtMFatHus_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMFatHus.KeyPress
        e.KeyChar = CChar(e.KeyChar.ToString.ToUpper)
    End Sub
    Private Sub txtMHno_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMHno.KeyPress
        e.KeyChar = CChar(e.KeyChar.ToString.ToUpper)
    End Sub

    Private Sub txtMIdno_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMIdno.KeyPress
        e.KeyChar = CChar(e.KeyChar.ToString.ToUpper)
    End Sub

    Private Sub txtMLocation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMLocation.KeyPress
        e.KeyChar = CChar(e.KeyChar.ToString.ToUpper)
    End Sub

    Private Sub txtIssuePlace_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtIssuePlace.KeyPress
        e.KeyChar = CChar(e.KeyChar.ToString.ToUpper)
    End Sub

    'Private Sub add_Land_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Try
    '        CheckForOnlineCustomer()
    '        Me.Cursor = Cursors.WaitCursor
    '        If Me.txtCustomerid.Text <> "" Then
    '            Me.Main_tab.TabPages(1).Enabled = True
    '            Me.tb_customermodify.BackColor = Drawing.Color.LightSteelBlue
    '            Land_DetailsUpdateBinding()
    '            Me.Main_tab.TabPages(7).Enabled = True
    '            Me.TextBox18.Text = ""
    '        Else
    '            MsgBox("Search Customer Details")
    '            Me.Main_tab.SelectedTab = Me.tb_search
    '        End If
    '    Catch ex As Exception 'added for req 4829
    '        MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    Finally
    '        Me.Cursor = Cursors.Default
    '    End Try
    'End Sub

    'Protected Sub Land_DetailsUpdateBinding()

    '    Me.TextBox16.DataBindings.Clear()
    '    Me.TextBox17.DataBindings.Clear()
    '    Me.TextBox18.DataBindings.Clear()

    '    If Not cust_result Is Nothing Then
    '        Me.TextBox16.DataBindings.Add("Text", cust_result, "CUST_ID")
    '        Me.TextBox17.DataBindings.Add("Text", cust_result, "name")
    '    End If
    'End Sub


    'Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Try
    '        Me.Cursor = Cursors.WaitCursor
    '        Dim obj As New customerService.customer(set_ip)
    '        Dim confstr As String = ""
    '        Dim result As String = ""
    '        'confstr = Me.ComboBox1.SelectedValue.ToString

    '        'confstr += "¥" + Me.TextBox18.Text.ToString 'LAND CERTIFICATE NO
    '        'confstr += "¥" + Me.TextBox17.Text.ToString 'CUSTOMER NAME
    '        'confstr += "¥" + Me.TextBox16.Text.ToString 'CUSTOMER ID

    '        Try
    '            If Me.TextBox16.Text = "" Then
    '                result = "Null Value!! Please Enter Land Certificate No"
    '            End If
    '            If Me.TextBox17.Text = "" Then
    '                result = "Null Value!! Please Check Customer details Before confirmation"
    '            End If
    '            If Me.TextBox18.Text = "" Then
    '                result = "Null Value!! Please Check Customer details Before confirmation"
    '            End If

    '            If Me.TextBox16.Text <> "" Then
    '                If confstr <> "" Then
    '                    result = obj.customeraddCerDtls(confstr, FirmID, BranchID, User_id)
    '                    ' result = "Yes"
    '                Else
    '                    result = "Null Value!! Please Check the Details"
    '                End If
    '            End If
    '        Catch ex As Exception
    '            result = ex.Message
    '        End Try

    '        MsgBox(result)
    '    Catch ex As Exception 'added for req 4829
    '        MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    Finally
    '        Me.Cursor = Cursors.Default
    '    End Try
    'End Sub

    Public Function checkMobileno(ByRef KeyCode As String, ByRef Control As TextBox) As Boolean
        ' MsgBox(KeyCode)
        Dim mobleNolen As Integer = Control.Text.Length
        If mobleNolen < 10 Then
            Control.Text = Control.Text
        Else
            If mobleNolen < 11 Then
                Control.Text = "0" & Control.Text
                MsgBox("Cannot Add More !")
            End If
        End If
    End Function
    Public Function checkSTDCode(ByRef KeyCode As String, ByRef Control As TextBox) As Boolean
        If Control.Text.Length >= 1 AndAlso Control.Text.Length <= 4 Then
            If Control.Text.Substring(0, 1) <> 0 Then
                If Control.Text.Length = 4 Then
                    Control.Text = "0" & Control.Text.Substring(0, 3)
                ElseIf Control.Text.Length = 3 Then
                    Control.Text = "0" & Control.Text.Substring(0, 2)
                ElseIf Control.Text.Length = 2 Then
                    Control.Text = "0" & Control.Text.Substring(0, 1)
                ElseIf Control.Text.Length = 1 Then
                    Control.Text = "0" & Control.Text.Substring(0, 0)
                End If
            End If
            If Control.Text.Length >= 2 Then
                If Control.Text.Substring(1, 1) = 0 Then
                    Control.Text = Control.Text.Remove(1, 1)
                End If
            End If

        End If
    End Function

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Main_tab.SelectedTab = Me.tb_search
    End Sub


    Public Function checkLessMobileno(ByRef KeyCode As String, ByRef Control As TextBox) As Boolean
        ' MsgBox(KeyCode)
        Dim mobleNolen As Integer = Control.Text.Length
        If mobleNolen < 11 Then
            Control.Text = Control.Text
            Control.Text = "0" & Control.Text
            MsgBox("Please Enter Atleast 11 digits")
        End If
    End Function


    'Private Sub txtMobileNo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMobileNo.LostFocus
    '    If Me.txtMobileNo.Text.Trim <> "" AndAlso Me.txtMobileNo.Text.Length <> 11 Then
    '        MsgBox("Enter Correct 10 Digit Mobile No!!")
    '        Me.txtMobileNo.Clear()
    '    End If
    'End Sub



    Private Sub cmbMcuststat_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMcuststat.SelectedIndexChanged
        If Me.cmbMcuststat.Items.Count > 0 Then
            If Me.cmbMcuststat.SelectedIndex > 0 AndAlso Me.cmbMcuststat.SelectedValue = 4 OrElse Me.cmbMediaType.SelectedValue = 2 AndAlso cmbMedia.SelectedValue = 8 Then
                Me.Label108.Visible = True
                Me.TextBox21.Visible = True
                Me.Label109.Visible = True
                Me.TextBox22.Visible = True
            Else
                Me.Label108.Visible = False
                Me.TextBox21.Visible = False
                Me.Label109.Visible = False
                Me.TextBox22.Visible = False
                TextBox21.Text = ""
                TextBox22.Text = ""
            End If
        End If
    End Sub

    ''' <summary>
    ''' TO capture Photo of pancard through web cam. Coded by Prasanth 
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
    Private Sub btn_Photo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_pan_photo.Click
        If photoStatus = 1 Then

            panphoto = wchelper.SaveImage()
            If panphoto Is Nothing Then
                Me.panPhotoCtl.Image = Nothing
            Else
                Me.panPhotoCtl.Image = panPhotoCtl.Image
            End If
            Me.Main_tab.SelectedTab = Me.tab_Pan
        Else
            Try
                Me.Cursor = Cursors.WaitCursor

                panphoto = wchelper.SaveImage()
                If panphoto Is Nothing Then
                    panPhotoCtl.Image = Nothing
                    'btn_Submit.Visible = True
                Else
                    panPhotoCtl.Image = panPhotoCtl.Image
                    btn_Submit.Visible = True
                End If
            Catch ex As Exception 'added for req 4829
                MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End If
    End Sub

    ''' <summary>
    '''  Coded added by prasanth. Handles the Enter event of the tab_Pan control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>

    Private Sub tab_Pan_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tab_Pan.Enter
        Try
            CheckForOnlineCustomer()
            Me.Cursor = Cursors.WaitCursor
            Me.txt_pan_no.Text = ""
            If Me.txtCustomerid.Text <> "" Then
                Me.Main_tab.TabPages(1).Enabled = True
                btn_edit_Pan.Visible = True
                btn_pan_photo.Visible = False
                btn_Submit.Visible = False
                btn_pan_start.Visible = False
                Dim sql As String = "select  pan_copy,pan from dms.deposit_pan_detail where cust_id='" + txtCustomerid.Text + "'"
                Dim dt As New Data.DataTable
                Dim ws As New customerService.customer(set_ip)
                dt = ws.QueryResult(sql).Tables(0)
                txt_pan_custname.Text = txtcustname.Text
                txt_pan_Custno.Text = txtCustomerid.Text
                Dim cp As String = System.IO.Path.GetTempPath()
                If dt.Rows.Count > 0 Then
                    If Not IsDBNull(dt.Rows(0)(0)) Then
                        Dim fnm As String
                        fnm = cp + "show.jpg"
                        Dim fp As New System.IO.FileInfo(fnm)
                        If fp.Exists() Then
                            fp.Delete()
                        End If
                        Dim fs As New System.IO.FileStream(fnm, System.IO.FileMode.Create)
                        Dim bl() As Byte
                        bl = CType(dt.Rows(0)(0), Byte())
                        fs.Write(bl, 0, bl.Length)
                        fs.Close()
                        fs = Nothing
                        Dim file_name As String = fnm.Substring(fnm.IndexOf("show"))
                        Me.panPhotoCtl.ImageLocation = cp + file_name
                        txt_pan_no.Text = dt.Rows.Item(0).Item("pan")
                    End If
                    txt_pan_no.Text = dt.Rows.Item(0).Item("pan")
                End If
            ElseIf txtCustId.Text <> "" Then
                Me.Main_tab.TabPages(1).Enabled = True
                btn_edit_Pan.Visible = True
                btn_pan_photo.Visible = False
                btn_Submit.Visible = False
                btn_pan_start.Visible = False
                Dim sql As String = "select  pan_copy,pan from dms.deposit_pan_detail where cust_id='" + txtCustomerid.Text + "'"
                Dim dt As New Data.DataTable
                Dim ws As New customerService.customer(set_ip)
                dt = ws.QueryResult(sql).Tables(0)
                txt_pan_Custno.Text = txtCustId.Text
            Else
                MsgBox("Search Customer Details")
                Me.Main_tab.SelectedTab = Me.tb_search
            End If
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary>
    ''' Handles the Click event of the btn_Submit control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
    Private Sub btn_Submit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Submit.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            '---------------coded by: prasanth  pancard validation ---------------------
            Dim regexpan As New Regex("[A-Z]{5}\d{4}[A-Z]{1}", RegexOptions.IgnoreCase)
            If regexpan.IsMatch(txt_pan_no.Text) = False Then
                MsgBox("Must Enter Correct Format,ie First 5 alphabets,then 4 nuemeric,last i alphabet", MsgBoxStyle.Information, "Check it" + txtEmpCode.Text)
                txt_pan_no.Text = ""
                txt_pan_no.Focus()
            Else
                Dim obj As New customerService.customer(set_ip)
                Dim confdata As String
                User_id = User_id
                confdata = 0 & "£" & txt_pan_Custno.Text + "£" + "0" + "£" + "0" + "£" + txt_pan_no.Text + "£" + UserId
                Dim mesg As String = obj.PANcardModification(confdata, panphoto)
                MessageBox.Show(mesg)
                Main_tab.SelectTab(tb_search)
            End If
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    '---------------coded by: prasanth  pancard validation ---------------------
    ''' <summary>
    ''' Handles the KeyPress event of the txt_pan_no control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.Windows.Forms.KeyPressEventArgs" /> instance containing the event data.</param>
    Private Sub txt_pan_no_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_pan_no.KeyPress
        e.KeyChar = CChar(e.KeyChar.ToString.ToUpper)
    End Sub

    '---------------coded by: prasanth  pancard validation ---------------------
    ''' <summary>
    ''' Handles the Click event of the btn_edit_Pan control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
    Private Sub btn_edit_Pan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_edit_Pan.Click
        txt_pan_no.ReadOnly = False
        'btn_pan_photo.Visible = True
        btn_pan_start.Visible = True
        txt_pan_no.Text = ""
        txt_pan_no.Focus()
    End Sub

    ''' <summary>
    ''' Handles the Click event of the btn_pan_start control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
    Private Sub btn_pan_start_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_pan_start.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            wchelper.Container = panPhotoCtl
            wchelper.Container.SizeMode = PictureBoxSizeMode.StretchImage
            wchelper.OpenConnection()
            wchelper.Load()
            btn_pan_photo.Visible = True
            If btn_Submit.Visible = True Then
                btn_Submit.Visible = False
            End If
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Function Addhead(ByVal dt As DataTable, ByVal txtfield As String, ByVal valuefield As String) As DataTable

        If Not IsNothing(dt) AndAlso dt.Rows.Count > 0 Then

            Dim dr As DataRow = dt.NewRow
            dr(txtfield) = "--Select--"
            If dt.Columns(valuefield).DataType.Name.ToLower = "string" Then
                dr(valuefield) = ""
            Else
                dr(valuefield) = 0
            End If
            dt.Rows.InsertAt(dr, 0)
        End If
        Return dt
    End Function


    'Private Sub cmbId_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbId.SelectedIndexChanged
    '    If Me.cmbId.SelectedIndex > 0 Then
    '        If cmbId.SelectedValue = "1" Or cmbId.SelectedValue = "3" Or cmbId.SelectedValue = "4" Then
    '            cmbOccupation.SelectedValue = "2"
    '            cmb_landHldtl.SelectedValue = "3"
    '            cmbOccupation.Enabled = False
    '            cmb_landHldtl.Enabled = False
    '        Else
    '            If cmbOccupation.Items.Count > 0 Then cmbOccupation.SelectedIndex = 0
    '            If cmb_landHldtl.Items.Count > 0 Then cmb_landHldtl.SelectedIndex = 0
    '            cmbOccupation.Enabled = True
    '            cmb_landHldtl.Enabled = True
    '        End If
    '    Else
    '        If cmbOccupation.Items.Count > 0 Then cmbOccupation.SelectedIndex = 0
    '        If cmb_landHldtl.Items.Count > 0 Then cmb_landHldtl.SelectedIndex = 0
    '        cmbOccupation.Enabled = True
    '        cmb_landHldtl.Enabled = True
    '    End If
    'End Sub


    Private Function GetValue(ByVal ddl As ComboBox) As String
        If Not IsNothing(ddl) AndAlso ddl.SelectedIndex > 0 Then
            Return ddl.SelectedValue
        Else
            Return String.Empty
        End If
    End Function

    Private Sub cmbMid_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMid.Leave
        If cmbMid.SelectedIndex > 0 Then
            'If cmbMid.SelectedValue = "1" Or cmbMid.SelectedValue = "3" Or cmbMid.SelectedValue = "4" Then
            '    cmbMoccu.SelectedValue = "2"
            '    cmbMoccu.Enabled = False
            'Else
            '    'If cmbMid.Focused Then
            '    '    cmbMoccu.SelectedIndex = 0
            '    'End If
            '    cmbMoccu.Enabled = True
            'End If

            'tooltipCmb.Show(cmbMid.Text, cmbMid, New Drawing.Point(cmbMid.ClientRectangle.X + cmbMid.Width, cmbMid.ClientRectangle.Y))
            'code added to restrict to change kyc
            If custBranchID <> BranchID Then
                'MessageBox.Show("Can't modify other branch customer's KYC", "Customer", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Dim result = MessageBox.Show("You are trying to modify other branch customer's KYC document..It will Require SRM Approval. Really want to Modify?", "Confirm", MessageBoxButtons.YesNo)
                If result = DialogResult.No Then
                    If idtype <> "" Then
                        cmbMid.SelectedValue = CInt(idtype)
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub cmbOccupation_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbOccupation.SelectedIndexChanged
        Try

            Me.Cursor = Cursors.WaitCursor
            If cmbOccupation.SelectedIndex > 0 Then
                PLoanFill(cmbOccupation.SelectedValue)

                If cmbOccupation.SelectedValue = "2" Then
                    cmbAPurposeofloan.SelectedIndex = 1
                    cmbAPurposeofloan.Enabled = False
                Else
                    If cmbAPurposeofloan.Items.Count > 0 Then
                        cmbAPurposeofloan.SelectedIndex = 0
                        cmbAPurposeofloan.Enabled = True
                    End If
                End If
            Else
                If cmbAPurposeofloan.Items.Count > 0 Then
                    cmbAPurposeofloan.SelectedIndex = 0
                    cmbAPurposeofloan.Enabled = True
                End If
            End If
            If cmbOccupation.SelectedIndex = 40 Then
                Dim objVeh As New VehicleFinanceSurvey
                objVeh.Show()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub PLoanFill(ByVal val As String)
        If tempPLoane.Rows.Count > 0 Then
            If val = "2" Then
                AddColumn(0)
            Else
                AddColumn(1)
            End If
            Me.cmbAPurposeofloan.DataSource = Addhead(FiteredRecord(tempPLoane), tempPLoane.Columns(1).ColumnName, tempPLoane.Columns(0).ColumnName) 'Added for req 4829
            Me.cmbAPurposeofloan.DisplayMember = tempPLoane.Columns(1).ColumnName
            Me.cmbAPurposeofloan.ValueMember = tempPLoane.Columns(0).ColumnName
        End If

    End Sub

    Private Sub AddColumn(ByVal setHilde As Integer)
        If Not tempPLoane.Columns.Contains("status") Then tempPLoane.Columns.Add(New DataColumn("status"))
        For i As Integer = 0 To tempPLoane.Rows.Count - 1
            tempPLoane.Rows(i).Item("status") = 1
            If setHilde = 1 AndAlso tempPLoane.Rows(i).Item("status_id") = 1 Then
                tempPLoane.Rows(i).Item("status") = 0
            End If
        Next
        tempPLoane.AcceptChanges()
    End Sub

    Private Function FiteredRecord(ByVal dt As DataTable)
        Dim dtOut As New DataTable
        If Not IsNothing(dt) Then
            dtOut = dt.Clone
            Dim dr() As DataRow = dt.Select("status=1")
            If Not IsNothing(dr) And dr.Length > 0 Then
                For Each dra As DataRow In dr
                    dtOut.ImportRow(dra)
                Next
            End If
        End If
        Return dtOut
    End Function

    Private Function ArrangeData(ByVal dt As DataTable) As DataTable
        Dim dtout As New DataTable
        Dim arrList As New ArrayList
        Dim param As String
        Dim sql As String = "select parmtr_name,parmtr_value from general_parameter where firm_id=1  and module_id=0 and parmtr_id=70"
        Dim outDS As New Data.DataSet
        Dim ws As New customerService.customer(set_ip)
        outDS = ws.QueryResult(sql)
        If outDS.Tables.Count > 0 AndAlso outDS.Tables(0).Rows.Count > 0 Then
            param = outDS.Tables(0).Rows(0)(1).ToString
            If param <> "" Then
                For Each strParam As String In param.Split(",")
                    arrList.Add(strParam)
                Next
            End If
        End If

        If Not IsNothing(dt) AndAlso dt.Rows.Count > 0 AndAlso arrList.Count > 0 Then
            dtout = dt.Clone
            For i As Integer = 0 To arrList.Count - 1
                For j As Integer = 0 To dt.Rows.Count - 1
                    If arrList(i) = dt.Rows(j)("occupation_id").ToString Then
                        dtout.ImportRow(dt.Rows(j))
                    End If
                Next
            Next
            For i As Integer = 0 To dt.Rows.Count - 1
                If Not arrList.Contains(dt.Rows(i)("occupation_id").ToString) Then
                    dtout.ImportRow(dt.Rows(i))
                End If
            Next
        Else
            dtout = dt
        End If
        Return dtout
    End Function

    Private Sub txtMMobno_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMMobno.KeyUp
        checkForNumeric(e.KeyCode, Me.txtMMobno)
        'checkMobileno(e.KeyCode, Me.txtMMobno)
    End Sub


    Private Sub txtMMobno_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMMobno.LostFocus
        If Me.txtMMobno.Text.Trim <> "" AndAlso Me.txtMMobno.Text.Length <> 10 Then
            MsgBox("Enter Correct 10 Digit Mobile No!!")
            Me.txtMMobno.Clear()
        End If
    End Sub

    Private Sub btnmody_address_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnmody_address.Click
        Dim altadress As New Adress2(2, txtMHno.Text, txtMLocation.Text, cmbMcountry.SelectedValue, cmbMstate.SelectedValue, cmbMDistrict.SelectedValue, cmbMPost.SelectedValue, txtMPincode.Text) ' 2 to indicate existing customer 
        altadress.ShowDialog()
    End Sub

    Private Sub btn_alt_address_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_alt_address.Click
        Dim altadress As New Adress2(1, txtHouse.Text, txtALocation.Text, cmbCountry.SelectedValue, cmbState.SelectedValue, cmbDistrict.SelectedValue, cmbPost.SelectedValue, txtPincode.Text) ' 1 to indicate  new customer 
        altadress.ShowDialog()
    End Sub

    Dim tooltip As New ToolTip
    Private Sub lnkInstruction_MouseHover(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInstruction.MouseHover
        btnInstruction.BackColor = Drawing.Color.LightGreen
        Dim sb As New System.Text.StringBuilder
        sb.Append("General Instruction" & vbCrLf)
        sb.Append("1. First scan the proof of identity then scan the proof of address one by one." & vbCrLf)
        sb.Append("2. If the document having text only then select Scan mode as Block & White else choose Gray Scale." & vbCrLf)
        sb.Append("3. Select the scanner from the source list on clicking the source button. If the scanner is already selected as default then leave it." & vbCrLf)
        sb.Append("4. Scan the document on clicking the scan button. Before scanning it will pop up one dialog asking about the number of pages, enter the correct pages to scan." & vbCrLf)
        sb.Append("5. After scanning click upload button to upload all the document.")
        sb.Append(vbCrLf)
        sb.Append("Source" & vbCrLf)
        sb.Append("-Select the scanner from the list" & vbCrLf)
        sb.Append(vbCrLf)
        sb.Append("Scan" & vbCrLf)
        sb.Append("-Scan the new document" & vbCrLf)
        sb.Append(vbCrLf)
        sb.Append("Upload" & vbCrLf)
        sb.Append("-Upload the scanned/Loaded document" & vbCrLf)
        sb.Append(vbCrLf)
        sb.Append("Reset" & vbCrLf)
        sb.Append("-Reset the scanned image in the viewer" & vbCrLf)
        sb.Append(vbCrLf)
        sb.Append("Append" & vbCrLf)
        sb.Append("-Scan and append the document with existing database document" & vbCrLf)

        tooltip.IsBalloon = True
        tooltip.ToolTipTitle = "Scanning Instruction"
        tooltip.UseAnimation = False
        tooltip.InitialDelay = 0
        tooltip.UseFading = False
        tooltip.ShowAlways = True
        tooltip.AutoPopDelay = 300 * 60 * 60
        tooltip.ToolTipIcon = ToolTipIcon.Info
        tooltip.SetToolTip(Me.btnInstruction, sb.ToString)
    End Sub

    Private Sub btnInstruction_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInstruction.MouseLeave
        tooltip.AutoPopDelay = 300
        tooltip.Hide(Me)
        btnInstruction.BackColor = Drawing.Color.FromArgb(255, 51, 102, 153)
    End Sub


    Public Function ConvertImageFiletoBytes(ByVal ImageFilePath As String) As Byte()
        Dim _tempByte() As Byte = Nothing
        Try
            If File.Exists(ImageFilePath) Then
                Dim _fileInfo As New System.IO.FileInfo(ImageFilePath)
                Dim _NumBytes As Long = _fileInfo.Length
                Dim _FStream As New System.IO.FileStream(ImageFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read)
                Dim _BinaryReader As New System.IO.BinaryReader(_FStream)
                _tempByte = _BinaryReader.ReadBytes(Convert.ToInt32(_NumBytes))
                _fileInfo = Nothing
                _NumBytes = 0
                _FStream.Close()
                _FStream.Dispose()
                _BinaryReader.Close()
                Return _tempByte
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Private Sub btnLoadImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadImage.Click
        Try
            Dim objLoadImage As New frmLoadFromFile
            objLoadImage.ScanMode = cmbScanMode.Text
            objLoadImage.AnnotationText = "Uploaded by " & UserId & " from " & BranchName & " on " & Now.ToString("dd-MMM-yyyy hh:mm tt")
            If objLoadImage.ShowDialog() = DialogResult.Yes Then
                If File.Exists(objLoadImage.OutFilePath) Then
                    kycDocFile = objLoadImage.OutFilePath

                    isappend = False
                    If File.Exists(kycDBFile) Then
                        If MessageBox.Show("This Customer already have KYC document." & vbCrLf & "Click 'YES' to append with existing document" & vbCrLf & "Clcik 'NO' to proceed with fresh document", "Customer", MessageBoxButtons.YesNo, MessageBoxIcon.Information) = DialogResult.Yes Then
                            ScanX1.AppendFileNew(kycDBFile, kycDocFile)
                            kycDocFile = kycDBFile
                            isappend = True
                        End If
                    End If

                    ScanX1.ImageFileLoad(kycDocFile)

                    MessageBox.Show("Please click upload button to save the image....", "Customer", MessageBoxButtons.OK, MessageBoxIcon.Information)

                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Function LoadKYCPhoto(ByVal custid As String)
        Try
            Dim sql As String = "select kyc_photo from customer_photo where cust_id='" & custid & "'"
            Dim ds As New Data.DataSet
            Dim ws As New customerService.customer(set_ip)
            ws.Timeout = 3 * 1000 * 60
            ds = ws.QueryResult(sql)
            If ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 AndAlso Not IsDBNull(ds.Tables(0).Rows(0)(0)) Then
                isKycDocAvailable = True
                Dim binary As Byte()
                binary = CType(ds.Tables(0).Rows(0)(0), Byte())
                kycDBFile = Path.GetTempFileName & "ASKyc.tif"
                Dim fs As New FileStream(kycDBFile, FileMode.OpenOrCreate, FileAccess.Write)
                fs.Write(binary, 0, binary.Length)
                fs.Close()
                fs.Dispose()
            Else
                kycDBFile = ""
                isKycDocAvailable = False
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function

    Private Sub btnAppend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAppend.Click
        Try
            btnLoadImage.Enabled = False
            btnAppend.Enabled = False
            btnScan.Enabled = False
            btnUpload.Enabled = False
            If Not ScanImage() Then
                MessageBox.Show("Scanning Aborted", "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            kycDocFile = ScanX1.ScannedImagePath
            If File.Exists(kycDBFile) And File.Exists(kycDocFile) Then
                ScanX1.AppendFileNew(kycDBFile, kycDocFile)
                kycDocFile = kycDBFile
                ScanX1.ImageFileLoad(kycDocFile)
            End If
            isappend = True
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
            btnLoadImage.Enabled = True
            btnAppend.Enabled = True
            btnScan.Enabled = True
            btnUpload.Enabled = True
        End Try
    End Sub

    Private Sub chkAddress_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAddress.CheckedChanged
        If chkAddress.Checked Then
            chkSameAsId.Visible = True
            lblAddressProof.Visible = True
            cmbAddressProf.Visible = True
        Else
            chkSameAsId.Checked = False
            chkSameAsId.Visible = False
            lblAddressProof.Visible = False
            cmbAddressProf.Visible = False
        End If

    End Sub

    Private Sub chkSameAsId_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSameAsId.CheckedChanged
        If chkSameAsId.Checked Then
            lblAddressProof.Enabled = False
            cmbAddressProf.Enabled = False
        Else
            lblAddressProof.Enabled = True
            cmbAddressProf.Enabled = True
        End If
    End Sub

    Private Sub cmdAddMoreKyc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddMoreKyc.Click
        'Dim frmAddKyc As New frmAddmoreKyc
        'frmAddKyc.SetIdentity = CType(Me.cmbId.DataSource, DataTable)
        'frmAddKyc.UserId = UserId
        'frmAddKyc.CustomerID = Me.txtCustomerid.Text
        'frmAddKyc.BranchName = BranchName
        'frmAddKyc.ShowDialog()
    End Sub



    Dim dsnonKyc As New DataSet
    Private Sub rdoIndividual_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoIndividual.CheckedChanged
        Try
            If rdoIndividual.Focused AndAlso rdoIndividual.Checked Then
                'Dim pnt As New Drawing.Point(pnlKycSelection.Location.X, pnlKycSelection.Location.Y)
                'cmbKycNonIndividual.Visible = False
                'pnlKycSelection.Location = cmbKycNonIndividual.Location
                'cmbKycNonIndividual.Location = pnt
                cmbKycNonIndividual.Visible = False
                pnlKycType.Visible = True
                rdoRegularKYC.Checked = True

                Dim objser As New customerService.customer
                Dim dsdet As New DataSet
                dsdet = objser.GetNonKycDetails(10)

                If dsdet.Tables.Count > 0 Then
                    Me.cmbId.DataSource = Addhead(dsdet.Tables(0), dsdet.Tables(0).Columns(1).ColumnName, dsdet.Tables(0).Columns(0).ColumnName) 'Added for req 4829
                    Me.cmbId.ValueMember = dsdet.Tables(0).Columns(0).ColumnName
                    Me.cmbId.DisplayMember = dsdet.Tables(0).Columns(1).ColumnName
                End If

            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub rdoIndividualM_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoIndividualM.CheckedChanged
        Try
            If (rdoIndividualM.Focused AndAlso rdoIndividualM.Checked) Then
                'Dim pnt As New Drawing.Point(pnlKycSelectionM.Location.X, pnlKycSelectionM.Location.Y)
                'cmbKycNonIndividualM.Visible = False
                'pnlKycSelectionM.Location = cmbKycNonIndividualM.Location
                'cmbKycNonIndividualM.Location = pnt
                cmbKycNonIndividualM.Visible = False
                pnlKycTypeM.Visible = True
                rdoRegularKYCM.Checked = True

                Dim objser As New customerService.customer
                Dim dsdet As New DataSet
                dsdet = objser.GetNonKycDetails(10)

                If dsdet.Tables.Count > 0 Then
                    Me.cmbMid.DataSource = Addhead(dsdet.Tables(0), dsdet.Tables(0).Columns(1).ColumnName, dsdet.Tables(0).Columns(0).ColumnName) 'Added for req 4829
                    Me.cmbMid.ValueMember = dsdet.Tables(0).Columns(0).ColumnName
                    Me.cmbMid.DisplayMember = dsdet.Tables(0).Columns(1).ColumnName
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub rdoNonIndividual_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoNonIndividual.CheckedChanged
        Try
            If rdoNonIndividual.Focused AndAlso rdoNonIndividual.Checked Then
                'Dim pnt As New Drawing.Point(cmbKycNonIndividual.Location.X, cmbKycNonIndividual.Location.Y)
                'cmbKycNonIndividual.Visible = True
                'cmbKycNonIndividual.Location = pnlKycSelection.Location
                'pnlKycSelection.Location = pnt

                cmbKycNonIndividual.Visible = True
                pnlKycType.Visible = False

                If cmbKycNonIndividual.Items.Count = 0 Then
                    Dim objser As New customerService.customer
                    dsnonKyc = objser.GetNonKycType()
                    If dsnonKyc.Tables.Count > 0 Then
                        cmbKycNonIndividual.DataSource = Addhead(dsnonKyc.Tables(0), dsnonKyc.Tables(0).Columns(1).ColumnName, dsnonKyc.Tables(0).Columns(0).ColumnName)
                        cmbKycNonIndividual.DisplayMember = "description"
                        cmbKycNonIndividual.ValueMember = "status_id"
                    End If
                Else
                    cmbKycNonIndividual.SelectedIndex = 0
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Private Sub rdoNonIndividualM_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoNonIndividualM.CheckedChanged
        Try
            If (rdoNonIndividualM.Focused AndAlso rdoNonIndividualM.Checked) Then
                'Dim pnt As New Drawing.Point(cmbKycNonIndividualM.Location.X, cmbKycNonIndividualM.Location.Y)
                'cmbKycNonIndividualM.Visible = True
                'cmbKycNonIndividualM.Location = pnlKycSelectionM.Location
                'pnlKycSelectionM.Location = pnt
                cmbKycNonIndividualM.Visible = True
                pnlKycTypeM.Visible = False
                If cmbKycNonIndividualM.Items.Count = 0 Then
                    Dim objser As New customerService.customer
                    dsnonKyc = objser.GetNonKycType()
                    If dsnonKyc.Tables.Count > 0 Then
                        cmbKycNonIndividualM.DataSource = Addhead(dsnonKyc.Tables(0), dsnonKyc.Tables(0).Columns(1).ColumnName, dsnonKyc.Tables(0).Columns(0).ColumnName)
                        cmbKycNonIndividualM.DisplayMember = "description"
                        cmbKycNonIndividualM.ValueMember = "status_id"
                    End If
                Else
                    cmbKycNonIndividualM.SelectedIndex = 0
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cmbKycNonIndividual_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbKycNonIndividual.SelectedIndexChanged
        Try
            Me.Cursor = Cursors.WaitCursor
            Dim objser As New customerService.customer
            Dim dsdet As New DataSet
            dsdet = objser.GetNonKycDetails(cmbKycNonIndividual.SelectedIndex)

            If dsdet.Tables.Count > 0 Then
                Me.cmbId.DataSource = Addhead(dsdet.Tables(0), dsdet.Tables(0).Columns(1).ColumnName, dsdet.Tables(0).Columns(0).ColumnName) 'Added for req 4829
                Me.cmbId.ValueMember = dsdet.Tables(0).Columns(0).ColumnName
                Me.cmbId.DisplayMember = dsdet.Tables(0).Columns(1).ColumnName
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub cmbKycNonIndividualM_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbKycNonIndividualM.SelectedIndexChanged
        Try
            Me.Cursor = Cursors.WaitCursor
            Dim objser As New customerService.customer
            Dim dsdet As New DataSet
            dsdet = objser.GetNonKycDetails(cmbKycNonIndividualM.SelectedIndex)

            If dsdet.Tables.Count > 0 Then
                Me.cmbMid.DataSource = Addhead(dsdet.Tables(0), dsdet.Tables(0).Columns(1).ColumnName, dsdet.Tables(0).Columns(0).ColumnName) 'Added for req 4829
                Me.cmbMid.ValueMember = dsdet.Tables(0).Columns(0).ColumnName
                Me.cmbMid.DisplayMember = dsdet.Tables(0).Columns(1).ColumnName
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Dim tooltipCmb As New ToolTip


    Private Sub rdoRegularKYC_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoRegularKYC.CheckedChanged
        If rdoRegularKYC.Focused AndAlso rdoRegularKYC.Checked Then
            Try
                Me.Cursor = Cursors.WaitCursor
                Dim objser As New customerService.customer
                Dim dsdet As New DataSet
                dsdet = objser.GetNonKycDetails(10)

                If dsdet.Tables.Count > 0 Then
                    Me.cmbId.DataSource = Addhead(dsdet.Tables(0), dsdet.Tables(0).Columns(1).ColumnName, dsdet.Tables(0).Columns(0).ColumnName) 'Added for req 4829
                    Me.cmbId.ValueMember = dsdet.Tables(0).Columns(0).ColumnName
                    Me.cmbId.DisplayMember = dsdet.Tables(0).Columns(1).ColumnName
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End If
    End Sub

    Private Sub rdoInterimKyc_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoInterimKyc.CheckedChanged
        If rdoInterimKyc.Focused AndAlso rdoInterimKyc.Checked Then
            Try
                Me.Cursor = Cursors.WaitCursor
                Dim objser As New customerService.customer
                Dim dsdet As New DataSet
                dsdet = objser.GetNonKycDetails(20)

                If dsdet.Tables.Count > 0 Then
                    Me.cmbId.DataSource = Addhead(dsdet.Tables(0), dsdet.Tables(0).Columns(1).ColumnName, dsdet.Tables(0).Columns(0).ColumnName) 'Added for req 4829
                    Me.cmbId.ValueMember = dsdet.Tables(0).Columns(0).ColumnName
                    Me.cmbId.DisplayMember = dsdet.Tables(0).Columns(1).ColumnName



                    loadfill()
                    btnkycc.Visible = False
                    txtACustName.ReadOnly = False
                    txtACustName.Text = ""

                    rb_MaleAdd.Checked = False

                    rb_FemaleAdd.Checked = False
                    txtAFatHus.ReadOnly = False
                    txtAFatHus.Text = ""
                    txtHouse.ReadOnly = False
                    txtHouse.Text = ""
                    txtALocation.ReadOnly = False
                    txtALocation.Text = ""
                    cmbCountry.Enabled = True
                    cmbState.Enabled = True
                    cmbDistrict.Enabled = True

                    'cmbDistrict.Enabled = False
                    'Me.DateTimePicker3.Enabled = True
                    'Me.DateTimePicker3.Text = DateTime.Now.Date.ToString()
                    Me.txtEmail.ReadOnly = True

                    txtPincode.ReadOnly = True
                    txtAIdno.Text = ""
                    'txtAIdno.Focus()

                    'btnkycc.Visible = False
                    txtAIdno.Focus()
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End If
    End Sub

    Private Sub rdoRegularKYCM_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoRegularKYCM.CheckedChanged
        If rdoRegularKYCM.Focused AndAlso rdoRegularKYCM.Checked Then
            Try
                Me.Cursor = Cursors.WaitCursor
                Dim objser As New customerService.customer
                Dim dsdet As New DataSet
                dsdet = objser.GetNonKycDetails(10)

                If dsdet.Tables.Count > 0 Then
                    Me.cmbMid.DataSource = Addhead(dsdet.Tables(0), dsdet.Tables(0).Columns(1).ColumnName, dsdet.Tables(0).Columns(0).ColumnName) 'Added for req 4829
                    Me.cmbMid.ValueMember = dsdet.Tables(0).Columns(0).ColumnName
                    Me.cmbMid.DisplayMember = dsdet.Tables(0).Columns(1).ColumnName
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End If
    End Sub

    Private Sub rdoInterimKycM_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoInterimKycM.CheckedChanged
        If (rdoInterimKycM.Focused AndAlso rdoInterimKycM.Checked) Then
            Try
                Me.Cursor = Cursors.WaitCursor
                Dim objser As New customerService.customer
                Dim dsdet As New DataSet
                dsdet = objser.GetNonKycDetails(20)

                If dsdet.Tables.Count > 0 Then
                    Me.cmbMid.DataSource = Addhead(dsdet.Tables(0), dsdet.Tables(0).Columns(1).ColumnName, dsdet.Tables(0).Columns(0).ColumnName) 'Added for req 4829
                    Me.cmbMid.ValueMember = dsdet.Tables(0).Columns(0).ColumnName
                    Me.cmbMid.DisplayMember = dsdet.Tables(0).Columns(1).ColumnName
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End If
    End Sub

    Private Sub btnReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try
            ScanX1.ResetImg()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    'Private Sub btnStartKyc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Try
    '        wchelper.Container = Me.picAddKyc
    '        wchelper.Container.SizeMode = PictureBoxSizeMode.StretchImage
    '        wchelper.OpenConnection()
    '        wchelper.Load()
    '    Catch ex As Exception
    '        If ex.Message = "Object reference not set to an instance of an object." Then
    '            MsgBox("Check The web cam")
    '        End If
    '    End Try
    'End Sub

    'Private Sub btnStopKyc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    wchelper.Dispose()
    'End Sub

    'Private Sub btnExitKyc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    If kycStatus = 1 Then
    '        wchelper.SaveImage()
    '        kycPhoto = wchelper.get_data()
    '    Else
    '        Dim message As String
    '        Try
    '            Me.Cursor = Cursors.WaitCursor
    '            wchelper.SaveImage()
    '            kycPhoto = wchelper.get_data()
    '            If kycPhoto Is Nothing Then
    '                '  Me.picSearchdtl.Image = Nothing
    '            Else
    '                Dim ws As New customerService.customer(set_ip)
    '                message = ws.AddCustomerPhoto(Me.txtCustomerid.Text, kycPhoto, 2)
    '                Me.picAddKyc.SizeMode = PictureBoxSizeMode.StretchImage
    '            End If
    '            Me.Main_tab.SelectedTab = Me.tb_search
    '            Me.picAddKyc.Image = Nothing
    '        Catch ex As Exception 'added for req 4829
    '            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        Finally
    '            Me.Cursor = Cursors.Default
    '        End Try
    '    End If
    'End Sub

    'Added by george for neft scan
    Private Function ScanNeftImage() As Boolean
        Dim err As ActiveScanner.ScanX.ErrorCode
        neftScanCtl.SetAnnotation("Scanned by " & UserId & " from " & BranchName & " on " & Now.ToString("dd-MMM-yyyy hh:mm tt"), 0, 0, "", 8, False, 0, 0, 0)
        neftScanCtl.SetFileName("neftdocument")
        neftScanCtl.ScanPage = 1
        neftScanCtl.SaveImageFormat = ActiveScanner.ScanX.ImageFormat.JPG
        'If cmbScanMode.Text = "Block & White" Then
        '    neftScanCtl.ColorMode = ActiveScanner.ScanX.ColorModeType.BLACK_WHITE
        '    err = neftScanCtl.ScanDocument(ActiveScanner.ScanX.ColorModeType.BLACK_WHITE, 0, 100, 0, 0, 0, 0)
        'Else
        neftScanCtl.ColorMode = ActiveScanner.ScanX.ColorModeType.COLOR
        err = neftScanCtl.ScanDocument(ActiveScanner.ScanX.ColorModeType.COLOR, 0, 120, 0, 0, 0, 0)
        'End If

        If err = ActiveScanner.ScanX.ErrorCode.SUCCESS Then
            Return True
        Else
            Return False
        End If

    End Function

    Private Sub btnNeftSource_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNeftSource.Click
        neftScanCtl.SelectSource()
    End Sub

    Private Sub tb_neftdtl_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles tb_neftdtl.Leave
        neft_status = 0
    End Sub
    Private Sub LoadNEFTPhoto(ByVal custid As String)
        Try
            Dim sql As String = "select id_proof from neft_customer where cust_id='" & custid & "'"
            Dim ds As New Data.DataSet
            Dim ws As New customerService.customer(set_ip)
            ws.Timeout = 3 * 1000 * 60
            ds = ws.QueryResult(sql)
            If ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 AndAlso Not IsDBNull(ds.Tables(0).Rows(0)(0)) Then
                Dim binary As Byte()
                binary = CType(ds.Tables(0).Rows(0)(0), Byte())
                Dim neftdbfile As String = Path.GetTempFileName & "ASNeft.jpg"
                Dim fs As New FileStream(neftdbfile, FileMode.OpenOrCreate, FileAccess.Write)
                fs.Write(binary, 0, binary.Length)
                fs.Close()
                fs.Dispose()
                neftScanCtl.ImageFileLoad(neftdbfile)
            End If
        Catch ex As Exception
            MessageBox.Show("LoadNEFTPhoto : " & ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub rdoPan_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoPan.CheckedChanged
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.lbl_searchnm.Text = "PAN Number"
            Me.txt_search.MaxLength = 10
            Dim dt As New DataTable
            dt.Clear()
            Me.dgsearchResult.DataSource = dt
            clear()
            Me.txt_search.Focus()
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub tb_addKyc_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles tb_addKyc.Leave
        kycStatus = 0
    End Sub

    Private Sub txtMIdno_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMIdno.Leave
        If custBranchID <> BranchID Then
            If idno <> txtMIdno.Text Then
                ' MessageBox.Show("Can't modify other branch customer's KYC", "Customer", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Dim result = MessageBox.Show("You are trying to modify other branch customer's KYC document..It will Requie SRM Approval. Really want to Modify?", "Confirm", MessageBoxButtons.YesNo)
                If result = DialogResult.No Then
                    txtMIdno.Text = idno
                End If
            End If
        End If
    End Sub

    Private Sub rbtMtKyc_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtMtKyc.CheckedChanged
        If rbtMtKyc.Focused AndAlso rbtMtKyc.Checked Then
            Try
                Me.Cursor = Cursors.WaitCursor
                Dim objser As New customerService.customer
                Dim dsdet As New DataSet
                dsdet = objser.GetNonKycDetails(16)

                If dsdet.Tables.Count > 0 Then
                    Me.cmbId.DataSource = Addhead(dsdet.Tables(0), dsdet.Tables(0).Columns(1).ColumnName, dsdet.Tables(0).Columns(0).ColumnName) 'Added for req 4829
                    Me.cmbId.ValueMember = dsdet.Tables(0).Columns(0).ColumnName
                    Me.cmbId.DisplayMember = dsdet.Tables(0).Columns(1).ColumnName

                    loadfill()
                    btnkycc.Visible = False
                    txtACustName.ReadOnly = False
                    txtACustName.Text = ""

                    rb_MaleAdd.Checked = False

                    rb_FemaleAdd.Checked = False
                    txtAFatHus.ReadOnly = False
                    txtAFatHus.Text = ""
                    txtHouse.ReadOnly = False
                    txtHouse.Text = ""
                    txtALocation.ReadOnly = False
                    txtALocation.Text = ""
                    cmbCountry.Enabled = True
                    cmbState.Enabled = True
                    cmbDistrict.Enabled = True

                    'cmbDistrict.Enabled = False
                    'Me.DateTimePicker3.Enabled = True
                    'Me.DateTimePicker3.Text = DateTime.Now.Date.ToString()
                    Me.txtEmail.ReadOnly = True

                    txtPincode.ReadOnly = True
                    txtAIdno.Text = ""
                    'txtAIdno.Focus()

                    'btnkycc.Visible = False
                    txtAIdno.Focus()
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End If
    End Sub

    Private Sub txtMCustnm_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMCustnm.KeyPress
        e.KeyChar = CChar(e.KeyChar.ToString.ToUpper)
    End Sub

    Private Sub txtMCustnm_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMCustnm.KeyUp
        checkForString(e.KeyCode, Me.txtMCustnm)
    End Sub

    Private Sub txtPBCustnm_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPBCustnm.KeyPress
        e.KeyChar = CChar(e.KeyChar.ToString.ToUpper)
    End Sub

    Private Sub txtPBCustnm_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPBCustnm.KeyUp
        checkForString(e.KeyCode, Me.txtPBCustnm)
    End Sub

    Private Sub txtPBCustnm_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPBCustnm.Enter
        Dim tt As New TextBox
        tt = sender
        Dim tb As New ToolTip()
        tb.ShowAlways = True
        tb.Show("Please enter customer Name as per Passbook.", tt)
    End Sub


    'Private Sub tab_BrokerInfo_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Try
    '        CheckForOnlineCustomer()
    '        Dim dt As DataTable
    '        Dim asset() As String
    '        Dim liab() As String
    '        Me.Cursor = Cursors.WaitCursor
    '        If Me.txtCustomerid.Text.Trim <> "" Then
    '            txtbrkcustomerid.Text = txtCustomerid.Text
    '            txtbrkcustomername.Text = txtcustname.Text
    '            Dim objService As New customerService.customer("")
    '            dt = objService.PawnBrokerInfoSelect("fetch", txtbrkcustomerid.Text, "NA", "NA", "NA", "NA")
    '            If dt.Rows.Count > 0 Then
    '                txtbrklicense.Text = dt.Rows(0)(0).ToString()
    '                dtIssuedate.Value = dt.Rows(0)(1).ToString()
    '                dtExpirydate.Value = dt.Rows(0)(2).ToString()
    '                txtbkrltv.Text = dt.Rows(0)(3).ToString()
    '                asset = dt.Rows(0)(4).ToString().Split("~")
    '                liab = dt.Rows(0)(5).ToString().Split("~")
    '                txtnetworth.Text = dt.Rows(0)(6).ToString()

    '                txtasst_bkdeposit.Text = CInt(asset(0))
    '                txtasst_OtherDeposit.Text = CInt(asset(1))
    '                txtasst_FinAsset.Text = CInt(asset(2))
    '                txtasst_LPUV.Text = CInt(asset(3))
    '                txtasst_imasset.Text = CInt(asset(4))
    '                txtasst_vehicleval.Text = CInt(asset(5))
    '                txtasset_chittypaidup.Text = CInt(asset(6))
    '                txtasset_goldinhand.Text = CInt(asset(7))

    '                txtliab_bankloan.Text = CInt(liab(0))
    '                txtliab_otherloan.Text = CInt(liab(1))
    '                txtliab_otherborrow.Text = CInt(liab(2))
    '                txtliab_chitliability.Text = CInt(liab(3))

    '                licimg = dt.Rows(0)(7).ToString()
    '                formimg = dt.Rows(0)(8).ToString()
    '            Else

    '                txtasst_bkdeposit.Text = 0
    '                txtasst_OtherDeposit.Text = 0
    '                txtasst_FinAsset.Text = 0
    '                txtasst_LPUV.Text = 0
    '                txtasst_imasset.Text = 0
    '                txtasst_vehicleval.Text = 0
    '                txtasset_chittypaidup.Text = 0
    '                txtasset_goldinhand.Text = 0

    '                txtliab_bankloan.Text = 0
    '                txtliab_otherloan.Text = 0
    '                txtliab_otherborrow.Text = 0
    '                txtliab_chitliability.Text = 0

    '                txtbrklicense.Text = String.Empty
    '                dtIssuedate.Value = Date.Today
    '                dtExpirydate.Value = Date.Today
    '                txtbkrltv.Text = 0
    '                txtnetworth.Text = 0
    '            End If
    '        Else
    '            MsgBox("Search Customer Details")
    '            Me.Main_tab.SelectedTab = Me.tb_search
    '        End If
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    Finally
    '        Me.Cursor = Cursors.Default
    '    End Try
    'End Sub

    'Private Sub btn_cnfBrokerinfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim result As String
    '    Try
    '        If txtbrklicense.Text = String.Empty Then
    '            MessageBox.Show("Please Enter License Number", "Customer", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '            Exit Sub
    '        End If

    '        If CDate(dtIssuedate.Value) > Now Then
    '            MessageBox.Show("Issue date should not be future date", "Customer", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '            Exit Sub
    '        End If

    '        If CDate(dtExpirydate.Value) < Now Then
    '            MessageBox.Show("Expiry date should not be past date", "Customer", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '            Exit Sub
    '        End If

    '        If CDate(dtExpirydate.Value) = CDate(dtIssuedate.Value) Then
    '            MessageBox.Show("Issue date and Expiry date should not be same", "Customer", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '            Exit Sub
    '        End If

    '        If CDate(dtExpirydate.Value) < CDate(dtIssuedate.Value) Then
    '            MessageBox.Show("Issue date should not greater than Expiry date", "Customer", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '            Exit Sub
    '        End If

    '        'If CInt(txtbkrltv.Text) = 0 Then
    '        '    MessageBox.Show("Please enter the LTV value", "Customer", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '        '    txtbkrltv.Focus()
    '        '    Exit Sub
    '        'End If

    '        If Not IsNothing(licensePhoto) AndAlso Not IsNothing(brokerform) Then
    '            Dim objService As New customerService.customer("")
    '            Dim asset As String = txtasst_bkdeposit.Text & "~" & txtasst_OtherDeposit.Text & "~" & txtasst_FinAsset.Text & "~" & txtasst_LPUV.Text & "~" & txtasst_imasset.Text & "~" & txtasst_vehicleval.Text & "~" & txtasset_chittypaidup.Text & "~" & txtasset_goldinhand.Text
    '            Dim liab As String = txtliab_bankloan.Text & "~" & txtliab_otherloan.Text & "~" & txtliab_otherborrow.Text & "~" & txtliab_chitliability.Text
    '            If licimg = "true" AndAlso formimg = "true" Then
    '                result = objService.PawnBrokerInfo("updtentry", txtbrkcustomerid.Text, txtbrklicense.Text, dtIssuedate.Text, dtExpirydate.Text, txtbkrltv.Text, asset, liab, txtnetworth.Text, User_id)
    '            Else
    '                result = objService.PawnBrokerInfo("entry", txtbrkcustomerid.Text, txtbrklicense.Text, dtIssuedate.Text, dtExpirydate.Text, txtbkrltv.Text, asset, liab, txtnetworth.Text, User_id)
    '            End If
    '            objService.AddAdditionalKycPhoto(txtbrkcustomerid.Text, licensePhoto, 1)
    '            objService.AddAdditionalKycPhoto(txtbrkcustomerid.Text, brokerform, 2)
    '            licensePhoto = Nothing
    '            brokerform = Nothing
    '            MessageBox.Show(result, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '        Else
    '            If licimg = "true" AndAlso formimg = "true" Then
    '                If (MessageBox.Show("Please scan the document & license before confirming. Click OK to use Existing Records ", "Customer", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) = DialogResult.OK) Then
    '                    Dim objService As New customerService.customer("")
    '                    Dim asset As String = txtasst_bkdeposit.Text & "~" & txtasst_OtherDeposit.Text & "~" & txtasst_FinAsset.Text & "~" & txtasst_LPUV.Text & "~" & txtasst_imasset.Text & "~" & txtasst_vehicleval.Text & "~" & txtasset_chittypaidup.Text & "~" & txtasset_goldinhand.Text
    '                    Dim liab As String = txtliab_bankloan.Text & "~" & txtliab_otherloan.Text & "~" & txtliab_otherborrow.Text & "~" & txtliab_chitliability.Text
    '                    result = objService.PawnBrokerInfo("reentry", txtbrkcustomerid.Text, txtbrklicense.Text, dtIssuedate.Text, dtExpirydate.Text, txtbkrltv.Text, asset, liab, txtnetworth.Text, User_id)
    '                    MessageBox.Show(result, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '                End If
    '            Else
    '                MessageBox.Show("There are no existing scanned documents for license and form. Please scan to complete")
    '            End If
    '        End If
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message)
    '    End Try
    'End Sub

    Private Sub btn_scan_license_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim frmAddKyc As New frmAddmoreKyc
        frmAddKyc.SetIdentity = 1
        frmAddKyc.UserId = UserId
        frmAddKyc.CustomerID = Me.txtCustomerid.Text
        frmAddKyc.BranchName = BranchName
        frmAddKyc.ShowDialog()
        licensePhoto = frmAddKyc.Photo
    End Sub

    Private Sub btn_scan_form_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim frmAddKyc As New frmAddmoreKyc
        frmAddKyc.SetIdentity = 2
        frmAddKyc.UserId = UserId
        frmAddKyc.CustomerID = Me.txtCustomerid.Text
        frmAddKyc.BranchName = BranchName
        frmAddKyc.ShowDialog()
        brokerform = frmAddKyc.Photo
    End Sub

    Private Sub txtasst_bkdeposit_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'checkForNumerical(e.KeyCode, txtasst_bkdeposit)
    End Sub

    Private Sub txtasst_OtherDeposit_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'If Not checkForNumerical(e.KeyCode, txtasst_OtherDeposit) = False Then
        '    txtasst_OtherDeposit.Text = "0"
        'End If
    End Sub

    Private Sub txtasst_LPUV_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'If Not checkForNumerical(e.KeyCode, txtasst_LPUV) = False Then
        '    txtasst_LPUV.Text = "0"
        'End If
    End Sub

    Private Sub txtasst_FinAsset_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'If Not checkForNumerical(e.KeyCode, txtasst_FinAsset) = False Then
        '    txtasst_FinAsset.Text = "0"
        'End If
    End Sub

    Private Sub txtasst_imasset_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'If Not checkForNumerical(e.KeyCode, txtasst_imasset) = False Then
        '    txtasst_imasset.Text = "0"
        'End If
    End Sub

    Private Sub txtasst_vehicleval_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'If Not checkForNumerical(e.KeyCode, txtasst_vehicleval) = False Then
        '    txtasst_vehicleval.Text = "0"
        'End If
    End Sub

    Private Sub txtliab_bankloan_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'If Not checkForNumerical(e.KeyCode, txtliab_bankloan) = False Then
        '    txtliab_bankloan.Text = "0"
        'End If
    End Sub

    Private Sub txtliab_otherloan_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'If Not checkForNumerical(e.KeyCode, txtliab_otherloan) = False Then
        '    txtliab_otherloan.Text = "0"
        'End If
    End Sub

    Private Sub txtliab_chitliability_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'If Not checkForNumerical(e.KeyCode, txtliab_chitliability) = False Then
        '    txtliab_chitliability.Text = "0"
        'End If
    End Sub

    Private Sub txtliab_otherborrow_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'If Not checkForNumerical(e.KeyCode, txtliab_otherborrow) = False Then
        '    txtliab_otherborrow.Text = "0"
        'End If
    End Sub

    Private Sub txtbkrltv_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'If Not checkForNumerical(e.KeyCode, txtbkrltv) = False Then
        '    txtbkrltv.Text = "0"
        'End If
    End Sub


    Private Sub txtasset_chittypaidup_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'If Not checkForNumerical(e.KeyCode, txtasset_chittypaidup) = False Then
        '    txtasset_chittypaidup.Text = "0"
        'End If
    End Sub

    Private Sub txtasset_goldinhand_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'If Not checkForNumerical(e.KeyCode, txtasset_goldinhand) = False Then
        '    txtasset_goldinhand.Text = "0"
        'End If
    End Sub

    Private Sub Calculate_NetWorth(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim asset_networth, liab_networth As Integer
        'If Not (IsNumeric(txtasst_bkdeposit.Text) AndAlso IsNumeric(txtasst_OtherDeposit.Text) AndAlso IsNumeric(txtasset_chittypaidup.Text) AndAlso IsNumeric(txtasset_goldinhand.Text) AndAlso IsNumeric(txtasst_FinAsset.Text) AndAlso IsNumeric(txtasst_imasset.Text) AndAlso IsNumeric(txtasst_LPUV.Text) AndAlso IsNumeric(txtasst_vehicleval.Text) AndAlso IsNumeric(txtliab_bankloan.Text) AndAlso IsNumeric(txtliab_chitliability.Text) AndAlso IsNumeric(txtliab_otherborrow.Text) AndAlso IsNumeric(txtliab_otherloan.Text)) Then
        '    Exit Sub
        'End If
        'asset_networth = CInt(txtasst_bkdeposit.Text) + CInt(txtasst_FinAsset.Text) + CInt(txtasst_LPUV.Text) + CInt(txtasst_OtherDeposit.Text) + CInt(txtasst_vehicleval.Text) + CInt(txtasst_imasset.Text) + CInt(txtasset_chittypaidup.Text) + CInt(txtasset_goldinhand.Text)
        'liab_networth = CInt(txtliab_bankloan.Text) + CInt(txtliab_chitliability.Text) + CInt(txtliab_otherborrow.Text) + CInt(txtliab_otherloan.Text)
        'txtnetworth.Text = asset_networth - liab_networth
    End Sub

    Private Sub txtbrklicense_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        e.KeyChar = CChar(e.KeyChar.ToString.ToUpper)
    End Sub
    Private MyWebsevice As New customerService.customer(set_ip)

    Private Sub txt_AddMobile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_AddMobile.Click

        Dim objMobileOTP As New MobileOTP(UserId, txtEmpCode.Text, "NEW", txtMobileNo.Text)
        Dim strResult As String = String.Empty
        If objMobileOTP.ShowDialog() = DialogResult.Yes Then
            ' If MobileOTP.strMobileNumOTP.Length > 0 Then
            'strResult = MyWebsevice.CheckForMobileDuplication(MobileOTP.strMobileNumOTP)
            'End If
            'If strResult <= 1 Then
            txtMobileNo.Text = MobileOTP.strMobileNumOTP
            'Else
            'MsgBox("Same phonenumber already used by Other Customers", vbCritical + vbOKOnly)
            'End If
        End If

        If MobileOTP.strMobileNumOTP.Length > 0 Then
            strResult = MyWebsevice.CheckForEmpReference(MobileOTP.strMobileNumOTP, 0)
        End If
        If strResult.Length > 1 Then
            If strResult = 1 Then
                cmbCMediatype.SelectedValue = "2"
                If Me.cmbCMediatype.Items.Count > 0 Then
                    Mediafill(Me.cmbCMediatype.SelectedValue)
                End If
                cmbCMedia.SelectedValue = "9"
                cmbCMediatype.Enabled = False
                cmbCMedia.Enabled = False
            Else
                cmbCMediatype.Enabled = True
                cmbCMedia.Enabled = True
            End If
        End If


    End Sub

    Private Sub btn_ChangeMob_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_ChangeMob.Click
        Dim objMobileOTP As New MobileOTP(UserId, TextBox21.Text, "CHANGE", txtMMobno.Text)
        If objMobileOTP.ShowDialog() = DialogResult.Yes Then
            txtMMobno.Text = MobileOTP.strMobileNumOTP
            txtMMobno.Modified = True
        End If
    End Sub

    Private Sub btn_AddRef_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_AddRef.Click
        Dim objRef As New ReferenceDetails(UserId, 1, "")
        objRef.ShowDialog()
    End Sub

    Private Sub bn_ModifyReference_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bn_ModifyReference.Click
        Dim objRef As New ReferenceDetails(UserId, 2, txtMCustid.Text.Trim())
        ReferenceDetails.MobNo = txtMobileNo.Text
        ReferenceDetails.LandNo = txtPhcode.Text & txtAPhoneno.Text
        objRef.ShowDialog()
    End Sub
    Public Sub SaveCustomerRefDetails(ByVal strCustID As String)
        Dim strDetails As String = String.Empty
        Dim strRefDetails As String = String.Empty
        Dim strMessage As String = String.Empty
        If Not IsNothing(Refdetails) AndAlso Refdetails.Rows.Count > 0 Then
            Dim i As Integer = 0
            For i = 0 To Refdetails.Rows.Count - 1
                strDetails = Refdetails.Rows(i)(0) & "*" & Refdetails.Rows(i)(1) & "*" & Refdetails.Rows(i)(2)
                If strDetails.Length > 0 Then
                    If strRefDetails.Length = 0 Then
                        strRefDetails = strDetails & "~"
                    Else
                        strRefDetails = strRefDetails & strDetails & "~"
                    End If
                End If
            Next
            strMessage = MyWebsevice.SaveRefDetails("REFDETAILS", strCustID, UserId, "", "", strRefDetails.TrimEnd("~"))
            If strMessage.Length > 0 Then
                'MsgBox(strMessage)
                ReferenceDetails.strRefFlag = "COMPLETED"
                Refdetails = Nothing
            End If
        End If
    End Sub
    Public Sub SaveCustomerMobOTPDetails(ByVal strCustID As String, ByVal strMob As String, ByVal strOTP As String)
        Dim strMessage As String = String.Empty
        strMessage = MyWebsevice.SaveRefDetails("MOBDETAILS", strCustID, UserId, strMob, strOTP, "")
    End Sub
    ' ---------------------------Online Customer Reg Details Start--------------------------
    Private Sub btn_GetOnlineCustomerDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_GetOnlineCustomerDetails.Click
        If txt_OnlineRegId.Text.Trim().Length > 0 Then
            'AndAlso txt_OnlineRegId.Text.Trim().Length = 16
            BindCustomerOnlineRegDetails()
        Else
            MsgBox("Enter a valid Customer Reg ID")
        End If
    End Sub
    Public Sub BindCustomerOnlineRegDetails()
        Dim strPhone As String = String.Empty
        Dim dtCustomerRegInfo As New DataTable
        Dim dsCustomerRegInfo As New DataSet
        Try
            dsCustomerRegInfo = MyWebsevice.GetCustomerRegDetails(txt_OnlineRegId.Text.Trim())
            If dsCustomerRegInfo.Tables.Count > 0 Then
                dtCustomerRegInfo = dsCustomerRegInfo.Tables(0)
                If dtCustomerRegInfo.Rows.Count > 0 Then
                    If Not dtCustomerRegInfo.Rows(0)(1) Is DBNull.Value Then
                        txtACustName.Text = dtCustomerRegInfo.Rows(0)(1).ToString()
                    Else
                        txtACustName.Text = String.Empty
                    End If
                    If Not dtCustomerRegInfo.Rows(0)(3) Is DBNull.Value Then
                        If dtCustomerRegInfo.Rows(0)(3).ToString() = "1" Then
                            cmbPre.SelectedIndex = 0
                            rb_MaleAdd.Checked = True
                        ElseIf dtCustomerRegInfo.Rows(0)(3).ToString() = "2" Then
                            cmbPre.SelectedIndex = 1
                            rb_FemaleAdd.Checked = True
                        End If
                    Else
                        cmbPre.SelectedValue = 0
                    End If
                    If Not dtCustomerRegInfo.Rows(0)(2) Is DBNull.Value Then
                        txtAFatHus.Text = dtCustomerRegInfo.Rows(0)(2).ToString()
                    Else
                        txtAFatHus.Text = String.Empty
                    End If
                    If Not dtCustomerRegInfo.Rows(0)(5) Is DBNull.Value Then
                        txtdob.Text = dtCustomerRegInfo.Rows(0)(5).ToString()
                    Else
                        txtdob.Text = Date.Now()
                    End If
                    If Not dtCustomerRegInfo.Rows(0)(8) Is DBNull.Value Then
                        txtHouse.Text = dtCustomerRegInfo.Rows(0)(8).ToString()
                    Else
                        txtHouse.Text = String.Empty
                    End If
                    If Not dtCustomerRegInfo.Rows(0)(9) Is DBNull.Value Then
                        txtALocation.Text = dtCustomerRegInfo.Rows(0)(9).ToString()
                    Else
                        txtALocation.Text = String.Empty
                    End If
                    cmbCountry.SelectedValue = 1
                    If Not dtCustomerRegInfo.Rows(0)(12) Is DBNull.Value Then
                        cmbState.SelectedValue = dtCustomerRegInfo.Rows(0)(12).ToString()
                        If Not cmbState.SelectedValue Is Nothing Then
                            If Not dtCustomerRegInfo.Rows(0)(13) Is DBNull.Value Then
                                cmbDistrict.SelectedValue = dtCustomerRegInfo.Rows(0)(13).ToString()
                                If Not cmbDistrict.SelectedValue Is Nothing Then
                                    If Not dtCustomerRegInfo.Rows(0)(11) Is DBNull.Value Then
                                        cmbPost.SelectedValue = dtCustomerRegInfo.Rows(0)(11).ToString().Trim() & "@" & dtCustomerRegInfo.Rows(0)(14).ToString().Trim()
                                    Else
                                        cmbPost.SelectedValue = 0
                                    End If
                                End If
                            Else
                                cmbDistrict.SelectedValue = 0
                            End If
                        End If
                    Else
                        cmbState.SelectedValue = 0
                    End If
                    If Not dtCustomerRegInfo.Rows(0)(22) Is DBNull.Value Then
                        strPhone = dtCustomerRegInfo.Rows(0)(22).ToString()
                        If strPhone.Length = 11 Then
                            txtPhcode.Text = strPhone.Substring(0, 4)
                            txtAPhoneno.Text = strPhone.Substring(4, 7)
                        End If
                    Else
                        txtAPhoneno.Text = String.Empty
                    End If
                    If Not dtCustomerRegInfo.Rows(0)(24) Is DBNull.Value Then
                        If dtCustomerRegInfo.Rows(0)(24).ToString().Length = 11 Then
                            txtMobileNo.Text = dtCustomerRegInfo.Rows(0)(24).ToString()
                        ElseIf dtCustomerRegInfo.Rows(0)(24).ToString().Length = 10 Then
                            txtMobileNo.Text = "0" & dtCustomerRegInfo.Rows(0)(24).ToString()
                        End If
                    Else
                        txtMobileNo.Text = String.Empty
                    End If
                    If Not dtCustomerRegInfo.Rows(0)(25) Is DBNull.Value Then
                        txtEmail.Text = dtCustomerRegInfo.Rows(0)(25).ToString()
                    Else
                        txtEmail.Text = String.Empty
                    End If
                    If Not dtCustomerRegInfo.Rows(0)(6) Is DBNull.Value Then
                        cmbReligion.SelectedValue = dtCustomerRegInfo.Rows(0)(6).ToString()
                        If Not cmbReligion.SelectedValue Is Nothing Then
                            castefill(cmbReligion.SelectedValue)
                        End If
                    Else
                        cmbReligion.SelectedValue = 0
                    End If
                    If Not dtCustomerRegInfo.Rows(0)(7) Is DBNull.Value Then
                        cmbCaste.SelectedValue = dtCustomerRegInfo.Rows(0)(7).ToString()
                    Else
                        cmbCaste.SelectedValue = 0
                    End If
                    If Not dtCustomerRegInfo.Rows(0)(36) Is DBNull.Value Then
                        cmbCMediatype.SelectedValue = dtCustomerRegInfo.Rows(0)(36).ToString()
                        If Not cmbCMediatype.SelectedValue Is Nothing Then
                            Mediafill(Me.cmbCMediatype.SelectedValue)
                        End If
                    Else
                        cmbCMediatype.SelectedValue = 0
                    End If
                    If Not dtCustomerRegInfo.Rows(0)(37) Is DBNull.Value Then
                        cmbCMedia.SelectedValue = dtCustomerRegInfo.Rows(0)(37).ToString()
                    Else
                        cmbCMedia.SelectedValue = 0
                    End If

                    If Not dtCustomerRegInfo.Rows(0)(33) Is DBNull.Value Then
                        cmbId.SelectedValue = dtCustomerRegInfo.Rows(0)(33).ToString()
                    Else
                        cmbId.SelectedValue = 0
                    End If
                    'If cmbId.SelectedValue = 14 Then
                    '    If Not dtCustomerRegInfo.Rows(0)(34) Is DBNull.Value Then
                    '        txtPanNo.Text = dtCustomerRegInfo.Rows(0)(34).ToString()
                    '    Else
                    '        txtPanNo.Text = String.Empty
                    '    End If
                    'End If      
                    If cmbCountry.SelectedValue Is Nothing Then
                        cmbCountry.SelectedIndex = 0
                    End If
                    If cmbState.SelectedValue Is Nothing Then
                        cmbState.SelectedIndex = 0
                    End If
                    'If cmbDistrict.SelectedValue Is Nothing Then
                    '    cmbDistrict.SelectedIndex = 0
                    'End If
                    'If cmbPost.SelectedValue Is Nothing Then
                    ' cmbPost.SelectedIndex = 0
                    'End If
                    Dim altadress As New Adress2(3, txtHouse.Text, txtALocation.Text, cmbCountry.SelectedValue, cmbState.SelectedValue, cmbDistrict.SelectedValue, cmbPost.SelectedValue, txtPincode.Text) ' 1 to indicate  new customer 

                    If Not dtCustomerRegInfo.Rows(0)(16) Is DBNull.Value Then
                        altadress.txtHouse.Text = dtCustomerRegInfo.Rows(0)(16).ToString()
                    Else
                        altadress.txtHouse.Text = String.Empty
                    End If
                    If Not dtCustomerRegInfo.Rows(0)(17) Is DBNull.Value Then
                        altadress.txtALocation.Text = dtCustomerRegInfo.Rows(0)(17).ToString()
                    Else
                        altadress.txtALocation.Text = String.Empty
                    End If

                    altadress.cmbCountry.SelectedValue = 1
                    If Not dtCustomerRegInfo.Rows(0)(19) Is DBNull.Value Then
                        altadress.cmbState.SelectedValue = dtCustomerRegInfo.Rows(0)(19).ToString()
                        If Not altadress.cmbState.SelectedValue Is Nothing Then
                            If Not dtCustomerRegInfo.Rows(0)(20) Is DBNull.Value Then
                                altadress.cmbDistrict.SelectedValue = dtCustomerRegInfo.Rows(0)(20).ToString()
                                If Not altadress.cmbDistrict.SelectedValue Is Nothing Then
                                    If Not dtCustomerRegInfo.Rows(0)(18) Is DBNull.Value Then
                                        altadress.cmbPost.SelectedValue = dtCustomerRegInfo.Rows(0)(18).ToString().Trim() & "@" & dtCustomerRegInfo.Rows(0)(21).ToString().Trim()
                                    Else
                                        altadress.cmbPost.SelectedValue = 0
                                    End If
                                End If
                            Else
                                altadress.cmbDistrict.SelectedValue = 0
                            End If
                        End If
                    Else
                        altadress.cmbState.SelectedValue = 0
                    End If
                    altadress.ShowDialog()
                    txt_OnlineRegId.Text = String.Empty
                Else
                    MsgBox("Customer with this ID does not exist")
                End If
            End If
        Catch ex As Exception
            Throw New ApplicationException("Customer Online Reg.Details Error:- " & ex.Message)
        End Try
    End Sub

    'Added by Tijo Chacko
    Private Sub rdoPhone_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoPhone.CheckedChanged
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.lbl_searchnm.Text = "Phone Number"
            Me.txt_search.MaxLength = 20
            Dim dt As New DataTable
            dt.Clear()
            Me.dgsearchResult.DataSource = dt
            clear()
            Me.txt_search.Focus()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    'Added by Tijo Chacko
    Private Sub rdoIDNo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoIDNo.CheckedChanged
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.lbl_searchnm.Text = "ID Number"
            Me.txt_search.MaxLength = 20
            Dim dt As New DataTable
            dt.Clear()
            Me.dgsearchResult.DataSource = dt
            clear()
            Me.txt_search.Focus()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub txtACustName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtACustName.TextChanged
        '###------ SEBIN001 -----------------------------------------------------------
        'If Me.cmbPre.SelectedValue Is Nothing Or Me.cmbPre.SelectedValue = "-1~0" Then
        '    MsgBox("Please Select the title(MR,MRS,MISS")
        '    Me.cmbPre.Focus()
        '    flag = 0
        '    txtACustName.Text = ""
        '    Exit Sub
        'End If
        Dim t As TextBox = DirectCast(sender, TextBox)
        Dim index As Integer = t.Text.IndexOf("  ")
        While index <> -1
            t.Text = t.Text.Replace("  ", " ")
            index = t.Text.IndexOf("  ")
        End While
        t.SelectionStart = t.Text.Length
    End Sub

    Private Sub txtAFatHus_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAFatHus.TextChanged

        'If Me.cmbiden.SelectedValue Is Nothing Or Me.cmbiden.SelectedValue = 0 Then
        '    MsgBox("Please Select the title (S/O,D/O,W/O)")
        '    Me.cmbiden.Focus()
        '    flag = 0
        '    txtAFatHus.Text = ""
        '    Exit Sub
        'End If
        Dim t As TextBox = DirectCast(sender, TextBox)
        Dim index As Integer = t.Text.IndexOf("  ")
        While index <> -1
            t.Text = t.Text.Replace("  ", " ")
            index = t.Text.IndexOf("  ")
        End While
        t.SelectionStart = t.Text.Length
    End Sub
    Private Sub txtAIdno_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtAIdno.Validating
        If cmbId.SelectedValue = 14 Then
            ValidatePAN(txtAIdno.Text, txtAIdno)
        ElseIf cmbId.SelectedValue = 1 OrElse cmbId.SelectedValue = 503 OrElse cmbId.SelectedValue = 553 Then
            If txtAIdno.Text.Length > 7 Or txtAIdno.Text.Length < 10 Then
                If cmbCountry.SelectedIndex <> 0 Then
                    If cmbCountry.SelectedIndex = 1 Then
                        ValidatePassport(txtAIdno.Text, txtAIdno)
                    End If
                Else
                    MsgBox("Please Select country first", vbCritical + vbOKOnly)
                    txtAIdno.Clear()
                End If
            Else
                MsgBox("Invalid Passport!", vbCritical + vbOKOnly)
            End If
        ElseIf cmbId.SelectedValue = 16 OrElse cmbId.SelectedValue = 505 OrElse cmbId.SelectedValue = 555 Then
            ValidateUIDAI(txtAIdno.Text, txtAIdno)
        End If
    End Sub
#Region "ValidatePanID"

    Function ValidatePAN(ByVal PAN As String, ByRef textBox As TextBox) As Boolean
        Dim Pos As Integer
        Dim Validated As Boolean
        Dim Passed As Boolean
        Passed = True
        For Pos = 1 To Len(PAN)
            Select Case Pos
                Case 1 To 3
                    Validated = ValidateForAlpha(Mid(PAN, Pos, 1))
                Case 4
                    Validated = FourthCharacter(Mid(PAN, Pos, 1))
                Case 5
                    Validated = ValidateForAlpha(Mid(PAN, Pos, 1))
                Case 6 To 9
                    Validated = ValidateForNumeric(Mid(PAN, Pos, 1))
                Case 10
                    Validated = ValidateForAlpha(Mid(PAN, Pos, 1))
            End Select
            If Not Validated Or Len(PAN) <> 10 Then
                MsgBox("Invalid PAN!", vbCritical + vbOKOnly)
                textBox.Clear()
                textBox.Focus()
                Passed = False
                Exit For

            End If
        Next Pos
        ValidatePAN = Passed
    End Function
    Function ValidateForAlpha(ByVal inp As String) As Boolean
        If inp >= "A" And inp <= "Z" Then
            ValidateForAlpha = True
        Else
            ValidateForAlpha = False

        End If
    End Function
    Function FourthCharacter(ByVal fourth As String) As Boolean
        If fourth = "P" Then
            FourthCharacter = True
        Else
            FourthCharacter = False

        End If
    End Function

    Function ValidateForNumeric(ByVal input As String) As Boolean
        If Not input >= "A" And input <= "Z" Then
            If input <> 0 Then
                If Val(input) >= 1 Then
                    ValidateForNumeric = True
                Else
                    ValidateForNumeric = False
                End If
            Else
                ValidateForNumeric = True
            End If
        Else
            ValidateForNumeric = False
        End If
    End Function
#End Region
#Region "ValidatePassport"
    Function ValidatePassport(ByVal Passport As String, ByRef textBox As TextBox) As Boolean
        Dim Pos As Integer
        Dim number As Integer
        Dim Validated As Boolean
        Dim Passed As Boolean
        Passed = True
        number = Len(Passport)
        For Pos = 1 To Len(Passport)
            Select Case Pos
                Case 1
                    Validated = FirstPassportNumber(Mid(Passport, Pos, 1))
                Case 2 To number
                    Validated = RestPassportNumber(Mid(Passport, Pos, 1))
            End Select
            If Not Validated Or Len(Passport) <> 8 Then
                MsgBox("Enter a valid Passport number", vbCritical + vbOKOnly)
                textBox.Clear()
                textBox.Focus()
                Passed = False
                Exit For

            End If
        Next Pos
        ValidatePassport = Passed
    End Function

    Function FirstPassportNumber(ByVal first As String) As Boolean
        If first >= "A" And first <= "Z" Then
            FirstPassportNumber = True
        Else
            FirstPassportNumber = False
        End If
    End Function

    Function RestPassportNumber(ByVal rest As String) As Boolean
        If Not rest >= "A" And rest <= "Z" Then
            If rest <> 0 Then
                If Val(rest) >= 1 Then
                    RestPassportNumber = True
                Else
                    RestPassportNumber = False
                End If
            Else
                RestPassportNumber = True
            End If
        Else
            RestPassportNumber = False
        End If
    End Function
#End Region
#Region "ValidateUIDAI"
    Function ValidateUIDAI(ByVal UIDAI As String, ByRef textBox As TextBox) As Boolean
        Dim Passed As Boolean
        Passed = True
        Dim valid As New Regex("^[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]$")
        If UIDAI.Length > 0 Then
            If Not valid.IsMatch(UIDAI) OrElse UIDAI.Length <> 12 Then
                MsgBox("Please enter valid UIDAI Number", vbCritical + vbOKOnly)
                textBox.Clear()
                textBox.Focus()
                Passed = False
            End If
        End If
        ValidateUIDAI = Passed
    End Function
#End Region


    Private Sub IFSC_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IFSC_Search.Click
        'Dim dtCustomerBankInfo, dt As New DataTable
        'Dim dsCustomerBankInfo, ds As New DataSet
        'Try
        '    dsCustomerBankInfo = MyWebsevice.GetCustomerBankDetails(txtIFSCCode.Text.ToUpper.Trim())
        '    If dsCustomerBankInfo.Tables.Count > 0 Then
        '        dtCustomerBankInfo = dsCustomerBankInfo.Tables(0)
        '        If dtCustomerBankInfo.Rows.Count > 0 Then
        '            If Not dtCustomerBankInfo.Rows(0)(0) Is DBNull.Value Then
        '                cmbNeftState.SelectedValue = dtCustomerBankInfo.Rows(0)(0).ToString()
        '                If Not cmbNeftState.SelectedValue Is Nothing Then
        '                    dt = MyWebsevice.brdistrict(cmbNeftState.SelectedValue, CInt(BranchID)).Tables(0)
        '                    If dt.Rows.Count > 0 Then
        '                        Me.cmbNeftDistrict.DataSource = dt
        '                        Me.cmbNeftDistrict.DisplayMember = dt.Columns(0).ColumnName
        '                        Me.cmbNeftDistrict.ValueMember = dt.Columns(1).ColumnName
        '                    End If
        '                End If

        '            Else
        '                cmbNeftState.SelectedIndex = 0
        '            End If
        '            If Not dtCustomerBankInfo.Rows(0)(1) Is DBNull.Value Then
        '                cmbNeftDistrict.SelectedValue = dtCustomerBankInfo.Rows(0)(1).ToString()
        '                If Not cmbNeftDistrict.SelectedValue Is Nothing Then
        '                    ds = MyWebsevice.bank(CInt(cmbNeftDistrict.SelectedValue))
        '                    If ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
        '                        Me.cmbBank.DataSource = ds.Tables(0)
        '                        Me.cmbBank.DisplayMember = ds.Tables(0).Columns(1).ColumnName
        '                        Me.cmbBank.ValueMember = ds.Tables(0).Columns(0).ColumnName
        '                    End If
        '                End If
        '            Else
        '                cmbNeftDistrict.SelectedIndex = 0
        '            End If
        '            If Not dtCustomerBankInfo.Rows(0)(2) Is DBNull.Value Then
        '                cmbBank.SelectedValue = dtCustomerBankInfo.Rows(0)(2).ToString()
        '            Else
        '                cmbBank.SelectedIndex = 0
        '            End If
        '        Else
        '            MsgBox("This IFSCCode Not In Our Database,Please contact Marketing Team", vbCritical + vbOKOnly)
        '        End If

        '    Else
        '        MsgBox("This IFSCCode Not In Our Database,Please contact Marketing Team", vbCritical + vbOKOnly)
        '    End If
        'Catch ex As Exception
        '    Throw New ApplicationException("Customer Online Reg.Details Error:- " & ex.Message)
        'End Try
        IFSC_Fill()
    End Sub

    Private Sub cmbCMedia_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCMedia.SelectedIndexChanged
        If (cmbCMedia.SelectedValue = 29) Then
            BA_DSA_User.Media_ID = 29
            Dim DSABA As New BA_DSA_User()
            DSABA.ShowDialog()


        ElseIf (cmbCMedia.SelectedValue = 14) Then
            If txtMobileNo.Text = "" Then
                MsgBox("Enter Mobile Number..!!", , "Warning")
                cmbCMedia.SelectedValue = 0
                Exit Sub
            Else
                Dim MobCnt As Integer = MyWebsevice.CheckForMobileDuplication(txtMobileNo.Text)
                If MobCnt > 0 Then
                    MsgBox("This lead customer aleady existing with us..!!", , "Warning")
                    cmbCMedia.SelectedValue = 0
                    Exit Sub
                End If
            End If
            Dim BaCnt As Integer = MyWebsevice.BaCheck(txtMobileNo.Text)
            If BaCnt = 0 Then
                MsgBox("There is no lead found for this customer..!!", , "Warning")
                cmbCMedia.SelectedValue = 0
                Exit Sub
            End If
            BA_DSA_User.Media_ID = 14
            Dim DSABA As New BA_DSA_User()
            DSABA.ShowDialog()
        Else
            code = ""
        End If
    End Sub

    Public Sub IFSC_Fill()
        Dim dtCustomerBankInfo, dt As New DataTable
        Dim dsCustomerBankInfo, ds As New DataSet
        Try
            dsCustomerBankInfo = MyWebsevice.GetCustomerBankDetails(txtIFSCCode.Text.ToUpper.Trim())
            If dsCustomerBankInfo.Tables.Count > 0 Then
                dtCustomerBankInfo = dsCustomerBankInfo.Tables(0)
                If dtCustomerBankInfo.Rows.Count > 0 Then
                    If Not dtCustomerBankInfo.Rows(0)(0) Is DBNull.Value Then
                        cmbNeftState.SelectedValue = dtCustomerBankInfo.Rows(0)(0).ToString()
                        If Not cmbNeftState.SelectedValue Is Nothing Then
                            dt = MyWebsevice.brdistrict(cmbNeftState.SelectedValue, CInt(BranchID)).Tables(0)
                            If dt.Rows.Count > 0 Then
                                Me.cmbNeftDistrict.DataSource = dt
                                Me.cmbNeftDistrict.DisplayMember = dt.Columns(0).ColumnName
                                Me.cmbNeftDistrict.ValueMember = dt.Columns(1).ColumnName
                            End If
                        End If

                    Else
                        cmbNeftState.SelectedIndex = 0
                    End If
                    If Not dtCustomerBankInfo.Rows(0)(1) Is DBNull.Value Then
                        cmbNeftDistrict.SelectedValue = dtCustomerBankInfo.Rows(0)(1).ToString()
                        If Not cmbNeftDistrict.SelectedValue Is Nothing Then
                            ds = MyWebsevice.bank(CInt(cmbNeftDistrict.SelectedValue))
                            If ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                                Me.cmbBank.DataSource = ds.Tables(0)
                                Me.cmbBank.DisplayMember = ds.Tables(0).Columns(1).ColumnName
                                Me.cmbBank.ValueMember = ds.Tables(0).Columns(0).ColumnName
                            End If
                        End If
                    Else
                        cmbNeftDistrict.SelectedIndex = 0
                    End If
                    If Not dtCustomerBankInfo.Rows(0)(2) Is DBNull.Value Then
                        cmbBank.SelectedValue = dtCustomerBankInfo.Rows(0)(2).ToString()
                    Else
                        cmbBank.SelectedIndex = 0
                    End If
                Else
                    MsgBox("This IFSCCode Not In Our Database,Please contact Marketing Team", vbCritical + vbOKOnly)
                End If

            Else
                MsgBox("This IFSCCode Not In Our Database,Please contact Marketing Team", vbCritical + vbOKOnly)
            End If
        Catch ex As Exception
            Throw New ApplicationException("Customer Online Reg.Details Error:- " & ex.Message)
        End Try
    End Sub

    Private Sub txtACustName_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtACustName.MouseDown
        If e.Button = MouseButtons.Right Then
            MessageBox.Show("Right-click is not allowed", "No Right-click")
            Return
        End If
    End Sub

    Private Sub txtHouse_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtHouse.TextChanged
        Dim t As TextBox = DirectCast(sender, TextBox)
        Dim index As Integer = t.Text.IndexOf("  ")
        While index <> -1
            t.Text = t.Text.Replace("  ", " ")
            index = t.Text.IndexOf("  ")
        End While
        t.SelectionStart = t.Text.Length
    End Sub

    Private Sub txtALocation_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtALocation.TextChanged
        Dim t As TextBox = DirectCast(sender, TextBox)
        Dim index As Integer = t.Text.IndexOf("  ")
        While index <> -1
            t.Text = t.Text.Replace("  ", " ")
            index = t.Text.IndexOf("  ")
        End While
        t.SelectionStart = t.Text.Length
    End Sub

    Private Sub txtAFatHus_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtAFatHus.MouseDown
        If e.Button = MouseButtons.Right Then
            MessageBox.Show("Right-click is not allowed", "No Right-click")
            Return
        End If
    End Sub

    Private Sub txtHouse_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtHouse.MouseDown
        If e.Button = MouseButtons.Right Then
            MessageBox.Show("Right-click is not allowed", "No Right-click")
            Return
        End If
    End Sub

    Private Sub txtALocation_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtALocation.MouseDown
        If e.Button = MouseButtons.Right Then
            MessageBox.Show("Right-click is not allowed", "No Right-click")
            Return
        End If
    End Sub

    Private Sub txtAIdno_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtAIdno.MouseDown
        'If e.Button = MouseButtons.Right Then
        '    MessageBox.Show("Right-click is not allowed", "No Right-click")
        '    Return
        'End If
    End Sub

    Private Sub rb_MaleAdd_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rb_MaleAdd.CheckedChanged
        FillRelation("0,21")

    End Sub

    Public Sub FillRelation(ByVal relation_Id As String)
        Dim obj As New customerService.customer(set_ip)
        Dim ds As New DataSet
        ds = obj.Combofill(BranchID)
        Dim dtSearch As DataTable = ds.Tables("relation_dtl")
        dtSearch.DefaultView.RowFilter = "relation_id in (" + relation_Id + ")"
        Me.cmbiden.DataSource = Addhead(dtSearch, ds.Tables("relation_dtl").Columns(1).ColumnName, ds.Tables("relation_dtl").Columns(0).ColumnName) 'Added for req 4829
        'dtSearch.DefaultView.RowFilter = "relation_id"
        'Me.cmbiden.DataSource = dtSearch

        Me.cmbiden.DisplayMember = dtSearch.Columns(1).ColumnName
        Me.cmbiden.ValueMember = dtSearch.Columns(0).ColumnName
    End Sub

    Private Sub rb_FemaleAdd_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rb_FemaleAdd.CheckedChanged
        FillRelation("0,23,22")
    End Sub

    Private Sub txtAPhoneno_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtAPhoneno.MouseDown
        If e.Button = MouseButtons.Right Then
            MessageBox.Show("Right-click is not allowed", "No Right-click")
            Return
        End If
    End Sub


    Private Sub txtPhcode_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtPhcode.MouseDown
        If e.Button = MouseButtons.Right Then
            MessageBox.Show("Right-click is not allowed", "No Right-click")
            Return
        End If
    End Sub


    Private Sub txtPhcode_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPhcode.KeyPress
        e.KeyChar = CChar(e.KeyChar.ToString.ToUpper)
        If Not (Asc(e.KeyChar) = 8) Then

            Dim allowedChars As String = "0123456789"
            If Not allowedChars.Contains(e.KeyChar.ToString.ToLower) Then
                e.KeyChar = ChrW(0)
                e.Handled = True
            End If


        End If
    End Sub
    Private Sub txtACustName_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtACustName.Enter
        tooltip.RemoveAll()
        tooltip.AutomaticDelay = 0
        tooltip.AutoPopDelay = 10000
        tooltip.InitialDelay = 10
        tooltip.IsBalloon = True
        tooltip.ReshowDelay = 10
        tooltip.ToolTipIcon = ToolTipIcon.Info
        tooltip.Show("Name must be as per the ID proof provided; only alphabets shall be entered", Me.txtACustName)
    End Sub

    Private Sub txtACustName_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtACustName.Leave
        tooltip.Hide(txtACustName)
    End Sub

    Private Sub txtAIdno_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAIdno.Enter
        'Label144.Text = "dfjcfgbcfksdhgfjksdhgjdf"
        tooltip.RemoveAll()
        tooltip.AutomaticDelay = 0
        tooltip.AutoPopDelay = 10000
        tooltip.InitialDelay = 10
        tooltip.IsBalloon = True
        tooltip.ReshowDelay = 10
        tooltip.ToolTipIcon = ToolTipIcon.Info
        tooltip.Show("The number of the ID proof provided and click Search KYC button", Me.txtAIdno)

    End Sub

    Private Sub cmbId_MouseHover(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbId.MouseHover
        tooltip.RemoveAll()
        tooltip.AutomaticDelay = 0
        tooltip.AutoPopDelay = 10000
        tooltip.InitialDelay = 10
        tooltip.IsBalloon = True
        tooltip.ReshowDelay = 10
        tooltip.ToolTipIcon = ToolTipIcon.Info
        tooltip.Show("The type of ID proof provided by the customer", Me.cmbId)
    End Sub

    'Private Sub DateTimePicker3_MouseHover(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    tooltip.RemoveAll()
    '    tooltip.AutomaticDelay = 0
    '    tooltip.AutoPopDelay = 10000
    '    tooltip.InitialDelay = 10
    '    tooltip.IsBalloon = True
    '    tooltip.ReshowDelay = 10
    '    tooltip.ToolTipIcon = ToolTipIcon.Info
    '    tooltip.Show("DOB must be as per the ID proof provided", Me.DateTimePicker3)
    'End Sub









    'Private Sub txtAIdno_PreviewKeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles txtAIdno.PreviewKeyDown
    '    Dim EYC As New EKYC()
    '    EYC.ShowDialog()
    'End Sub






    'Private Sub cmbId_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbId.SelectedIndexChanged
    '    Try
    '        Dim ss As String = cmbId.SelectedValue.ToString()
    '        Me.Cursor = Cursors.WaitCursor
    '        If cmbId.SelectedValue = 505 Then

    '        End If
    '    Catch ex As Exception 'added for req 4829
    '        MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    Finally
    '        Me.Cursor = Cursors.Default
    '    End Try

    'End Sub





    Private Sub add_Land_Details_Enter(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            CheckForOnlineCustomer()
            Me.Cursor = Cursors.WaitCursor
            If Me.txtCustomerid.Text <> "" Then
                Me.Main_tab.TabPages(1).Enabled = True
                Me.tb_customermodify.BackColor = Drawing.Color.LightSteelBlue
                'Land_DetailsUpdateBinding()
                Me.Main_tab.TabPages(6).Enabled = True
                'Me.TextBox18.Text = ""
            Else
                MsgBox("Search Customer Details")
                Me.Main_tab.SelectedTab = Me.tb_search
            End If
        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    'Protected Sub Land_DetailsUpdateBinding()

    '    Me.TextBox16.DataBindings.Clear()
    '    Me.TextBox17.DataBindings.Clear()
    '    Me.TextBox18.DataBindings.Clear()

    '    If Not cust_result Is Nothing Then
    '        Me.TextBox16.DataBindings.Add("Text", cust_result, "CUST_ID")
    '        Me.TextBox17.DataBindings.Add("Text", cust_result, "cust_name")
    '    End If
    'End Sub

    'Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Try
    '        Me.Cursor = Cursors.WaitCursor
    '        Dim obj As New customerService.customer(set_ip)
    '        Dim confstr As String = ""
    '        Dim result As String = ""
    '        confstr = Me.ComboBox1.SelectedValue.ToString

    '        confstr += "¥" + Me.TextBox18.Text.ToString 'LAND CERTIFICATE NO
    '        confstr += "¥" + Me.TextBox17.Text.ToString 'CUSTOMER NAME
    '        confstr += "¥" + Me.TextBox16.Text.ToString 'CUSTOMER ID

    '        Try
    '            If Me.TextBox16.Text = "" Then
    '                result = "Null Value!! Please Enter Land Certificate No"
    '            End If
    '            If Me.TextBox17.Text = "" Then
    '                result = "Null Value!! Please Check Customer details Before confirmation"
    '            End If
    '            If Me.TextBox18.Text = "" Then
    '                result = "Null Value!! Please Check Customer details Before confirmation"
    '            End If

    '            If Me.TextBox16.Text <> "" Then
    '                If confstr <> "" Then
    '                    result = obj.customeraddCerDtls(confstr, FirmID, BranchID, User_id)
    '                    ' result = "Yes"
    '                Else
    '                    result = "Null Value!! Please Check the Details"
    '                End If
    '            End If
    '        Catch ex As Exception
    '            result = ex.Message
    '        End Try

    '        MsgBox(result)
    '    Catch ex As Exception 'added for req 4829
    '        MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    Finally
    '        Me.Cursor = Cursors.Default
    '    End Try
    'End Sub

    Private Sub Button6_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Main_tab.SelectedTab = Me.tb_search
    End Sub


    Private Sub cmbId_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbId.SelectedIndexChanged

        Try
            Me.Cursor = Cursors.WaitCursor
            If Me.cmbId.Focused = True Then
                If Me.cmbId.Items.Count > 0 Then
                    'Mediafill(Me.cmbCMediatype.SelectedValue)
                    If (cmbId.SelectedValue = 16 Or cmbId.SelectedValue = 505) Then

                        'btngetkyc.Visible = False
                        btnkycc.Visible = False
                        txtACustName.ReadOnly = True
                        'rb_MaleAdd.Enabled = True

                        'rb_FemaleAdd.Enabled = True
                        txtAFatHus.ReadOnly = True
                        txtHouse.ReadOnly = True
                        txtALocation.ReadOnly = True
                        cmbCountry.Enabled = False
                        cmbState.Enabled = False
                        cmbDistrict.Enabled = False

                        'cmbDistrict.Enabled = False
                        'Me.DateTimePicker3.Enabled = False
                        'txtdob
                        Me.txtdob.Enabled = False
                        Me.txtEmail.ReadOnly = False

                        txtPincode.ReadOnly = True
                        txtAIdno.Focus()
                        loadfill()
                    Else
                        loadfill()
                        'btngetkyc.Visible = False
                        btnkycc.Visible = False
                        txtACustName.ReadOnly = False
                        txtACustName.Text = ""

                        rb_MaleAdd.Checked = False

                        rb_FemaleAdd.Checked = False
                        txtAFatHus.ReadOnly = False
                        txtAFatHus.Text = ""
                        txtHouse.ReadOnly = False
                        txtHouse.Text = ""
                        txtALocation.ReadOnly = False
                        txtALocation.Text = ""
                        cmbCountry.Enabled = True
                        cmbState.Enabled = True
                        cmbDistrict.Enabled = True

                        'cmbDistrict.Enabled = False
                        'Me.DateTimePicker3.Enabled = True
                        'Me.DateTimePicker3.Text = DateTime.Now.Date.ToString()
                        Me.txtEmail.ReadOnly = True

                        txtPincode.ReadOnly = True
                        txtAIdno.Text = ""
                        'txtAIdno.Focus()

                        'btnkycc.Visible = False
                        txtAIdno.Focus()

                    End If


                End If
            Else
                'If Me.cmbCMediatype.SelectedIndex = 0 Then
                '    Mediafill("0")
                'End If
            End If

        Catch ex As Exception 'added for req 4829
            MessageBox.Show(ex.Message, "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    'Private Sub txtAIdno_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAIdno.MouseLeave
    '    Label144.Text = "dfjksdhgfjksdhgjdf"
    'End Sub
    Private Sub btnkycc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnkycc.Click
        txtCustId.Text = ""
        Dim ws As New customerService.customer(set_ip)
        Dim USERID As String = WHELPER.getData("userid")
        If txtAIdno.Text Is Nothing Or txtAIdno.Text = "" Then
            MessageBox.Show("Please enter valid UIDAI Number")
        Else
            Dim result As Integer = MessageBox.Show("Are you sure do you want confirm with this Aadhaar number", "Attention", MessageBoxButtons.YesNo)
            If result = DialogResult.No Then
                txtAIdno.Focus()
            ElseIf result = DialogResult.Yes Then
                Try

                    'Dim ChkExist As String = ws.ChkExistEKYC(txtAIdno.Text)
                    'If (ChkExist = "0") Then
                    Dim dts As DataTable

                    dts = ws.ExtCust_KycVerifyDetails(txtAIdno.Text, "AADHAARID")
                    If dts.Rows.Count > 0 Then
                        If Not IsDBNull(dts.Rows(0)(0)) And Not IsDBNull(dts.Rows(0)(2)) Then
                            MessageBox.Show("This Aadhaar number already verified . Verified Customer ID : '" + dts.Rows(0)(2) + "' | Verified Date : '" + dts.Rows(0)(1) + "'")
                        Else
                            btngetkyc.Visible = True
                            Diagnostics.Process.Start("iexplore.exe", "http://app.manappuram.net/CUSTOMEREKYC/EKYC/EKYC.application?processid=7F7F9B5EF752F72FFCD330451F1781A41BAA3A11C179108CAAC4376C3532610F69AF1053D30FAB2E83AE95F2BE1684EECC86649FB86DE040F45D8224D3D3B92822DFB201AA5FF3F232578B6F83ED8BDB$" + txtAIdno.Text + "$" + User_id() + "$" + BranchID)

                            ' Diagnostics.Process.Start("iexplore.exe", "http://10.0.0.111/EKYC/EKYC.application?processid=7F7F9B5EF752F72FFCD330451F1781A41BAA3A11C179108CAAC4376C3532610F69AF1053D30FAB2E83AE95F2BE1684EECC86649FB86DE040F45D8224D3D3B92822DFB201AA5FF3F232578B6F83ED8BDB$" + txtAIdno.Text + "$" + User_id() + "$" + BranchID)
                        End If

                    Else
                        btngetkyc.Visible = True
                        Diagnostics.Process.Start("iexplore.exe", "http://app.manappuram.net/CUSTOMEREKYC/EKYC/EKYC.application?processid=7F7F9B5EF752F72FFCD330451F1781A41BAA3A11C179108CAAC4376C3532610F69AF1053D30FAB2E83AE95F2BE1684EECC86649FB86DE040F45D8224D3D3B92822DFB201AA5FF3F232578B6F83ED8BDB$" + txtAIdno.Text + "$" + User_id() + "$" + BranchID)

                        'Diagnostics.Process.Start("iexplore.exe", "http://10.0.0.111/EKYC/EKYC.application?processid=7F7F9B5EF752F72FFCD330451F1781A41BAA3A11C179108CAAC4376C3532610F69AF1053D30FAB2E83AE95F2BE1684EECC86649FB86DE040F45D8224D3D3B92822DFB201AA5FF3F232578B6F83ED8BDB$" + txtAIdno.Text + "$" + User_id() + "$" + BranchID)

                    End If





                    ' Dim wc As WebClient = New WebClient()
                    ' wc.DownloadFile("http://10.0.0.111/EKYC/EKYC.application?processid=7F7F9B5EF752F72FFCD330451F1781A41BAA3A11C179108CAAC4376C3532610F69AF1053D30FAB2E83AE95F2BE1684EECC86649FB86DE040F45D8224D3D3B92822DFB201AA5FF3F232578B6F83ED8BDB$" + txtAIdno.Text + "$" + User_id() + "$" + BranchID, "test")
                    'myProcess.StartInfo.Arguments = serverurl


                    'myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal
                    ''CUSTOMEREKYC\EKYC
                    'Diagnostics.Process.Start("iexplore.exe")
                    ''Dim msg As String = "http://10.0.0.111/EKYC/EKYC.application?processid=7F7F9B5EF752F72FFCD330451F1781A41BAA3A11C179108CAAC4376C3532610F69AF1053D30FAB2E83AE95F2BE1684EECC86649FB86DE040F45D8224D3D3B92822DFB201AA5FF3F232578B6F83ED8BDB$" + txtAIdno.Text + "$" + User_id() + "$" + BranchID
                    'myProcess.StartInfo.FileName = "http://10.0.0.111/EKYC/EKYC.application?processid=7F7F9B5EF752F72FFCD330451F1781A41BAA3A11C179108CAAC4376C3532610F69AF1053D30FAB2E83AE95F2BE1684EECC86649FB86DE040F45D8224D3D3B92822DFB201AA5FF3F232578B6F83ED8BDB$" + txtAIdno.Text + "$" + User_id() + "$" + BranchID

                    ''myProcess.StartInfo.FileName = "http://app.manappuram.net/CUSTOMEREKYC/EKYC/EKYC.application?processid=7F7F9B5EF752F72FFCD330451F1781A41BAA3A11C179108CAAC4376C3532610F69AF1053D30FAB2E83AE95F2BE1684EECC86649FB86DE040F45D8224D3D3B92822DFB201AA5FF3F232578B6F83ED8BDB$" + txtAIdno.Text + "$" + User_id() + "$" + BranchID
                    'Dim process As String = myProcess.Start()

                    'SetParent(process, Me.Handle)
                    'myProcess.Kill()




                    'myProcess.StartInfo = New System.Diagnostics.ProcessStartInfo("iexplore")
                    'Dim s As String = "http://10.0.0.111/EKYC/EKYC.application?processid=7F7F9B5EF752F72FFCD330451F1781A41BAA3A11C179108CAAC4376C3532610F69AF1053D30FAB2E83AE95F2BE1684EECC86649FB86DE040F45D8224D3D3B92822DFB201AA5FF3F232578B6F83ED8BDB$" + txtAIdno.Text + "$" + User_id() + "$" + BranchID

                    'myProcess.Arguments = s
                    'myProcess.Start()








                    'Else
                    'MessageBox.Show("This Aadhaar number already exist")
                    'End If

                Catch ex As Exception

                End Try
            End If
        End If
    End Sub
    Private Sub btngetkyc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btngetkyc.Click
        Dim resStr As String = ""
        Dim status, reson As String
        resStr = GetKycDtl(1)
        If resStr <> "0" Then
            Try
                Dim ws As New customerService.customer(set_ip)
                'MessageBox.Show(set_ip() + txtAIdno.Text + BranchID)

                Dim ser As JObject = JObject.Parse(resStr)
                'Dim data As List(Of JToken) = ser.Children().ToList
                Dim data As List(Of JToken) = New List(Of JToken)(ser.Children())

                For Each item As JProperty In data
                    item.CreateReader()
                    Select Case item.Name
                       
                        Case "KYCResponse"
                           

                            For Each msg As JObject In item.Values

                                status = msg("Status").ToString()
                                reson = msg("Reason").ToString()
                                If status <> 200 Then
                                    MsgBox(reson)
                                    Exit Sub
                                End If


                                Dim data1 As String = msg("Data").ToString()
                                Dim resbyt As Byte() = Convert.FromBase64String(data1)
                                Dim RESB64 As Byte() = FetchDecryptUsingSessionKey(resbyt)


                                Dim respdec As String = System.Text.Encoding.Default.GetString(RESB64)
                                Dim ser1 As JObject = JObject.Parse(respdec)
                                Dim data2 As List(Of JToken) = New List(Of JToken)(ser1.Children())

                                For Each item1 As JProperty In data2
                                    item1.CreateReader()
                                    Select Case item1.Name
                                        Case "UUID"
                                            UUID = item1.Value.ToString()
                                        Case "Poiname"
                                            Poiname = item1.Value.ToString()
                                        Case "Poidob"
                                            Poidob = item1.Value.ToString()
                                        Case "Poigender"
                                            Poigender = item1.Value.ToString()
                                        Case "Poaco"
                                            Poaco = item1.Value.ToString()
                                        Case "Poadist"
                                            Poadist = item1.Value.ToString()
                                        Case "Poahouse"
                                            Poahouse = item1.Value.ToString()
                                        Case "Poaloc"
                                            Poaloc = item1.Value.ToString()
                                        Case "Poapc"
                                            Poapc = item1.Value.ToString()
                                        Case "Poastate"
                                            Poastate = item1.Value.ToString()
                                        Case "Poavtc"
                                            Poavtc = item1.Value.ToString()
                                        Case "Poastreet"
                                            Poastreet = item1.Value.ToString()
                                        Case "Poalm"
                                            Poalm = item1.Value.ToString()
                                        Case "Poasubdist"
                                            Poasubdist = item1.Value.ToString()
                                        Case "Poapo"
                                            Poapo = item1.Value.ToString()
                                        Case "Pht"
                                            Pht = item1.Value
                                    End Select
                                Next
                            Next
                    End Select


                Next
               
                'Duplication Check
                dt = ws.CheckExist(UUID)
                If dt.Tables(0).Rows.Count > 0 Then
                    MsgBox("This KYC details already mapped with an existing customer..!! | Customer ID : " & dt.Tables(0).Rows(0)(0).ToString(), MsgBoxStyle.OkOnly, "Customer EKYC")
                    btngetkyc.Visible = True
                    Exit Sub
                End If

                If servType = "04" Then
                    servType = "BIO"
                ElseIf servType = "05" Then
                    servType = "OTP"
                End If
                Dim trnekyc As String = Poiname + "©" + Poidob + "©" + Poigender + "©" + "" + "©" + "" + "©" + Poaco + "©" + "" + "©" + Poahouse + "©" + "" + "©" + Poaloc + "©" + "" + "©" + "" + "©" + Poadist + "©" + Poastate + "©" + Poapc + "©" + "" + "©" + "1"
                ws.UpdateAadhaar(BranchID, "", servType, "KUA", UUID, ekyctrn, DateTime.Now, resStr, Pht, "Y", UserId, trnekyc, rrn_n)



                txtAIdno.Text = UUID
                dt = ws.customerKYCDisplaydata(txtAIdno.Text, rrn_n, BranchID)
                If Not (dt.Tables("customer_dt1").Rows.Count = 0) Then 'trying to check if some of my column values are null
                    If Not (IsDBNull(dt.Tables("customer_dt1").Rows(0)("CUS_NAME"))) Then
                        Me.txtACustName.Text = CStr(dt.Tables("customer_dt1").Rows(0)("CUS_NAME")).ToUpper()
                    Else
                        Me.txtACustName.ReadOnly = False
                    End If
                    If Not (IsDBNull(dt.Tables("customer_dt1").Rows(0)("CUS_DOB"))) Then
                        'Dim str As String
                        'Dim strArr() As String


                        'Dim xmlDate As String = "11/01/2017"
                        txtdob.Text = dt.Tables("customer_dt1").Rows(0)("CUS_DOB").ToString()
                        'Format(Convert.ToDateTime(dt.Tables("customer_dt1").Rows(0)("CUS_DOB")), "dd/MMM/yyyy")
                        'strArr = str.Split("-")

                        'Dim DAY As String = strArr(0).ToString()

                        'Dim MON As String = strArr(1).ToString()
                        'Dim YEAR As String = strArr(2).ToString()

                        ''Me.DateTimePicker3.Text = Convert.ToDateTime(DAY + "/" + MON + "/" + YEAR).Date.ToString()
                        'TextBox16.Text = DAY + "/" + MON + "/" + YEAR


                        'Dim xmlDate As String = dt.Tables("customer_dt1").Rows(0)("CUS_DOB").ToString()

                        'Dim convertedDate As Date = Date.Parse(xmlDate)
                        'Me.DateTimePicker3.Text = convertedDate
                        'Return convertedDate



                        'Dim format() = {"dd/MM/yyyy", "d/M/yyyy", "dd-MM-yyyy"}
                        'Dim expenddt As Date = Date.ParseExact(xmlDate, format, System.Globalization.DateTimeFormatInfo.InvariantInfo, Globalization.DateTimeStyles.None)

                        'Me.DateTimePicker3.Text = dt.Tables("customer_dt1").Rows(0)("CUS_DOB").ToString()
                        'Dim ss As String = dt.Tables("customer_dt1").Rows(0)("CUS_DOB").ToString()
                        'DateTime date = Convert.ToDateTime(dateString);

                        'Dim xmldates As DateTime = DateTime.Parse(xmlDate, CultureInfo.InvariantCulture)
                        'Dim xmldates As Date = Convert.ToDateTime(xmlDate)
                        'Dim DAY As String = expenddt.Day.ToString()

                        'Dim MON As String = expenddt.Month.ToString()
                        'Dim YEAR As String = expenddt.Year.ToString()

                        'Me.DateTimePicker3.Text = DAY + "/" + MON + "/" + YEAR

                        'Me.DateTimePicker3.Text = DateTime.ParseExact(Convert.ToDateTime(dt.Tables("customer_dt1").Rows(0)("CUS_DOB")), "dd/MM/yyyy", CultureInfo.InvariantCulture)
                        'Dim reformatted As String = Date.ToString("yyyyMMdd", CultureInfo.InvariantCulture)

                        'Dim dob As DateTime = DateTime.ParseExact(d, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)
                        'DateTime.ParseExact(YourDate, "ddd MMM dd HH:mm:ss KKKK yyyy", CultureInfo.InvariantCulture)
                        'Me.DateTimePicker3.Text = Format(Convert.ToDateTime(dt.Tables("customer_dt1").Rows(0)("CUS_DOB")), "dd/MM/yyyy")
                    Else
                        txtdob.ReadOnly = False
                    End If
                    If Not (IsDBNull(dt.Tables("customer_KYCdt1").Rows(0)("country_id"))) Then
                        Me.cmbCountry.SelectedValue = CStr(dt.Tables("customer_KYCdt1").Rows(0)("country_id"))
                    Else
                        Me.cmbCountry.Enabled = True
                    End If
                    If Not (IsDBNull(dt.Tables("customer_KYCdt1").Rows(0)("state_id"))) Then
                        Me.cmbState.SelectedValue = CStr(dt.Tables("customer_KYCdt1").Rows(0)("state_id"))
                    Else
                        Me.cmbState.Enabled = True
                    End If
                    If Not (IsDBNull(dt.Tables("customer_KYCdt1").Rows(0)("district_id"))) Then
                        Me.cmbDistrict.SelectedValue = CStr(dt.Tables("customer_KYCdt1").Rows(0)("district_id"))
                    Else
                        Me.cmbDistrict.Enabled = True
                    End If
                    If Not (IsDBNull(dt.Tables("customer_dt1").Rows(0)("CUST_MAIL"))) Then
                        Me.txtEmail.Text = CStr(dt.Tables("customer_dt1").Rows(0)("CUST_MAIL"))
                    Else
                        Me.txtEmail.ReadOnly = False
                    End If
                    If Not (IsDBNull(dt.Tables("customer_dt1").Rows(0)("CUS_HOUSE"))) Then
                        Dim House As String = CStr(dt.Tables("customer_dt1").Rows(0)("CUS_HOUSE"))
                        If House = "" Then
                            Me.txtHouse.ReadOnly = False
                        Else
                            Me.txtHouse.Text = House.Trim().TrimStart(","c)
                        End If
                    Else
                        Me.txtHouse.ReadOnly = False
                    End If
                    If Not (IsDBNull(dt.Tables("customer_dt1").Rows(0)("CUS_GENDER"))) Then
                        Dim GENDER As String = CStr(dt.Tables("customer_dt1").Rows(0)("CUS_GENDER"))
                        If GENDER = "MALE" Then
                            rb_MaleAdd.Checked = True
                        Else
                            rb_FemaleAdd.Checked = True
                        End If
                    End If
                    If Not (IsDBNull(dt.Tables("customer_dt1").Rows(0)("CUST_FAT"))) Then
                        Dim NameSpilt As String = CStr(dt.Tables("customer_dt1").Rows(0)("CUST_FAT"))
                        If NameSpilt <> "--" Then
                            If NameSpilt.Substring(0, 3) = "D/O" Then
                                cmbiden.SelectedValue = 22
                                Me.txtAFatHus.Text = NameSpilt.Replace("D/O", "").Replace(":", "")
                            ElseIf NameSpilt.Substring(0, 3) = "W/O" Then
                                cmbiden.SelectedValue = 23
                                Me.txtAFatHus.Text = NameSpilt.Replace("W/O", "").Replace(":", "")
                            ElseIf NameSpilt.Substring(0, 3) = "S/O" Then
                                cmbiden.SelectedValue = 21
                                Me.txtAFatHus.Text = NameSpilt.Replace("S/O", "").Replace(":", "")
                            Else
                                Me.txtAFatHus.Text = CStr(dt.Tables("customer_dt1").Rows(0)("CUST_FAT"))
                            End If
                            'If NameSpilt.Split(":")(1) Is Nothing Or NameSpilt.Split(":")(1) = "" Then
                            '    Me.txtAFatHus.Text = CStr(dt.Tables("customer_dt1").Rows(0)("CUST_FAT"))
                            'Else
                            '    Me.txtAFatHus.Text = NameSpilt.Split(":")(1)
                            'End If
                        Else
                            Me.txtAFatHus.ReadOnly = False
                        End If
                    Else
                        Me.txtAFatHus.ReadOnly = False
                    End If
                    If Not (IsDBNull(dt.Tables("customer_dt1").Rows(0)("CUST_MOBILE"))) Then
                        Dim Mobile_NO As String = CStr(dt.Tables("customer_dt1").Rows(0)("CUST_MOBILE"))
                        txtMobileNo.Text = Mobile_NO
                        MobileOTP.AdharMobile = Mobile_NO
                    End If
                    Me.cmbPost.DataSource = Addhead(dt.Tables("customer_POSTKYCdt1"), dt.Tables("customer_POSTKYCdt1").Columns(1).ColumnName, dt.Tables("customer_POSTKYCdt1").Columns(0).ColumnName) 'Added for req 4829
                    Me.cmbPost.ValueMember = dt.Tables("customer_POSTKYCdt1").Columns(0).ColumnName
                    Me.cmbPost.DisplayMember = dt.Tables("customer_POSTKYCdt1").Columns(1).ColumnName
                    If Not (IsDBNull(dt.Tables("customer_dt1").Rows(0)("CUS_STREET")) Or (IsDBNull(dt.Tables("customer_dt1").Rows(0)("CUS_LOCAL")))) Then
                        Dim Location As String = CStr(dt.Tables("customer_dt1").Rows(0)("CUS_STREET")) + "," + CStr(dt.Tables("customer_dt1").Rows(0)("CUS_LOCAL"))
                        Me.txtALocation.Text = Location.Trim().TrimStart(","c)
                        If Me.txtALocation.Text <> "" And txtALocation.TextLength < 41 Then
                            Me.txtALocation.ReadOnly = True
                        Else
                            Me.txtALocation.ReadOnly = False
                        End If
                    Else
                        Me.txtALocation.ReadOnly = False
                    End If
                    'If Not (IsDBNull(dt.Tables("customer_dt1").Rows(0)("CUST_PIN"))) Then
                    Me.txtPincode.Text = CStr(dt.Tables("customer_dt1").Rows(0)("CUST_PIN"))
                    'End If
                Else
                    MessageBox.Show("No data found ")
                    btngetkyc.Visible = True
                End If
            Catch ex As Exception
                MessageBox.Show(ex.ToString())
            End Try
        End If
    End Sub
    Private Sub btnKycMatch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnKycMatch.Click
        If Me.txtaadhaarnum.Text = "" Or Me.txtaadhaarnum.Text = "0" Then
            MsgBox("Please enter Id No:")
            Me.txtaadhaarnum.Focus()
            flag = 0
            Exit Sub
        End If
        Dim dts As DataTable
        Dim ws As New customerService.customer(set_ip)
        dts = ws.ExtCust_KycVerifyDetails(txtaadhaarnum.Text, "AADHAARID")
        If dts.Rows.Count > 0 Then
            If Not IsDBNull(dts.Rows(0)(0)) And Not IsDBNull(dts.Rows(0)(2)) Then
                MessageBox.Show("This Aadhaar number already verified . Verified Customer ID : '" + dts.Rows(0)(2) + "' | Verified Date : '" + dts.Rows(0)(1) + "'")
            Else
                Diagnostics.Process.Start("iexplore.exe", "http://app.manappuram.net/CUSTOMEREKYC/EKYC/EKYC.application?processid=7F7F9B5EF752F72FFCD330451F1781A41BAA3A11C179108CAAC4376C3532610F69AF1053D30FAB2E83AE95F2BE1684EECC86649FB86DE040F45D8224D3D3B92822DFB201AA5FF3F232578B6F83ED8BDB$" + txtaadhaarnum.Text + "$" + User_id() + "$" + BranchID)
                btnKycMatch.Enabled = False
                btn_mapKyc.Enabled = True
            End If
        Else
            Diagnostics.Process.Start("iexplore.exe", "http://app.manappuram.net/CUSTOMEREKYC/EKYC/EKYC.application?processid=7F7F9B5EF752F72FFCD330451F1781A41BAA3A11C179108CAAC4376C3532610F69AF1053D30FAB2E83AE95F2BE1684EECC86649FB86DE040F45D8224D3D3B92822DFB201AA5FF3F232578B6F83ED8BDB$" + txtaadhaarnum.Text + "$" + User_id() + "$" + BranchID)
            btnKycMatch.Enabled = False
            btn_mapKyc.Enabled = True
        End If

        'Diagnostics.Process.Start("iexplore.exe", "http://app.manappuram.net/EKYC/EKYC.application?processid=7F7F9B5EF752F72FFCD330451F1781A41BAA3A11C179108CAAC4376C3532610F69AF1053D30FAB2E83AE95F2BE1684EECC86649FB86DE040F45D8224D3D3B92822DFB201AA5FF3F232578B6F83ED8BDB$" + txtaadhaarnum.Text + "$" + User_id() + "$" + BranchID)



    End Sub

    Private Sub btn_mapKyc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_mapKyc.Click
        Try
            Dim Customer_id As String = txtCustomerid.Text.ToString()
            'Dim Aadhaar_Id As String = txtaadhaarnum.Text.ToString()

            Dim resStr As String = ""
            Dim status, reson As String
            resStr = GetKycDtl(0)
            If resStr <> "0" Then
                Try
                    Dim ws As New customerService.customer(set_ip)
                    'MessageBox.Show(set_ip() + txtAIdno.Text + BranchID)

                    Dim ser As JObject = JObject.Parse(resStr)
                    'Dim data As List(Of JToken) = ser.Children().ToList
                    Dim data As List(Of JToken) = New List(Of JToken)(ser.Children())

                    For Each item As JProperty In data
                        item.CreateReader()
                        Select Case item.Name

                            Case "KYCResponse"


                                For Each msg As JObject In item.Values

                                    status = msg("Status").ToString()
                                    reson = msg("Reason").ToString()
                                    If status <> 200 Then
                                        MsgBox(reson)
                                        Exit Sub
                                    End If


                                    Dim data1 As String = msg("Data").ToString()
                                    Dim resbyt As Byte() = Convert.FromBase64String(data1)
                                    Dim RESB64 As Byte() = FetchDecryptUsingSessionKey(resbyt)


                                    Dim respdec As String = System.Text.Encoding.Default.GetString(RESB64)
                                    'MsgBox(respdec)
                                    'Dim respdec1 As String = System.Text.Encoding.Default.GetString(RESB64)
                                    'MsgBox(respdec1)
                                    '''''''''''''
                                    Dim ser1 As JObject = JObject.Parse(respdec)
                                    'Dim data As List(Of JToken) = ser.Children().ToList
                                    Dim data2 As List(Of JToken) = New List(Of JToken)(ser1.Children())

                                    For Each item1 As JProperty In data2
                                        item1.CreateReader()
                                        Select Case item1.Name



                                            Case "UUID"
                                                UUID = item1.Value.ToString()

                                                'MsgBox(item.Name)
                                            Case "Poiname"
                                                Poiname = item1.Value.ToString()
                                            Case "Poidob"
                                                Poidob = item1.Value.ToString()
                                            Case "Poigender"
                                                Poigender = item1.Value.ToString()

                                            Case "Poaco"
                                                Poaco = item1.Value.ToString()
                                            Case "Poadist"
                                                Poadist = item1.Value.ToString()
                                            Case "Poahouse"
                                                Poahouse = item1.Value.ToString()
                                            Case "Poaloc"
                                                Poaloc = item1.Value.ToString()
                                            Case "Poapc"
                                                Poapc = item1.Value.ToString()
                                            Case "Poastate"
                                                Poastate = item1.Value.ToString()
                                            Case "Poavtc"
                                                Poavtc = item1.Value.ToString()
                                            Case "Poastreet"
                                                Poastreet = item1.Value.ToString()
                                            Case "Poalm"
                                                Poalm = item1.Value.ToString()
                                            Case "Poasubdist"
                                                Poasubdist = item1.Value.ToString()
                                            Case "Poapo"
                                                Poapo = item1.Value.ToString()
                                            Case "Pht"
                                                Pht = item1.Value

                                                ' For Each msg1 As JObject In item1.Values

                                                ' Pht = msg1("Pht")
                                                '  Next
                                        End Select


                                    Next

                                    '''''''''''''''


                                    'UUID = msg("UUID").ToString()
                                    'Poiname = msg("Poiname").ToString()
                                    'Poidob = msg("Poidob").ToString()
                                    'Poigender = msg("Poigender").ToString()
                                    'Poaco = msg("Poaco").ToString()
                                    'Poadist = msg("Poadist").ToString()
                                    'Poahouse = msg("Poahouse").ToString()
                                    'Poaloc = msg("Poaloc").ToString()
                                    'Poapc = msg("Poapc").ToString()
                                    'Poastate = msg("Poastate").ToString()
                                    'Poavtc = msg("Poavtc").ToString()
                                    'Poastreet = msg("Poastreet").ToString()
                                    'Poalm = msg("Poalm").ToString()
                                    'Poasubdist = msg("Poasubdist").ToString()
                                    'Poapo = msg("Poapo").ToString()
                                    'Pht = msg("Pht")
                                Next


                        End Select


                    Next


                    dt = ws.CheckExist(UUID)
                    If dt.Tables(0).Rows.Count > 0 Then
                        MsgBox("This KYC details already mapped with an existing customer..!! | Customer ID : " & dt.Tables(0).Rows(0)(0).ToString(), MsgBoxStyle.OkOnly, "Customer EKYC")
                        Exit Sub
                    End If

                    If servType = "04" Then
                        servType = "BIO"
                    ElseIf servType = "05" Then
                        servType = "OTP"
                    End If
                    Dim trnekyc As String = Poiname + "©" + Poidob + "©" + Poigender + "©" + "" + "©" + "" + "©" + Poaco + "©" + "" + "©" + Poahouse + "©" + "" + "©" + Poaloc + "©" + "" + "©" + "" + "©" + Poadist + "©" + Poastate + "©" + Poapc + "©" + "" + "©" + "1"
                    ws.UpdateAadhaar(BranchID, "", servType, "KUA", UUID, ekyctrn, DateTime.Now, resStr, Pht, "Y", UserId, trnekyc, rrn_n)

                    Dim SucessMsg As String = ws.Matching_ExtCust_KycVerify(Customer_id, UUID, rrn_n)
                    MessageBox.Show(SucessMsg)

                    btnKycMatch.Enabled = False
                    btn_mapKyc.Enabled = False
                    Me.grp2.Visible = False
                    Me.grp3.Visible = False
                    Me.grp2.Height = 0
                    Me.grp3.Height = 0
                Catch ex As Exception
                    MessageBox.Show(ex.ToString())
                End Try
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try
    End Sub

    Private Sub txtaadhaarnum_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtaadhaarnum.Validating
        'If cmbId.SelectedValue = 16 OrElse cmbId.SelectedValue = 505 OrElse cmbId.SelectedValue = 555 Then
        ValidateUIDAI(txtaadhaarnum.Text, txtaadhaarnum)
        'End If
    End Sub

    Private Function GetKycDtl(ByVal Flg As Integer) As String
        Try
            Dim ws As New customerService.customer(set_ip)
            Dim GetUUID As String = ""
            Dim xmlDoc = New XmlDocument
            Dim xmlNode As Xml.XmlNode
            If Flg = 1 Then
                Dim srvc As New Syntizen.DataVault.UUIDService
                Dim resp = srvc.GetUUID(ws.Rrn_Gen(), BranchID)
                xmlDoc.LoadXml(resp)
                xmlNode = xmlDoc.SelectSingleNode("//RESPONSE/STATUS")
                If xmlNode.InnerText <> 1 And xmlNode.InnerText <> 2 Then
                    xmlNode = xmlDoc.SelectSingleNode("//RESPONSE/ERRORDESC")
                    MsgBox(xmlNode.InnerText)
                    Return "0"
                Else
                    xmlNode = xmlDoc.SelectSingleNode("//RESPONSE/UUID")
                    GetUUID = xmlNode.InnerText
                End If

                If GetUUID <> "0" Then
                    dt = ws.CheckExist(GetUUID)
                    If dt.Tables(0).Rows.Count > 0 Then
                        MsgBox("This KYC details already mapped with an existing customer..!! | Customer ID : '" & dt.Tables(0).Rows(0)(0).ToString() & "'", MsgBoxStyle.OkOnly, "Customer EKYC")
                        Return "0"
                    End If

                    dt = ws.ChkUuidExist(GetUUID)
                    If dt.Tables(0).Rows(0)(0).ToString() <> "" Then
                        MsgBox("Customer with same aadhaar number already exist..!! | Customer ID(s) : '" & dt.Tables(0).Rows(0)(0).ToString() & "'", MsgBoxStyle.OkOnly, "Customer EKYC")
                        Return "0"
                    End If
                End If
            End If

            Dim authLib As eKycAuthLib = New eKycAuthLib()
            Dim ekycResponse As String = authLib.DoAuth(ws.Rrn_Gen(), "", BranchID)
            'MsgBox(ekycResponse)
            Dim i As Integer = 0
            Dim stus As XmlReadMode

            'Dim xmlDoc = New XmlDocument
            'Dim xmlNode As Xml.XmlNode
            'MsgBox(ekycResponse)
            xmlDoc.LoadXml(ekycResponse)
            XmlNode = xmlDoc.SelectSingleNode("//KYCRES/STATUS")
            If XmlNode.InnerText <> 1 Then
                XmlNode = xmlDoc.SelectSingleNode("//KYCRES/ERRMSG")
                MsgBox(XmlNode.InnerText)
                Return "0"
            End If
            XmlNode = xmlDoc.SelectSingleNode("//KYCRES/TXNID")
            ekyctrn = XmlNode.InnerText
            XmlNode = xmlDoc.SelectSingleNode("//KYCRES/SERVICETYPE")
            servType = XmlNode.InnerText
            XmlNode = xmlDoc.SelectSingleNode("//KYCRES/RRN")
            Dim msgid As String
            If Not XmlNode Is Nothing Then
                rrn_n = XmlNode.InnerText
            Else
                rrn_n = ""
            End If
        Catch ex As Exception
            Return ex.Message.ToString()
        End Try

        Try
            Dim API_KEY As String = ConfigurationSettings.AppSettings.Get("apiv")

            Dim URLAuth As String = "https://mafildemofetch.manappuram.com/FetchkycProd/api/requestkycdata"
            Dim postString As String = String.Format("RRN={0}&APIKEY={1}", rrn_n, API_KEY)

            Const contentType As String = "application/x-www-form-urlencoded"
            System.Net.ServicePointManager.Expect100Continue = False

            Dim cookies As New CookieContainer()
            Dim webRequest__1 As HttpWebRequest = TryCast(WebRequest.Create(URLAuth), HttpWebRequest)
            webRequest__1.Method = "POST"
            webRequest__1.ContentType = contentType
            'webRequest__1.CookieContainer = cookies
            webRequest__1.ContentLength = postString.Length
            webRequest__1.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8"
            webRequest__1.Referer = "https://mafildemofetch.manappuram.com"
            System.Net.ServicePointManager.ServerCertificateValidationCallback = Function(se As Object, cert As System.Security.Cryptography.X509Certificates.X509Certificate, chain As System.Security.Cryptography.X509Certificates.X509Chain, sslerror As System.Net.Security.SslPolicyErrors) True
            Dim requestWriter As New StreamWriter(webRequest__1.GetRequestStream())
            requestWriter.Write(postString)
            requestWriter.Close()
            Dim responseReader As New StreamReader(webRequest__1.GetResponse().GetResponseStream())
            Dim responseData As String = responseReader.ReadToEnd()
            'MsgBox(responseData)
            'Dim resbyt As Byte() = Convert.FromBase64String(responseData)
            'Dim RESB64 As Byte() = FetchDecryptUsingSessionKey(resbyt)

            'Dim respdec As String = Convert.ToBase64String(RESB64)

            Return responseData
        Catch ex As Exception
            Return ex.Message.ToString()
        End Try
    End Function

    Public Function FetchDecryptUsingSessionKey(ByVal data As Byte()) As Byte()
        Try
        Dim STRSKEY As String = ConfigurationManager.AppSettings("SessionKey")
        Dim Skey As Byte() = Convert.FromBase64String(STRSKEY)
        Dim cipher As PaddedBufferedBlockCipher = New PaddedBufferedBlockCipher(New AesEngine(), New Pkcs7Padding())
        cipher.Init(False, New Org.BouncyCastle.Crypto.Parameters.KeyParameter(Skey))
        Dim outputSize As Integer = cipher.GetOutputSize(data.Length)
        Dim tempOP As Byte() = New Byte(outputSize - 1) {}
        Dim processLen As Integer = cipher.ProcessBytes(data, 0, data.Length, tempOP, 0)
        Dim outputLen As Integer = cipher.DoFinal(tempOP, processLen)
        Dim result As Byte() = New Byte(processLen + outputLen - 1) {}
        System.Array.Copy(tempOP, 0, result, 0, result.Length)
            Return result

        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Private Sub BtnDwnCnsnt_Click(sender As Object, e As EventArgs) Handles BtnDwnCnsnt.Click
        If cmb_lang.SelectedValue = 0 Then
            MsgBox("Select consent language..!!", MsgBoxStyle.OkOnly, "EKYC Customer")
            Exit Sub
        End If
        Dim obj As New customerService.customer(set_ip)
        Dim dt1 As DataTable
        Dim retval As String
        Dim door As String = cmb_lang.SelectedText
        Dim bl() As Byte
        btnAddKyc.Visible = True
        btngetkyc.Visible = True
        bl = obj.pdfcard(Me.cmb_lang.SelectedValue.ToString)
        Dim fnm As String = "MANAPPURAMEKYCCONSENT_" & Me.cmb_lang.SelectedValue.ToString & ".pdf"
        OpenDownloadDoc(fnm, bl)
        ' dt1 = oh.ExecuteDataSet("select  t.consent from DMS.TBL_EKYCONSENT t where t.language=" & cmb_lang.SelectedItem).Tables(0)
        ' retval = obj.pdfcard(door)
        ' bl = CType(dt1.Rows(0)("ESI_CARD"), Byte())
        'My.Computer.Network.DownloadFile(retval, "C:\Users\338763\Downloads")

        ' bl = CType(dt1.Rows(0)("ESI_CARD"), Byte()) '  My.Computer.Network.DownloadFile(retval)        'If Not (IsDBNull(dt1.Rows(0)(0))) Then
        '    Dim bl() As Byte
        '    bl = CType(dt1.Rows(0)("ESI_CARD"), Byte())
        '    Response.Buffer = True
        '    Response.Charset = ""
        '    Response.Cache.SetCacheability(HttpCacheability.NoCache)
        '    Response.ContentType = "application/pdf" ' pdf format
        '    Response.AppendHeader("Content-Length", CStr(bl.Length))
        '    Response.AddHeader("content-disposition", "attachment;filename=esidocument.pdf")
        '    Response.BinaryWrite(bl)
        '    Response.Flush()
        '    Response.End()
        'End If




    End Sub

    Friend Function OpenDownloadDoc(ByVal fname As String, _
                                 ByVal bytBLOBData() As Byte) As Boolean
        Dim binwrt As BinaryWriter
        Try
            '  With binwrt
            'Dim flstrm As System.IO.FileStream = New System.IO.FileStream(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\" & fname, FileMode.OpenOrCreate, FileAccess.Write)
            '     binwrt = New BinaryWriter(flstrm)
            '    .Write(bytBLOBData)
            '    .Flush()
            '    .Close()
            '    flstrm.Close()
            If File.Exists(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\" & fname) Then
                Process.Start("explorer.exe", System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\" & fname)
                '  File.Delete(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\" & fname)
                MessageBox.Show("EKYC CONSENT File already opened ! please close the PDF file ..." + System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\" & fname)
            Else
                File.WriteAllBytes(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\" & fname, bytBLOBData)
                Process.Start("explorer.exe", System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\" & fname)

                '   System.Diagnostics.Process.Start(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\" & fname)
                '  End With
                Return True
            End If
         
          
            
        Catch ex As Exception
            MsgBox(ex.Message)
            Return False
        End Try
    End Function

    Private Sub TabCustDtl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabCustDtl.Click
        '   If Me.txtCustomerid.Text <> "" Then
        Me.Main_tab.TabPages(1).Enabled = True

    End Sub

    Private Sub TabCustDtl_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabCustDtl.Enter
        Dim objser As New customerService.customer
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim dtSearch As New DataTable
        PanFlg = "0"
        NriFlg = "0"
        ClearCkycDtl()
        If CkycFlag = 0 And txtCustomerid.Text = "" Then
            MsgBox("Search Customer Details.")
            Me.Main_tab.SelectedTab = Me.tb_search
            Exit Sub
        ElseIf CkycFlag = 1 And txtCustId.Text = "" Then
            MsgBox("No Customer ID Found.", MsgBoxStyle.OkOnly, "CKYC")
            Me.Main_tab.SelectedTab = Me.tb_addcustomer
            Exit Sub
        Else
            If CkycFlag = 0 Then
                TxtCust_Id.Text = txtCustomerid.Text
                TxtCust_Name.Text = txtcustname.Text
                TxtFatFname.Text = txtfathus.Text
                ds = objser.CKYC_FILL(txtCustomerid.Text)
                dtSearch = ds.Tables("CKYCDTL")
                If dtSearch.Rows.Count > 0 Then
                    MsgBox("CKYC details already added..!!", MsgBoxStyle.OkOnly, "CKYC Customer")
                    Me.Main_tab.SelectedTab = Me.tb_search
                    Exit Sub
                End If
            ElseIf CkycFlag = 1 Then
                TxtCust_Id.Text = txtCustId.Text
                TxtCust_Name.Text = CuName
                TxtFatFname.Text = FaName
                ds = objser.CKYC_FILL(txtCustId.Text)
            End If

            dtSearch = ds.Tables("NAMEPFX")
            Me.CmbCustPfx.DataSource = Addhead(dtSearch, ds.Tables("NAMEPFX").Columns(1).ColumnName, ds.Tables("NAMEPFX").Columns(0).ColumnName)
            Me.CmbCustPfx.DisplayMember = dtSearch.Columns(1).ColumnName
            Me.CmbCustPfx.ValueMember = dtSearch.Columns(0).ColumnName

            dtSearch = ds.Tables("FATSPOUSE")
            Me.CmbFatSpouse.DataSource = Addhead(dtSearch, ds.Tables("FATSPOUSE").Columns(1).ColumnName, ds.Tables("FATSPOUSE").Columns(0).ColumnName)
            Me.CmbFatSpouse.DisplayMember = dtSearch.Columns(1).ColumnName
            Me.CmbFatSpouse.ValueMember = dtSearch.Columns(0).ColumnName

            dtSearch = ds.Tables("MALEPFX")
            Me.CmbFatPfx.DataSource = Addhead(dtSearch, ds.Tables("MALEPFX").Columns(1).ColumnName, ds.Tables("MALEPFX").Columns(0).ColumnName)
            Me.CmbFatPfx.DisplayMember = dtSearch.Columns(1).ColumnName
            Me.CmbFatPfx.ValueMember = dtSearch.Columns(0).ColumnName

            dtSearch = ds.Tables("FEMALEPFX")
            Me.CmbMotPfx.DataSource = Addhead(dtSearch, ds.Tables("FEMALEPFX").Columns(1).ColumnName, ds.Tables("FEMALEPFX").Columns(0).ColumnName)
            Me.CmbMotPfx.DisplayMember = dtSearch.Columns(1).ColumnName
            Me.CmbMotPfx.ValueMember = dtSearch.Columns(0).ColumnName

            dtSearch = ds.Tables("ADDRTYP")
            Me.CmbAddrTyp.DataSource = Addhead(dtSearch, ds.Tables("ADDRTYP").Columns(1).ColumnName, ds.Tables("ADDRTYP").Columns(0).ColumnName)
            Me.CmbAddrTyp.DisplayMember = dtSearch.Columns(1).ColumnName
            Me.CmbAddrTyp.ValueMember = dtSearch.Columns(0).ColumnName

            dtSearch = ds.Tables("country_dtl")
            Me.CmbJurisRes.DataSource = Addhead(dtSearch, ds.Tables("country_dtl").Columns(1).ColumnName, ds.Tables("country_dtl").Columns(0).ColumnName)
            Me.CmbJurisRes.DisplayMember = dtSearch.Columns(1).ColumnName
            Me.CmbJurisRes.ValueMember = dtSearch.Columns(0).ColumnName

            dtSearch = ds.Tables("country_dtl1")
            Me.CmbBirCtry.DataSource = Addhead(dtSearch, ds.Tables("country_dtl1").Columns(1).ColumnName, ds.Tables("country_dtl1").Columns(0).ColumnName)
            Me.CmbBirCtry.DisplayMember = dtSearch.Columns(1).ColumnName
            Me.CmbBirCtry.ValueMember = dtSearch.Columns(0).ColumnName

            dtSearch = ds.Tables("CUSTDTL")
            If (Not IsNothing(dtSearch) AndAlso dtSearch.Rows.Count > 0) Then
                NriFlg = dtSearch.Rows(0)(0).ToString()
                PanFlg = dtSearch.Rows(0)(1).ToString()
                CtryFlg = dtSearch.Rows(0)(2).ToString()
            End If
            If NriFlg <> "2" And CtryFlg = "1" And PanFlg <> "23" Then
                MsgBox("CKYC details already added..!!", MsgBoxStyle.OkOnly, "CKYC Customer")
                Me.Main_tab.SelectedTab = Me.tb_search
                Exit Sub
            End If
            If NriFlg <> "2" And CtryFlg = "1" Then
                CmbJurisRes.Enabled = False
                CmbBirCtry.Enabled = False
                TxtTaxId.Enabled = False
            Else
                CmbJurisRes.Enabled = True
                CmbBirCtry.Enabled = True
                TxtTaxId.Enabled = True
            End If

            If PanFlg = "23" Then
                CmbFatSpouse.SelectedValue = 1
                CmbFatSpouse.Enabled = False
                LblPanMsg.Visible = True
                TxtFatFname.Text = ""
            Else
                CmbFatSpouse.SelectedValue = 1
                CmbFatSpouse.Enabled = False
                LblPanMsg.Visible = False
            End If

            dtSearch = ds.Tables("CKYCDTL")
            Dim EmpPost As Integer = objser.GetEmpPostID(UserId)
            If dtSearch.Rows.Count = 0 Then
                'If EmpPost = 10 Or EmpPost = 198 Then
                '    MsgBox("You don't have permission to enter CKYC details..!!", , "CKYC Customer")
                '    Me.Main_tab.SelectedTab = Me.tb_search
                '    Exit Sub
                'Else
                GrpCkyc.Enabled = True
                UserFlg = 1
                'End If
            ElseIf dtSearch.Rows.Count > 0 Then
                'MsgBox("You don't have permission to confirm CKYC details..!!", , "CKYC Customer")
                MsgBox("CKYC details already added..!!", MsgBoxStyle.OkOnly, "CKYC Customer")
                Me.Main_tab.SelectedTab = Me.tb_search
                Exit Sub
            End If
        End If
    End Sub

    Sub ClearCkycDtl()
        TxtCustFName.Text = ""
        TxtCustMName.Text = ""
        TxtCustLName.Text = ""

        TxtFatFname.Text = ""
        TxtFatMname.Text = ""
        TxtFatLname.Text = ""

        TxtMotFname.Text = ""
        TxtMotMname.Text = ""
        TxtMotLname.Text = ""

        TxtTaxId.Text = ""
    End Sub

    Private Sub BtnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Try
            'If CmbCustPfx.SelectedValue = 0 Then
            '    MsgBox("Select Customer Name Prefix..!!", , "CKYC Customer")
            '    Exit Sub
            'ElseIf TxtCustFName.Text.Trim() = "" Then
            '    TxtCustFName.Text = ""
            '    MsgBox("Enter Customer First Name..!!", , "CKYC Customer")
            '    Exit Sub
            'ElseIf TxtCustMName.Text.Trim = "" Then
            '    TxtCustMName.Text = ""
            '    MsgBox("Enter Customer Middle Name..!!", , "CKYC Customer")
            '    Exit Sub
            'ElseIf TxtCustLName.Text.Trim = "" Then
            '    TxtCustLName.Text = ""
            '    MsgBox("Enter Customer Last Name..!!", , "CKYC Customer")
            '    Exit Sub

            If CmbFatSpouse.SelectedValue = 0 Then
                MsgBox("Select Father/Spouse..!!", , "CKYC Customer")
                Exit Sub
            ElseIf CmbFatPfx.SelectedValue = 0 Then
                MsgBox("Select Father/Spouse Prefix..!!", , "CKYC Customer")
                Exit Sub
            ElseIf TxtFatFname.Text.Trim = "" Then
                TxtFatFname.Text = ""
                MsgBox("Enter Father/Spouse First Name..!!", , "CKYC Customer")
                Exit Sub
            End If
            'ElseIf TxtFatMname.Text.Trim = "" Then
            '    TxtFatMname.Text = ""
            '    MsgBox("Enter Father/Spouse Middle Name..!!", , "CKYC Customer")
            '    Exit Sub
            'ElseIf TxtFatLname.Text.Trim = "" Then
            '    TxtFatLname.Text = ""
            '    MsgBox("Enter Father/Spouse Last Name..!!", , "CKYC Customer")
            '    Exit Sub
            'ElseIf TxtFatLname.Text.Trim = "" Then
            '    TxtFatLname.Text = ""
            '    MsgBox("Enter Father/Spouse Last Name..!!", , "CKYC Customer")
            '    Exit Sub
            'ElseIf CmbMotPfx.SelectedValue = 0 Then
            '    MsgBox("Select Mother Prefix..!!", , "CKYC Customer")
            '    Exit Sub
            'ElseIf TxtMotFname.Text.Trim = "" Then
            '    TxtMotFname.Text = ""
            '    MsgBox("Enter Mother's First Name..!!", , "CKYC Customer")
            '    Exit Sub
            'ElseIf TxtMotMname.Text.Trim = "" Then
            '    TxtMotMname.Text = ""
            '    MsgBox("Enter Mother's Middle Name..!!", , "CKYC Customer")
            '    Exit Sub
            'ElseIf TxtMotLname.Text.Trim = "" Then
            '    TxtMotLname.Text = ""
            '    MsgBox("Enter Mother's Last Name..!!", , "CKYC Customer")
            '    Exit Sub
            If (NriFlg = "2" Or CtryFlg <> "1") And CmbJurisRes.SelectedValue = 0 Then
                MsgBox("Select Jurisdiction of Residence..!!", , "CKYC Customer")
                Exit Sub
            ElseIf (NriFlg = "2" Or CtryFlg <> "1") And CmbBirCtry.SelectedValue = 0 Then
                MsgBox("Select Birth Country..!!", , "CKYC Customer")
                Exit Sub
                'ElseIf CmbAddrTyp.SelectedValue = 0 Then
                '    MsgBox("Select Address Type..!!", , "CKYC Customer")
                '    Exit Sub
            End If

            Dim objser As New customerService.customer
            Dim resp As String = objser.AddCkycDtl(TxtCust_Id.Text, CmbCustPfx.SelectedValue, TxtCustFName.Text, TxtCustMName.Text, TxtCustLName.Text, CmbFatSpouse.SelectedValue, CmbFatPfx.SelectedValue, TxtFatFname.Text, TxtFatMname.Text, TxtFatLname.Text, CmbMotPfx.SelectedValue, TxtMotFname.Text, TxtMotMname.Text, TxtMotLname.Text, CmbJurisRes.SelectedValue, TxtTaxId.Text, CmbBirCtry.SelectedValue, CmbAddrTyp.SelectedValue, UserId, UserFlg)
            MsgBox(resp, , "CKYC Customer")
            Me.Main_tab.SelectedTab = Me.tb_addcustomer

        Catch ex As Exception
            MsgBox(ex.Message.ToString(), , "EKYC Customer")
        End Try

    End Sub

    Private Sub TxtCustFName_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TxtCustFName.KeyPress
        e.KeyChar = CChar(e.KeyChar.ToString.ToUpper)

        If Not (Asc(e.KeyChar) = 8) Then
            If Not e.KeyChar = " " Then
                Dim allowedChars As String = "abcdefghijklmnopqrstuvwxyz"
                If Not allowedChars.Contains(e.KeyChar.ToString.ToLower) Then
                    e.KeyChar = ChrW(0)
                    e.Handled = True
                End If
            End If
        End If
    End Sub

    Private Sub TxtCustMName_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TxtCustMName.KeyPress
        e.KeyChar = CChar(e.KeyChar.ToString.ToUpper)

        If Not (Asc(e.KeyChar) = 8) Then
            If Not e.KeyChar = " " Then
                Dim allowedChars As String = "abcdefghijklmnopqrstuvwxyz"
                If Not allowedChars.Contains(e.KeyChar.ToString.ToLower) Then
                    e.KeyChar = ChrW(0)
                    e.Handled = True
                End If
            End If
        End If
    End Sub

    Private Sub TxtCustLName_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TxtCustLName.KeyPress
        e.KeyChar = CChar(e.KeyChar.ToString.ToUpper)

        If Not (Asc(e.KeyChar) = 8) Then
            If Not e.KeyChar = " " Then
                Dim allowedChars As String = "abcdefghijklmnopqrstuvwxyz"
                If Not allowedChars.Contains(e.KeyChar.ToString.ToLower) Then
                    e.KeyChar = ChrW(0)
                    e.Handled = True
                End If
            End If
        End If
    End Sub

    Private Sub TxtFatFname_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TxtFatFname.KeyPress
        e.KeyChar = CChar(e.KeyChar.ToString.ToUpper)

        If Not (Asc(e.KeyChar) = 8) Then
            If Not e.KeyChar = " " Then
                Dim allowedChars As String = "abcdefghijklmnopqrstuvwxyz"
                If Not allowedChars.Contains(e.KeyChar.ToString.ToLower) Then
                    e.KeyChar = ChrW(0)
                    e.Handled = True
                End If
            End If
        End If
    End Sub

    Private Sub TxtFatMname_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TxtFatMname.KeyPress
        e.KeyChar = CChar(e.KeyChar.ToString.ToUpper)

        If Not (Asc(e.KeyChar) = 8) Then
            If Not e.KeyChar = " " Then
                Dim allowedChars As String = "abcdefghijklmnopqrstuvwxyz"
                If Not allowedChars.Contains(e.KeyChar.ToString.ToLower) Then
                    e.KeyChar = ChrW(0)
                    e.Handled = True
                End If
            End If
        End If
    End Sub

    Private Sub TxtFatLname_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TxtFatLname.KeyPress
        e.KeyChar = CChar(e.KeyChar.ToString.ToUpper)

        If Not (Asc(e.KeyChar) = 8) Then
            If Not e.KeyChar = " " Then
                Dim allowedChars As String = "abcdefghijklmnopqrstuvwxyz"
                If Not allowedChars.Contains(e.KeyChar.ToString.ToLower) Then
                    e.KeyChar = ChrW(0)
                    e.Handled = True
                End If
            End If
        End If
    End Sub

    Private Sub TxtMotFname_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TxtMotFname.KeyPress
        e.KeyChar = CChar(e.KeyChar.ToString.ToUpper)

        If Not (Asc(e.KeyChar) = 8) Then
            If Not e.KeyChar = " " Then
                Dim allowedChars As String = "abcdefghijklmnopqrstuvwxyz"
                If Not allowedChars.Contains(e.KeyChar.ToString.ToLower) Then
                    e.KeyChar = ChrW(0)
                    e.Handled = True
                End If
            End If
        End If
    End Sub

    Private Sub TxtMotMname_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TxtMotMname.KeyPress
        e.KeyChar = CChar(e.KeyChar.ToString.ToUpper)

        If Not (Asc(e.KeyChar) = 8) Then
            If Not e.KeyChar = " " Then
                Dim allowedChars As String = "abcdefghijklmnopqrstuvwxyz"
                If Not allowedChars.Contains(e.KeyChar.ToString.ToLower) Then
                    e.KeyChar = ChrW(0)
                    e.Handled = True
                End If
            End If
        End If
    End Sub

    Private Sub TxtMotLname_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TxtMotLname.KeyPress
        e.KeyChar = CChar(e.KeyChar.ToString.ToUpper)

        If Not (Asc(e.KeyChar) = 8) Then
            If Not e.KeyChar = " " Then
                Dim allowedChars As String = "abcdefghijklmnopqrstuvwxyz"
                If Not allowedChars.Contains(e.KeyChar.ToString.ToLower) Then
                    e.KeyChar = ChrW(0)
                    e.Handled = True
                End If
            End If
        End If
    End Sub

    Private Sub FillCkycDtls(ByVal dt As DataTable)
        CmbCustPfx.SelectedValue = dt.Rows(0)("NAME_PFX").ToString()
        TxtCustFName.Text = dt.Rows(0)("FIRST_NAME").ToString()
        TxtCustMName.Text = dt.Rows(0)("MIDDLE_NAME").ToString()
        TxtCustLName.Text = dt.Rows(0)("LAST_NAME").ToString()
        CmbFatPfx.SelectedValue = dt.Rows(0)("FAT_PFX").ToString()
        TxtFatFname.Text = dt.Rows(0)("FAT_F_NAME").ToString()
        TxtFatMname.Text = dt.Rows(0)("FAT_M_NAME").ToString()
        TxtFatLname.Text = dt.Rows(0)("FAT_L_NAME").ToString()
        CmbFatSpouse.SelectedValue = dt.Rows(0)("FAT_SPOUSE_FLG").ToString()
        CmbMotPfx.SelectedValue = dt.Rows(0)("MOTHER_PFX").ToString()
        TxtMotFname.Text = dt.Rows(0)("MOTH_F_NAME").ToString()
        TxtMotMname.Text = dt.Rows(0)("MOTH_M_NAME").ToString()
        TxtMotLname.Text = dt.Rows(0)("MOTH_L_NAME").ToString()
        CmbJurisRes.SelectedValue = dt.Rows(0)("JURIS_RESID").ToString()
        TxtTaxId.Text = dt.Rows(0)("TAX_ID_NUM").ToString()
        CmbBirCtry.SelectedValue = dt.Rows(0)("BIRTH_COUNTRY").ToString()
        CmbAddrTyp.SelectedValue = dt.Rows(0)("ADDR_TYPE").ToString()
    End Sub

    Private Sub TxtTaxId_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        e.KeyChar = CChar(e.KeyChar.ToString.ToUpper)

        If Not (Asc(e.KeyChar) = 8) Then
            If Not e.KeyChar = " " Then
                Dim allowedChars As String = "abcdefghijklmnopqrstuvwxyz"
                If Not allowedChars.Contains(e.KeyChar.ToString.ToLower) Then
                    e.KeyChar = ChrW(0)
                    e.Handled = True
                End If
            End If
        End If
    End Sub
   
End Class
