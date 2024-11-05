Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Data
Imports System.Data.OracleClient
Imports System.Xml
Imports System.IO
Imports System.Drawing.Bitmap
Imports System.Security.Cryptography

<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class customer
    Inherits System.Web.Services.WebService
    Dim oh As New Helper.Oracle.OracleHelper
    Dim sqlDSA, DSABAresult As String

    <WebMethod()> _
       Public Function passwordCheck(ByVal userId As Integer, ByVal passWord As String) As String
        Dim oh1 As New Helper.Oracle.OracleHelper
        Dim result As String
        Dim ps As New passwdClass
        result = ps.getRoles(userId, passWord)
        result = Encrypt(result + "~" + GenerateRandomnum(), "m@f1l992")
        Return result
    End Function

    '<WebMethod()> _
    '   Public Function passwordCheck(ByVal userId As Integer, ByVal passWord As String) As String 'UAT
    '    Dim oh1 As New Helper.Oracle.OracleHelper
    '    Dim result As String
    '    Dim ps As New passwdClass
    '    result = ps.getRoles(userId, passWord)
    '    result = Encrypt(result + "~" + GenerateRandomnum(), "m@f1l992")
    '    Return result
    'End Function

    Protected Function GenerateRandomnum() As String
        Dim alphabets As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        Dim small_alphabets As String = "abcdefghijklmnopqrstuvwxyz"
        Dim numbers As String = "1234567890"
        Dim characters As String = ""
        characters += Convert.ToString(alphabets & small_alphabets) & numbers
        Dim length As Integer = 10
        Dim otp As String = String.Empty
        Dim random As String = String.Empty
        For i As Integer = 0 To length - 1
            Dim character As String = String.Empty
            Do
                Dim index As Integer = New Random().[Next](0, characters.Length)
                character = characters.ToCharArray()(index).ToString()
            Loop While otp.IndexOf(character) <> -1

            otp += character
        Next

        random = otp
        Return random
    End Function

    Public Function Encrypt(ByVal stringToEncrypt As String, ByVal pass As String) As String
        Dim inputByteArray() As Byte = Encoding.UTF8.GetBytes(stringToEncrypt)
        Dim rgbIV() As Byte = New Byte() {33, 67, 86, 135, 16, 253, 234, 28}
        Try
            Dim key() As Byte = Encoding.UTF8.GetBytes(pass)
            Dim des As DESCryptoServiceProvider = New DESCryptoServiceProvider
            Dim ms As MemoryStream = New MemoryStream
            Dim cs As CryptoStream = New CryptoStream(ms, des.CreateEncryptor(key, rgbIV), CryptoStreamMode.Write)
            cs.Write(inputByteArray, 0, inputByteArray.Length)
            cs.FlushFinalBlock()
            Return Convert.ToBase64String(ms.ToArray)
        Catch e As Exception
            Return e.Message
        End Try

    End Function


    <WebMethod()> _
     Public Function getPrinter(ByVal brno As Integer, ByVal keyVal As Integer) As String
        Dim oh As New Helper.Oracle.OracleHelper
        Dim dt As New DataTable
        Dim printerName As String
        dt = oh.ExecuteDataSet("select '\\'||system_ip||'\'||printer_name from branch_printer where branch_id=" & brno & " and key=" & keyVal & " and status=4").Tables(0)
        printerName = dt.Rows(0)(0)
        Return printerName
    End Function
    <WebMethod()> _
        Public Function Combofill(ByVal branchid As Integer) As DataSet
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim country_id, state_id, district_id, relgionid As String
        Try
            dt = oh.ExecuteDataSet("select * from identity id where identity_id in(1,14,3,2,4,16,5) order by id.identity_name").Tables(0).Copy
            dt.TableName = "identity"
            ds.Tables.Add(dt)
            dt = oh.ExecuteDataSet("select occupation_id, occupation_name from occupation_master om where om.status=1 and om.occupation_id <> 28 order by om.occupation_name").Tables(0).Copy
            dt.TableName = "occupation_master"
            ds.Tables.Add(dt)
            dt = oh.ExecuteDataSet("select type_id,descr from customer_type").Tables(0).Copy
            dt.TableName = "customer_type"
            ds.Tables.Add(dt)
            dt = oh.ExecuteDataSet("select country_id,country_name from country_dtl  order by country_id ").Tables(0).Copy
            dt.TableName = "country_dtl"
            country_id = dt.Rows(0)(0).ToString
            ds.Tables.Add(dt)
            dt = oh.ExecuteDataSet("select state_id,state_name from state_master  order by state_name").Tables(0).Copy
            dt.TableName = "state_master"
            state_id = dt.Rows(0)(0).ToString
            ds.Tables.Add(dt)
            dt = oh.ExecuteDataSet("select district_id,district_name from district_master where state_id in (select state_id from branch_master  where branch_id=" & branchid & ")  order by district_name").Tables(0).Copy
            dt.TableName = "district_master"
            district_id = dt.Rows(0)(0).ToString
            ds.Tables.Add(dt)
            dt = oh.ExecuteDataSet("select pin_code ||'@'||sr_number as pincode,post_office from post_master where district_id in (select district_id from branch_master  where branch_id=" & branchid & ") order by post_office").Tables(0).Copy
            dt.TableName = "post_master"
            ds.Tables.Add(dt)
            dt = oh.ExecuteDataSet("select t.type_id, t.type_name  from media_type_new t where t.type_id = 10 union all select type_id, type_name   from media_type_new m where m.type_id <> 10").Tables(0).Copy
            dt.TableName = "media_type"
            ds.Tables.Add(dt)
            dt = oh.ExecuteDataSet("select media_id,media_name from media_master_new where type_id= " & ds.Tables("media_type").Rows(0)(0)).Tables(0).Copy
            dt.TableName = "media_master"
            ds.Tables.Add(dt)
            dt = oh.ExecuteDataSet("select module_id,mod_abr from mod_master  order by module_id").Tables(0).Copy
            dt.TableName = "mod_master"
            ds.Tables.Add(dt)
            dt = oh.ExecuteDataSet("select religion_id,religion from religion_master  order by religion_id").Tables(0).Copy
            dt.TableName = "religion_master"
            ds.Tables.Add(dt)
            relgionid = dt.Rows(0)(0).ToString
            dt = oh.ExecuteDataSet("select caste_id,caste_name from caste_master  where religion_id=" & relgionid & " order by caste_id").Tables(0).Copy
            dt.TableName = "caste_master"
            ds.Tables.Add(dt)
            dt = oh.ExecuteDataSet("select prename_id||'~'||gender prename_id,prename from prename_master order by prename_id").Tables(0).Copy
            dt.TableName = "prename_master"
            ds.Tables.Add(dt)
            dt = oh.ExecuteDataSet("select relation_id,relation_name from relation_dtl where relation_id>20  order by relation_id").Tables(0).Copy
            dt.TableName = "relation_dtl"
            ds.Tables.Add(dt)
            '###-SEBIN------------------------------------------------------------------------------------------------------------------
            'dt = oh.ExecuteDataSet("select relation_id,relation_name from relation_dtl where relation_id in (1,19)order by relation_id").Tables(0).Copy
            'dt.TableName = "relation_dtl"
            'ds.Tables.Add(dt)
            '--------------------------------------------------------------------------------------------------------------------
            dt = oh.ExecuteDataSet("select t.status_id,t.description from status_master t where t.module_id=1 and t.option_id=11 and status_id > 2").Tables(0).Copy
            dt.TableName = "cmb_landHldtl"
            ds.Tables.Add(dt)

            dt = oh.ExecuteDataSet("select t.status_id,t.description from status_master t where t.module_id=1 and t.option_id=14").Tables(0).Copy
            dt.TableName = "cmb_cerDtls"
            ds.Tables.Add(dt)

            dt = oh.ExecuteDataSet("select t.status_id,t.description from status_master t where t.module_id=1 and t.option_id=32 order by order_by").Tables(0).Copy
            dt.TableName = "loan_purpose"
            ds.Tables.Add(dt)

            dt = oh.ExecuteDataSet("select t.status_id, t.description from STATUS_MASTER t where t.module_id = 890 and t.option_id = 1").Tables(0).Copy
            dt.TableName = "lang"
            ds.Tables.Add(dt)
            '------------------------------------- SEBIN JOSEPH -------------------------------------------------------------------------------------------------
            dt = oh.ExecuteDataSet("select P.PEP_ID,P.PEP_DESCRIPTION from CUST_PEP P where P.ISACTIVE = 1 order by P.PEP_ID").Tables(0).Copy
            dt.TableName = "CUST_PEP"
            ds.Tables.Add(dt)
                 '---------------------------------------------------------------------------------------------------------------------------------------------
            dt = oh.ExecuteDataSet("select t.status_id, t.description from STATUS_MASTER t where t.module_id = 892 and t.option_id = 1").Tables(0).Copy
            dt.TableName = "MARITAL"
            ds.Tables.Add(dt)

            dt = oh.ExecuteDataSet("select t.status_id, t.description from STATUS_MASTER t where t.module_id = 893 and t.option_id = 1").Tables(0).Copy
            dt.TableName = "CITIZEN"
            ds.Tables.Add(dt)

            dt = oh.ExecuteDataSet("select t.status_id, t.description from STATUS_MASTER t where t.module_id = 894 and t.option_id = 1").Tables(0).Copy
            dt.TableName = "NATION"
            ds.Tables.Add(dt)

            dt = oh.ExecuteDataSet("select t.status_id, t.description from STATUS_MASTER t where t.module_id = 895 and t.option_id = 1").Tables(0).Copy
            dt.TableName = "RESIDENT"
            ds.Tables.Add(dt)

            dt = oh.ExecuteDataSet("select t.status_id, t.description from STATUS_MASTER t where t.module_id = 896 and t.option_id = 1").Tables(0).Copy
            dt.TableName = "EDUCATION"
            ds.Tables.Add(dt)

            dt = oh.ExecuteDataSet("select t.status_id, t.description from STATUS_MASTER t where t.module_id = 896 and t.option_id = 2").Tables(0).Copy
            dt.TableName = "INCOME"
            ds.Tables.Add(dt)

            dt = oh.ExecuteDataSet("select t.status_id, t.description from STATUS_MASTER t where t.module_id = 896 and t.option_id = 3").Tables(0).Copy
            dt.TableName = "NEEDFORLOAN"
            ds.Tables.Add(dt)

            dt = oh.ExecuteDataSet("select t.status_id, t.description from STATUS_MASTER t where t.module_id = 896 and t.option_id = 4").Tables(0).Copy
            dt.TableName = "FIRSTLOAN"
            ds.Tables.Add(dt)

            dt = oh.ExecuteDataSet("select t.serial_number, t.language from dms.tbl_ekyconsent t").Tables(0).Copy
            dt.TableName = "CONSENTLANG"
            ds.Tables.Add(dt)

        Catch ex As Exception
        End Try
        Return ds
    End Function
    <WebMethod()> _
    Public Function language_id() As DataTable
        Dim ds As New DataTable
        Dim stated As String
        ds = oh.ExecuteDataSet("select t.language from tbl_ekyconsent t").Tables(0)
        stated = ds.Rows(0)(0)
        Return ds
    End Function
    <WebMethod()> _
    Public Function branchState(ByVal brno As Integer) As Integer
        Dim ds As New DataTable
        Dim stateid As Integer
        ds = oh.ExecuteDataSet("select state_id from branch_master  where branch_id=" & brno).Tables(0)
        stateid = ds.Rows(0)(0)
        Return stateid
    End Function

    <WebMethod()> _
    Public Function branchDistrict(ByVal brno As Integer) As Integer
        Dim ds As New DataTable
        Dim districtid As Integer
        ds = oh.ExecuteDataSet("select district_id from branch_master  where branch_id=" & brno).Tables(0)
        districtid = ds.Rows(0)(0)
        Return districtid
    End Function
    <WebMethod()> _
    Public Function pdfcard(ByVal pdf As String) As Byte()
        Dim ds As New DataTable
        Dim dtl() As Byte
        Dim stare As String
        stare = "select  t.consent from DMS.TBL_EKYCONSENT t where t.SERIAL_NUMBER=" & pdf
        ds = oh.ExecuteDataSet(stare).Tables(0)
        dtl = ds.Rows(0)(0)
        Return dtl
    End Function

    <WebMethod()> _
    Public Function QueryResult(ByVal query As String) As DataSet
        Dim ds As New DataSet
        ds = oh.ExecuteDataSet(query)
        Return ds
    End Function
    <WebMethod()> _
    Public Function customeradd(ByVal data As String, ByVal firmid As Integer, ByVal branchid As Integer, ByVal user_id As String, ByVal neftdata As String) As String
        Dim datastr() As String = data.Split("¥")
        Dim parm_coll(32) As OracleParameter
        Dim message As String
        Dim oh1 As New Helper.Oracle.OracleHelper
        Try
            parm_coll(0) = New OracleParameter("fmno", OracleType.Number, 5)
            parm_coll(0).Value = CInt(firmid)
            parm_coll(0).Direction = ParameterDirection.Input

            parm_coll(1) = New OracleParameter("brno", OracleType.Number, 5)
            parm_coll(1).Value = CInt(branchid)
            parm_coll(1).Direction = ParameterDirection.Input

            parm_coll(2) = New OracleParameter("cusname", OracleType.VarChar, 40)
            parm_coll(2).Value = datastr(0)
            'ADADA/                             0
            parm_coll(2).Direction = ParameterDirection.Input

            parm_coll(3) = New OracleParameter("tele", OracleType.VarChar, 15)
            parm_coll(3).Value = datastr(12)
            '21323123123                        12
            parm_coll(3).Direction = ParameterDirection.Input

            parm_coll(4) = New OracleParameter("mob", OracleType.VarChar, 15)
            parm_coll(4).Value = datastr(13)
            '13123                              13
            parm_coll(4).Direction = ParameterDirection.Input

            parm_coll(5) = New OracleParameter("fathus", OracleType.VarChar, 40)
            parm_coll(5).Value = datastr(1)
            'ADASD                              1
            parm_coll(5).Direction = ParameterDirection.Input

            parm_coll(6) = New OracleParameter("housenm", OracleType.VarChar, 40)
            parm_coll(6).Value = datastr(2)
            'QWEQEWQW                           2
            parm_coll(6).Direction = ParameterDirection.Input

            parm_coll(7) = New OracleParameter("loca", OracleType.VarChar, 100)
            parm_coll(7).Value = datastr(4)
            'EQWSCAS                            4
            parm_coll(7).Direction = ParameterDirection.Input

            parm_coll(8) = New OracleParameter("pinsrl", OracleType.Number, 7)
            Dim pinsr() As String = datastr(8).Split("@")
            parm_coll(8).Value = CInt(pinsr(1))
            '680554@4016                        8
            parm_coll(8).Direction = ParameterDirection.Input

            parm_coll(9) = New OracleParameter("email_id", OracleType.VarChar, 35)
            parm_coll(9).Value = datastr(14)
            'zczcas                             14
            parm_coll(9).Direction = ParameterDirection.Input

            parm_coll(10) = New OracleParameter("occ_id", OracleType.Number, 2)
            parm_coll(10).Value = CInt(datastr(11))
            '2                                  11
            parm_coll(10).Direction = ParameterDirection.Input

            parm_coll(11) = New OracleParameter("pass_id", OracleType.VarChar, 35)
            parm_coll(11).Value = ""
            parm_coll(11).Direction = ParameterDirection.Input

            parm_coll(12) = New OracleParameter("pan", OracleType.VarChar, 35)
            parm_coll(12).Value = datastr(29)
            parm_coll(12).Direction = ParameterDirection.Input

            Dim st As Date
            st = CDate(datastr(20))
            parm_coll(13) = New OracleParameter("dob", OracleType.DateTime, 11)
            parm_coll(13).Value = CDate(Format(st, "dd-MMM-yyyy"))
            '1                                  20
            parm_coll(13).Direction = ParameterDirection.Input

            parm_coll(14) = New OracleParameter("cust_type", OracleType.Number, 2)
            parm_coll(14).Value = CInt(datastr(10))
            '8                                  10  
            parm_coll(14).Direction = ParameterDirection.Input

            parm_coll(15) = New OracleParameter("cntid", OracleType.Number, 4)
            parm_coll(15).Value = CInt(datastr(5))
            '1                                  5
            parm_coll(15).Direction = ParameterDirection.Input

            parm_coll(16) = New OracleParameter("custid", OracleType.VarChar, 200)
            parm_coll(16).Direction = ParameterDirection.Output

            parm_coll(17) = New OracleParameter("id", OracleType.Number, 3)
            parm_coll(17).Value = CInt(datastr(15))
            '5                                  15
            parm_coll(17).Direction = ParameterDirection.Input

            parm_coll(18) = New OracleParameter("id_no", OracleType.VarChar, 20)
            parm_coll(18).Value = datastr(16)
            'ASDAS12312                         16
            parm_coll(18).Direction = ParameterDirection.Input

            Dim isdt As Date
            isdt = CDate(datastr(17))
            parm_coll(19) = New OracleParameter("is_dt", OracleType.DateTime, 11)
            parm_coll(19).Value = CDate(Format(isdt, "dd-MMM-yyyy"))
            'Friday, January 08, 2010           17
            parm_coll(19).Direction = ParameterDirection.Input

            Dim exdt As Date
            exdt = CDate(datastr(18))
            parm_coll(20) = New OracleParameter("ex_dt", OracleType.DateTime, 11)
            parm_coll(20).Value = CDate(Format(exdt, "dd-MMM-yyyy"))
            'Thursday, December 08, 2011        18
            parm_coll(20).Direction = ParameterDirection.Input

            parm_coll(21) = New OracleParameter("is_plce", OracleType.VarChar, 40)
            parm_coll(21).Value = datastr(19)
            'ASDADAFriday, October 09, 2009     19
            parm_coll(21).Direction = ParameterDirection.Input

            parm_coll(22) = New OracleParameter("gen", OracleType.Number, 2)
            parm_coll(22).Value = CInt(datastr(21))
            '1                                  21
            parm_coll(22).Direction = ParameterDirection.Input

            parm_coll(23) = New OracleParameter("p_street", OracleType.VarChar, 40)
            parm_coll(23).Value = datastr(3)
            'EQWSCAS                            3
            parm_coll(23).Direction = ParameterDirection.Input

            parm_coll(24) = New OracleParameter("p_media_id", OracleType.Number, 5)
            parm_coll(24).Value = CInt(datastr(22))
            '1                                  22
            parm_coll(24).Direction = ParameterDirection.Input

            parm_coll(25) = New OracleParameter("p_module_id", OracleType.Number, 8)
            parm_coll(25).Value = CInt(datastr(24))
            '1                                  24
            parm_coll(25).Direction = ParameterDirection.Input

            parm_coll(26) = New OracleParameter("userid", OracleType.VarChar, 40)
            parm_coll(26).Value = user_id
            parm_coll(26).Direction = ParameterDirection.Input
            '18                                 6
            '390                                7
            '680554                             9
            '1                                  25
            '                                   26
            '                                   28
            parm_coll(27) = New OracleParameter("typeid", OracleType.VarChar, 25)
            parm_coll(27).Value = datastr(23)
            '0                                  23
            parm_coll(27).Direction = ParameterDirection.Input

            parm_coll(28) = New OracleParameter("cardNo", OracleType.VarChar, 40)
            parm_coll(28).Value = datastr(27)
            'F                                  27
            parm_coll(28).Direction = ParameterDirection.Input

            parm_coll(29) = New OracleParameter("shareflag", OracleType.VarChar, 2)
            parm_coll(29).Value = datastr(28)

            parm_coll(29).Direction = ParameterDirection.Input

            parm_coll(30) = New OracleParameter("err_msg", OracleType.VarChar, 300)
            parm_coll(30).Direction = ParameterDirection.Output

            parm_coll(31) = New OracleParameter("err_stat", OracleType.Number, 1)
            parm_coll(31).Direction = ParameterDirection.Output

            parm_coll(32) = New OracleParameter("neftdetails", OracleType.VarChar, 1000)
            parm_coll(32).Direction = ParameterDirection.Input
            parm_coll(32).Value = neftdata

            'parm_coll(33) = New OracleParameter("LandHLD", OracleType.Number, 10)
            'parm_coll(33).Direction = ParameterDirection.Input
            'parm_coll(33).Value = CInt(datastr(30))

            Try
                oh1.ExecuteNonQuery("ADDCUSTOMER", parm_coll)
                message = parm_coll(30).Value.ToString & "+" & parm_coll(31).Value.ToString & "+" & parm_coll(16).Value.ToString
            Catch ex As Exception
                message = ex.Message
            End Try
        Catch ex As Exception
            message = ex.Message
        End Try
        Return message
    End Function

    <WebMethod()> _
    Public Function customeradd_new(ByVal data As String, ByVal firmid As Integer, ByVal branchid As Integer, ByVal user_id As String, ByVal neftdata As String) As String
        Dim datastr() As String = data.Split("¥")
        Dim parm_coll(33) As OracleParameter
        Dim message As String
        Dim oh1 As New Helper.Oracle.OracleHelper
        Try
            parm_coll(0) = New OracleParameter("fmno", OracleType.Number, 5)
            parm_coll(0).Value = CInt(firmid)
            parm_coll(0).Direction = ParameterDirection.Input

            parm_coll(1) = New OracleParameter("brno", OracleType.Number, 5)
            parm_coll(1).Value = CInt(branchid)
            parm_coll(1).Direction = ParameterDirection.Input

            parm_coll(2) = New OracleParameter("cusname", OracleType.VarChar, 40)
            parm_coll(2).Value = datastr(0)
            'ADADA/                             0
            parm_coll(2).Direction = ParameterDirection.Input

            parm_coll(3) = New OracleParameter("tele", OracleType.VarChar, 15)
            parm_coll(3).Value = datastr(12)
            '21323123123                        12
            parm_coll(3).Direction = ParameterDirection.Input

            parm_coll(4) = New OracleParameter("mob", OracleType.VarChar, 15)
            parm_coll(4).Value = datastr(13)
            '13123                              13
            parm_coll(4).Direction = ParameterDirection.Input

            parm_coll(5) = New OracleParameter("fathus", OracleType.VarChar, 40)
            parm_coll(5).Value = datastr(1)
            'ADASD                              1
            parm_coll(5).Direction = ParameterDirection.Input

            parm_coll(6) = New OracleParameter("housenm", OracleType.VarChar, 40)
            parm_coll(6).Value = datastr(2)
            'QWEQEWQW                           2
            parm_coll(6).Direction = ParameterDirection.Input

            parm_coll(7) = New OracleParameter("loca", OracleType.VarChar, 100)
            parm_coll(7).Value = datastr(4)
            'EQWSCAS                            4
            parm_coll(7).Direction = ParameterDirection.Input

            parm_coll(8) = New OracleParameter("pinsrl", OracleType.Number, 7)
            Dim pinsr() As String = datastr(8).Split("@")
            parm_coll(8).Value = CInt(pinsr(1))
            '680554@4016                        8
            parm_coll(8).Direction = ParameterDirection.Input

            parm_coll(9) = New OracleParameter("email_id", OracleType.VarChar, 35)
            parm_coll(9).Value = datastr(14)
            'zczcas                             14
            parm_coll(9).Direction = ParameterDirection.Input

            parm_coll(10) = New OracleParameter("occ_id", OracleType.Number, 2)
            parm_coll(10).Value = CInt(datastr(11))
            '2                                  11
            parm_coll(10).Direction = ParameterDirection.Input

            parm_coll(11) = New OracleParameter("pass_id", OracleType.VarChar, 35)
            parm_coll(11).Value = ""
            parm_coll(11).Direction = ParameterDirection.Input

            parm_coll(12) = New OracleParameter("pan", OracleType.VarChar, 35)
            parm_coll(12).Value = datastr(29)
            parm_coll(12).Direction = ParameterDirection.Input

            Dim st As Date
            st = CDate(datastr(20))
            parm_coll(13) = New OracleParameter("dob", OracleType.DateTime, 11)
            parm_coll(13).Value = CDate(Format(st, "dd-MMM-yyyy"))
            '1                                  20
            parm_coll(13).Direction = ParameterDirection.Input

            parm_coll(14) = New OracleParameter("cust_type", OracleType.Number, 2)
            parm_coll(14).Value = CInt(datastr(10))
            '8                                  10  
            parm_coll(14).Direction = ParameterDirection.Input

            parm_coll(15) = New OracleParameter("cntid", OracleType.Number, 4)
            parm_coll(15).Value = CInt(datastr(5))
            '1                                  5
            parm_coll(15).Direction = ParameterDirection.Input

            parm_coll(16) = New OracleParameter("custid", OracleType.VarChar, 200)
            parm_coll(16).Direction = ParameterDirection.Output

            parm_coll(17) = New OracleParameter("id", OracleType.Number, 3)
            parm_coll(17).Value = CInt(datastr(15))
            '5                                  15
            parm_coll(17).Direction = ParameterDirection.Input

            parm_coll(18) = New OracleParameter("id_no", OracleType.VarChar, 20)
            parm_coll(18).Value = datastr(16)
            'ASDAS12312                         16
            parm_coll(18).Direction = ParameterDirection.Input

            Dim isdt As Date
            isdt = CDate(datastr(17))
            parm_coll(19) = New OracleParameter("is_dt", OracleType.DateTime, 11)
            parm_coll(19).Value = CDate(Format(isdt, "dd-MMM-yyyy"))
            'Friday, January 08, 2010           17
            parm_coll(19).Direction = ParameterDirection.Input

            Dim exdt As Date
            exdt = CDate(datastr(18))
            parm_coll(20) = New OracleParameter("ex_dt", OracleType.DateTime, 11)
            parm_coll(20).Value = CDate(Format(exdt, "dd-MMM-yyyy"))
            'Thursday, December 08, 2011        18
            parm_coll(20).Direction = ParameterDirection.Input

            parm_coll(21) = New OracleParameter("is_plce", OracleType.VarChar, 40)
            parm_coll(21).Value = datastr(19)
            'ASDADAFriday, October 09, 2009     19
            parm_coll(21).Direction = ParameterDirection.Input

            parm_coll(22) = New OracleParameter("gen", OracleType.Number, 2)
            parm_coll(22).Value = CInt(datastr(21))
            '1                                  21
            parm_coll(22).Direction = ParameterDirection.Input

            parm_coll(23) = New OracleParameter("p_street", OracleType.VarChar, 40)
            parm_coll(23).Value = datastr(3)
            'EQWSCAS                            3
            parm_coll(23).Direction = ParameterDirection.Input

            parm_coll(24) = New OracleParameter("p_media_id", OracleType.Number, 5)
            parm_coll(24).Value = CInt(datastr(22))
            '1                                  22
            parm_coll(24).Direction = ParameterDirection.Input

            parm_coll(25) = New OracleParameter("p_module_id", OracleType.Number, 8)
            parm_coll(25).Value = CInt(datastr(24))
            '1                                  24
            parm_coll(25).Direction = ParameterDirection.Input

            parm_coll(26) = New OracleParameter("userid", OracleType.VarChar, 40)
            parm_coll(26).Value = user_id
            parm_coll(26).Direction = ParameterDirection.Input
            '18                                 6
            '390                                7
            '680554                             9
            '1                                  25
            '                                   26
            '                                   28
            parm_coll(27) = New OracleParameter("typeid", OracleType.VarChar, 25)
            parm_coll(27).Value = datastr(23)
            '0                                  23
            parm_coll(27).Direction = ParameterDirection.Input

            parm_coll(28) = New OracleParameter("cardNo", OracleType.VarChar, 40)
            parm_coll(28).Value = datastr(27)
            'F                                  27
            parm_coll(28).Direction = ParameterDirection.Input

            parm_coll(29) = New OracleParameter("shareflag", OracleType.VarChar, 2)
            parm_coll(29).Value = datastr(28)

            parm_coll(29).Direction = ParameterDirection.Input

            parm_coll(30) = New OracleParameter("err_msg", OracleType.VarChar, 300)
            parm_coll(30).Direction = ParameterDirection.Output

            parm_coll(31) = New OracleParameter("err_stat", OracleType.Number, 1)
            parm_coll(31).Direction = ParameterDirection.Output

            parm_coll(32) = New OracleParameter("neftdetails", OracleType.VarChar, 1000)
            parm_coll(32).Direction = ParameterDirection.Input
            parm_coll(32).Value = neftdata

            parm_coll(33) = New OracleParameter("LandHLD", OracleType.Number, 10)
            parm_coll(33).Direction = ParameterDirection.Input
            parm_coll(33).Value = CInt(datastr(30))

            Try
                oh1.ExecuteNonQuery("ADDCUSTOMER_LIMA", parm_coll)
                message = parm_coll(30).Value.ToString & "+" & parm_coll(31).Value.ToString & "+" & parm_coll(16).Value.ToString
            Catch ex As Exception
                message = ex.Message
            End Try
        Catch ex As Exception
            message = ex.Message
        End Try
        Return message
    End Function


    <WebMethod()> _
    Public Function customeradd_new1(ByVal data As String, ByVal firmid As Integer, ByVal branchid As Integer, ByVal user_id As String, ByVal neftdata As String) As String
        Dim datastr() As String = data.Split("¥")
        Dim parm_coll(38) As OracleParameter
        Dim message As String
        Dim oh1 As New Helper.Oracle.OracleHelper
        Try
            parm_coll(0) = New OracleParameter("fmno", OracleType.Number, 5)
            parm_coll(0).Value = CInt(firmid)
            parm_coll(0).Direction = ParameterDirection.Input

            parm_coll(1) = New OracleParameter("brno", OracleType.Number, 5)
            parm_coll(1).Value = CInt(branchid)
            parm_coll(1).Direction = ParameterDirection.Input

            parm_coll(2) = New OracleParameter("cusname", OracleType.VarChar, 40)
            parm_coll(2).Value = datastr(0)
            'ADADA/                             0
            parm_coll(2).Direction = ParameterDirection.Input

            parm_coll(3) = New OracleParameter("tele", OracleType.VarChar, 15)
            parm_coll(3).Value = datastr(12)
            '21323123123                        12
            parm_coll(3).Direction = ParameterDirection.Input

            parm_coll(4) = New OracleParameter("mob", OracleType.VarChar, 15)
            parm_coll(4).Value = datastr(13)
            '13123                              13
            parm_coll(4).Direction = ParameterDirection.Input

            parm_coll(5) = New OracleParameter("fathus", OracleType.VarChar, 40)
            parm_coll(5).Value = datastr(1)
            'ADASD                              1
            parm_coll(5).Direction = ParameterDirection.Input

            parm_coll(6) = New OracleParameter("housenm", OracleType.VarChar, 40)
            parm_coll(6).Value = datastr(2)
            'QWEQEWQW                           2
            parm_coll(6).Direction = ParameterDirection.Input

            parm_coll(7) = New OracleParameter("loca", OracleType.VarChar, 100)
            parm_coll(7).Value = datastr(4)
            'EQWSCAS                            4
            parm_coll(7).Direction = ParameterDirection.Input

            parm_coll(8) = New OracleParameter("pinsrl", OracleType.Number, 7)
            Dim pinsr() As String = datastr(8).Split("@")
            If Not IsNumeric(pinsr(1)) Then
                parm_coll(8).Value = DBNull.Value
            Else
                parm_coll(8).Value = CInt(pinsr(1))
            End If
            '680554@4016                        8
            parm_coll(8).Direction = ParameterDirection.Input

            parm_coll(9) = New OracleParameter("email_id", OracleType.VarChar, 35)
            parm_coll(9).Value = datastr(14)
            'zczcas                             14
            parm_coll(9).Direction = ParameterDirection.Input

            parm_coll(10) = New OracleParameter("occ_id", OracleType.Number, 2)
            If Not IsNumeric(datastr(11)) Then
                parm_coll(10).Value = DBNull.Value
            Else
                parm_coll(10).Value = CInt(datastr(11))
            End If

            '2                                  11
            parm_coll(10).Direction = ParameterDirection.Input

            parm_coll(11) = New OracleParameter("pass_id", OracleType.VarChar, 35)
            parm_coll(11).Value = ""
            parm_coll(11).Direction = ParameterDirection.Input

            parm_coll(12) = New OracleParameter("pan", OracleType.VarChar, 35)
            parm_coll(12).Value = datastr(29)
            parm_coll(12).Direction = ParameterDirection.Input

            Dim st As Date
            st = CDate(datastr(20))
            parm_coll(13) = New OracleParameter("dob", OracleType.DateTime, 11)
            parm_coll(13).Value = CDate(Format(st, "dd-MMM-yyyy"))
            '1                                  20
            parm_coll(13).Direction = ParameterDirection.Input

            parm_coll(14) = New OracleParameter("cust_type", OracleType.Number, 2)
            If Not IsNumeric(datastr(10)) Then
                parm_coll(14).Value = DBNull.Value
            Else
                parm_coll(14).Value = CInt(datastr(10))
            End If

            '8                                  10  
            parm_coll(14).Direction = ParameterDirection.Input

            parm_coll(15) = New OracleParameter("cntid", OracleType.Number, 4)
            If Not IsNumeric(datastr(5)) Then
                parm_coll(15).Value = DBNull.Value
            Else
                parm_coll(15).Value = CInt(datastr(5))
            End If

            '1                                  5
            parm_coll(15).Direction = ParameterDirection.Input

            parm_coll(16) = New OracleParameter("custid", OracleType.VarChar, 200)
            parm_coll(16).Direction = ParameterDirection.Output

            parm_coll(17) = New OracleParameter("id", OracleType.Number, 3)
            If Not IsNumeric(datastr(15)) Then
                parm_coll(17).Value = DBNull.Value
            Else
                parm_coll(17).Value = CInt(datastr(15))
            End If

            '5                                  15
            parm_coll(17).Direction = ParameterDirection.Input

            parm_coll(18) = New OracleParameter("id_no", OracleType.VarChar, 20)
            parm_coll(18).Value = datastr(16)
            'ASDAS12312                         16
            parm_coll(18).Direction = ParameterDirection.Input

            Dim isdt As Date
            isdt = CDate(datastr(17))
            parm_coll(19) = New OracleParameter("is_dt", OracleType.DateTime, 11)
            parm_coll(19).Value = CDate(Format(isdt, "dd-MMM-yyyy"))
            'Friday, January 08, 2010           17
            parm_coll(19).Direction = ParameterDirection.Input

            Dim exdt As Date
            exdt = CDate(datastr(18))
            parm_coll(20) = New OracleParameter("ex_dt", OracleType.DateTime, 11)
            parm_coll(20).Value = CDate(Format(exdt, "dd-MMM-yyyy"))
            'Thursday, December 08, 2011        18
            parm_coll(20).Direction = ParameterDirection.Input

            parm_coll(21) = New OracleParameter("is_plce", OracleType.VarChar, 40)
            parm_coll(21).Value = datastr(19)
            'ASDADAFriday, October 09, 2009     19
            parm_coll(21).Direction = ParameterDirection.Input

            parm_coll(22) = New OracleParameter("gen", OracleType.Number, 2)
            If Not IsNumeric(datastr(21)) Then
                parm_coll(22).Value = DBNull.Value
            Else
                parm_coll(22).Value = CInt(datastr(21))
            End If

            '1                                  21
            parm_coll(22).Direction = ParameterDirection.Input

            parm_coll(23) = New OracleParameter("p_street", OracleType.VarChar, 40)
            parm_coll(23).Value = datastr(3)
            'EQWSCAS                            3
            parm_coll(23).Direction = ParameterDirection.Input

            parm_coll(24) = New OracleParameter("p_media_id", OracleType.Number, 5)
            If Not IsNumeric(datastr(23)) Then
                parm_coll(24).Value = DBNull.Value
            Else
                parm_coll(24).Value = CInt(datastr(23))
            End If

            '1                                  22
            parm_coll(24).Direction = ParameterDirection.Input

            parm_coll(25) = New OracleParameter("p_module_id", OracleType.Number, 8)
            If Not IsNumeric(datastr(24)) Then
                parm_coll(25).Value = DBNull.Value
            Else
                parm_coll(25).Value = CInt(datastr(24))
            End If

            '1                                  24
            parm_coll(25).Direction = ParameterDirection.Input

            parm_coll(26) = New OracleParameter("userid", OracleType.VarChar, 40)
            parm_coll(26).Value = user_id
            parm_coll(26).Direction = ParameterDirection.Input
            '18                                 6
            '390                                7
            '680554                             9
            '1                                  25
            '                                   26
            '                                   28
            parm_coll(27) = New OracleParameter("typeid", OracleType.VarChar, 25)
            If IsNumeric(datastr(22)) Then
                parm_coll(27).Value = datastr(22)
            Else
                parm_coll(27).Value = DBNull.Value
            End If
            '0                                  23
            parm_coll(27).Direction = ParameterDirection.Input

            parm_coll(28) = New OracleParameter("cardNo", OracleType.VarChar, 40)
            parm_coll(28).Value = datastr(27)
            'F                                  27
            parm_coll(28).Direction = ParameterDirection.Input

            parm_coll(29) = New OracleParameter("shareflag", OracleType.VarChar, 2)
            parm_coll(29).Value = datastr(28)

            parm_coll(29).Direction = ParameterDirection.Input

            parm_coll(30) = New OracleParameter("err_msg", OracleType.VarChar, 300)
            parm_coll(30).Direction = ParameterDirection.Output

            parm_coll(31) = New OracleParameter("err_stat", OracleType.Number, 1)
            parm_coll(31).Direction = ParameterDirection.Output

            parm_coll(32) = New OracleParameter("neftdetails", OracleType.VarChar, 1000)
            parm_coll(32).Direction = ParameterDirection.Input
            parm_coll(32).Value = neftdata

            parm_coll(33) = New OracleParameter("LandHLD", OracleType.Number, 10)
            parm_coll(33).Direction = ParameterDirection.Input
            If Not IsNumeric(datastr(30)) Then
                parm_coll(33).Value = DBNull.Value
            Else
                parm_coll(33).Value = CInt(datastr(30))
            End If

            parm_coll(34) = New OracleParameter("EX_STATUS", OracleType.Number, 2)
            parm_coll(34).Direction = ParameterDirection.Input
            If Not IsNumeric(datastr(31)) Then
                parm_coll(34).Value = DBNull.Value
            Else
                parm_coll(34).Value = CInt(datastr(31))
            End If

            parm_coll(35) = New OracleParameter("EX_NO", OracleType.VarChar, 40)
            parm_coll(35).Direction = ParameterDirection.Input
            If Not IsNumeric(datastr(32)) Then
                parm_coll(35).Value = DBNull.Value
            Else
                parm_coll(35).Value = CStr(datastr(32))
            End If

            parm_coll(36) = New OracleParameter("relgn", OracleType.Number, 2)
            If Not IsNumeric(datastr(25)) Then
                parm_coll(36).Value = DBNull.Value
            Else
                parm_coll(36).Value = CInt(datastr(25))
            End If
            parm_coll(36).Direction = ParameterDirection.Input


            parm_coll(37) = New OracleParameter("cst", OracleType.Number, 2)
            If Not IsNumeric(datastr(26)) Then
                parm_coll(37).Value = DBNull.Value
            Else
                parm_coll(37).Value = CInt(datastr(26))
            End If
            parm_coll(37).Direction = ParameterDirection.Input

            parm_coll(38) = New OracleParameter("purofloan", OracleType.Number, 2)
            If Not IsNumeric(datastr(33)) Then
                parm_coll(38).Value = DBNull.Value
            Else
                parm_coll(38).Value = CInt(datastr(33))
            End If
            parm_coll(38).Direction = ParameterDirection.Input

            Try
                oh1.ExecuteNonQuery("ADDCUSTOMER_LIMA_NEW1", parm_coll)
                message = Convert.ToString(parm_coll(30).Value) & "+" & Convert.ToString(parm_coll(31).Value) & "+" & Convert.ToString(parm_coll(16).Value)
            Catch ex As Exception
                message = ex.Message
            End Try
        Catch ex As Exception
            message = ex.Message
        End Try
        Return message
    End Function



    <WebMethod()> _
    Public Function NewcustomeraddPhoto(ByVal custid As String, ByVal cust_photo() As Byte, ByVal add_photo() As Byte, ByVal kyc_photo() As Byte, ByVal neftphoto() As Byte) As String
        Dim sql As String
        If Not IsNothing(cust_photo) Then
            sql = "update customer_photo set pledge_photo=:ph where cust_id='" & custid & "'"
            Dim parm(0) As OracleParameter
            parm(0) = New OracleParameter
            parm(0).ParameterName = "ph"
            parm(0).OracleType = OracleType.Blob
            parm(0).Direction = ParameterDirection.Input
            parm(0).Value = cust_photo
            oh.ExecuteNonQuery(sql, parm)
        End If

        If Not IsNothing(add_photo) Then
            sql = "update customer_photo set kyc_photo=:ph1 where cust_id='" & custid & "'"
            Dim parm1(0) As OracleParameter
            parm1(0) = New OracleParameter
            parm1(0).ParameterName = "ph1"
            parm1(0).OracleType = OracleType.Blob
            parm1(0).Direction = ParameterDirection.Input
            parm1(0).Value = add_photo
            oh.ExecuteNonQuery(sql, parm1)
        End If
        If Not IsNothing(kyc_photo) Then
            sql = "update customer_photo set add_photo=:ph2 where cust_id='" & custid & "'"
            Dim parm2(0) As OracleParameter
            parm2(0) = New OracleParameter
            parm2(0).ParameterName = "ph2"
            parm2(0).OracleType = OracleType.Blob
            parm2(0).Direction = ParameterDirection.Input
            parm2(0).Value = kyc_photo
            oh.ExecuteNonQuery(sql, parm2)
        End If
        If Not IsNothing(neftphoto) Then
            sql = "update neft_customer set id_proof=:ph3 where cust_id='" & custid & "'"
            Dim parm2(0) As OracleParameter
            parm2(0) = New OracleParameter
            parm2(0).ParameterName = "ph3"
            parm2(0).OracleType = OracleType.Blob
            parm2(0).Direction = ParameterDirection.Input
            parm2(0).Value = neftphoto
            oh.ExecuteNonQuery(sql, parm2)
        End If
        'End If
    End Function

    '<WebMethod()> _
    'Public Function searchCustomer(ByVal custid As String, ByVal fmno As Integer, ByVal brno As Integer, ByVal typ As Integer) As DataSet
    '    Dim logdat As String = ""
    '    Dim dt As New DataSet
    '    If typ = 1 Then
    '        dt = oh.ExecuteDataSet("select count(*) from customer where cust_id='" & custid & "' and branch_id=" & brno & "") ' and branch_id=" & brno & " and firm_id=" & fmno & "
    '        If dt.Tables(0).Rows.Count > 0 Then
    '            logdat = "select c.cust_id,c.name,c.fat_hus,c.house_name,c.locality,p.post_office,p.pin_code,c.phone1,c.share_no from customer c,post_master p where c.cust_id='" & custid & "' and c.pin_serial=p.sr_number  and c.branch_id=" & brno & ""
    '            dt = oh.ExecuteDataSet(logdat)
    '        End If
    '    ElseIf typ = 2 Then
    '        logdat = "select  c.cust_id,c.name,c.fat_hus,c.house_name,c.locality,p.post_office,p.pin_code,c.phone1,c.share_no  from customer c,post_master p where c.name like '" & custid.ToUpper & "%' and c.pin_serial=p.sr_number  and c.branch_id=" & brno & ""
    '        dt = oh.ExecuteDataSet(logdat)
    '    Else
    '        dt = oh.ExecuteDataSet("select count(*)  from customer c,post_master p where c.card_no='" & custid & "' and c.pin_serial=p.sr_number ")
    '        If dt.Tables(0).Rows.Count > 0 Then
    '            logdat = "select  c.cust_id,c.name,c.fat_hus,c.house_name,c.locality,p.post_office,p.pin_code,c.phone1,c.share_no  from customer c,post_master p where c.card_no='" & custid & "' and c.pin_serial=p.sr_number and c.branch_id=" & brno & ""
    '            dt = oh.ExecuteDataSet(logdat)
    '        End If
    '        End If
    '        Return dt
    'End Function
    <WebMethod()> _
    Public Function searchCustomer(ByVal custid As String, ByVal fmno As Integer, ByVal brno As Integer, ByVal typ As Integer) As DataSet
        Dim logdat As String = ""
        Dim dt As New DataSet
        If typ = 1 Then
            'dt = oh.ExecuteDataSet("select count(*) from customer where cust_id='" & custid & "' And branch_id = " & brno & " And firm_id = " & fmno & "")
            dt = oh.ExecuteDataSet("select count(*) from customer where cust_id='" & custid & "'")
            If dt.Tables(0).Rows.Count > 0 Then
                logdat = "select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office,  p.pin_code,  c.Phone1,  c.Phone2,  c.share_no  from customer c, post_master p  where c.cust_id = '" & custid & "'  and c.pin_serial = p.sr_number  and nvl(c.isactive, 0) not in (2, 3)  and c.branch_id = " & brno & "  union  select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office,  p.pin_code,  c.Phone1,  c.Phone2,  c.share_no  from customer c, post_master p  where c.cust_id = '" & custid & "'  and c.pin_serial = p.sr_number  and nvl(c.isactive, 0) not in (2, 3)  and c.branch_id <> " & brno & "  and (c.cust_id) in (select m.cust_id  from pledge_master m, pledge_status s  where m.pledge_no = s.pledge_no  and m.cust_id = '" & custid & "'  and m.branch_id = " & brno & "  and s.status_id <> 0)  union  select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office,  p.pin_code,  substr(rtrim(ltrim(c.Phone1)), 1, 1) || '********' ||  substr(rtrim(ltrim(c.Phone1)), -3, 8) as Phone1,  substr(rtrim(ltrim(c.Phone2)), 1, 1) || '********' ||  substr(rtrim(ltrim(c.Phone2)), -3, 8) as Phone2,  c.share_no  from customer c, post_master p  where c.cust_id = '" & custid & "'  and c.pin_serial = p.sr_number  and nvl(c.isactive, 0) not in (2, 3)  and c.branch_id <> " & brno & "  and (c.cust_id) not in  (select m.cust_id  from pledge_master m, pledge_status s, customer t  where t.cust_id = m.cust_id  and nvl(t.isactive, 0) not in (2, 3)  and t.branch_id <> " & brno & "  and m.pledge_no = s.pledge_no  and m.cust_id = '" & custid & "'  and m.branch_id = " & brno & "  and s.status_id  <> 0)"
                dt = oh.ExecuteDataSet(logdat)
            End If
        ElseIf typ = 2 Then
            logdat = "select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office,  p.pin_code,  c.Phone1,  c.Phone2,  c.share_no  from customer c, post_master p  where c.name like '" & custid.ToUpper & "%'  and c.pin_serial = p.sr_number  and nvl(c.isactive, 0) not in (2, 3)  and c.branch_id = " & brno & "  union  select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office,  p.pin_code,  c.Phone1,  c.Phone2,  c.share_no  from customer c, post_master p  where c.name like '" & custid.ToUpper & "%'  and c.pin_serial = p.sr_number  and nvl(c.isactive, 0) not in (2, 3)  and c.branch_id <> " & brno & "  and (c.cust_id) in (select m.cust_id  from pledge_master m, pledge_status s  where m.pledge_no = s.pledge_no  and m.cust_id = c.cust_id  and m.branch_id = " & brno & "  and s.status_id <> 0)"
            dt = oh.ExecuteDataSet(logdat)
        ElseIf typ = 4 Then
            logdat = "select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office,  p.pin_code,  c.Phone1,  c.Phone2,  c.share_no  from customer c, post_master p, customer_detail d  where d.cust_id = c.cust_id  and upper(d.pan) = upper('" & custid & "')  and nvl(c.isactive, 0) not in (2, 3)  and c.pin_serial = p.sr_number  and c.branch_id = " & brno & "  union  select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office,  p.pin_code,  c.Phone1,  c.Phone2,  c.share_no  from customer c, post_master p  where upper(d.pan) = upper('" & custid & "')  and c.pin_serial = p.sr_number  and nvl(c.isactive, 0) not in (2, 3)  and c.branch_id <> " & brno & "  and (c.cust_id) in (select m.cust_id  from pledge_master m, pledge_status s  where m.pledge_no = s.pledge_no  and m.cust_id = c.cust_id  and m.branch_id = " & brno & "  and s.status_id <> 0)   union  select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office,  p.pin_code,  substr(rtrim(ltrim(c.Phone1)), 1, 1) || '********' ||  substr(rtrim(ltrim(c.Phone1)), -3, 8) as Phone1,  substr(rtrim(ltrim(c.Phone2)), 1, 1) || '********' ||  substr(rtrim(ltrim(c.Phone2)), -3, 8) as Phone2,  c.share_no  from customer c, post_master p  where upper(d.pan) = upper('" & custid & "')  and c.pin_serial = p.sr_number  and nvl(c.isactive, 0) not in (2, 3)  and c.branch_id <> " & brno & "  and (c.cust_id) not in  (select m.cust_id  from pledge_master m, pledge_status s, customer t  where t.cust_id = m.cust_id  and nvl(t.isactive, 0) not in (2, 3)  and t.branch_id <> " & brno & "  and m.pledge_no = s.pledge_no  and m.cust_id = c.cust_id  and m.branch_id = " & brno & ")  and s.status_id  <> 0"
            dt = oh.ExecuteDataSet(logdat)
        ElseIf typ = 5 Then
            logdat = "select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office,  p.pin_code,  c.Phone1,  c.Phone2,  c.share_no  from customer c, post_master p, customer_detail d  where d.cust_id = c.cust_id  and (((c.phone1 is not null) and c.phone1 = ('" & custid & "')) or  ((c.phone2 is not null) and c.phone2 = ('" & custid & "')))  and nvl(c.isactive, 0) not in (2, 3)  and c.pin_serial = p.sr_number  and c.branch_id = " & brno & "  union  select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office,  p.pin_code,  c.Phone1,  c.Phone2,  c.share_no  from customer c, post_master p  where (((c.phone1 is not null) and c.phone1 = ('" & custid & "')) or  ((c.phone2 is not null) and c.phone2 = ('" & custid & "')))  and c.pin_serial = p.sr_number  and nvl(c.isactive, 0) not in (2, 3)  and c.branch_id <> " & brno & "  and (c.cust_id) in (select m.cust_id  from pledge_master m, pledge_status s  where m.pledge_no = s.pledge_no  and m.cust_id = c.cust_id  and m.branch_id = " & brno & "  and s.status_id <> 0)  union  select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office,  p.pin_code,  substr(rtrim(ltrim(c.Phone1)), 1, 1) || '********' ||  substr(rtrim(ltrim(c.Phone1)), -3, 8) as Phone1,  substr(rtrim(ltrim(c.Phone2)), 1, 1) || '********' ||  substr(rtrim(ltrim(c.Phone2)), -3, 8) as Phone2,  c.share_no  from customer c, post_master p  where (((c.phone1 is not null) and c.phone1 = ('" & custid & "')) or  ((c.phone2 is not null) and c.phone2 = ('" & custid & "')))  and c.pin_serial = p.sr_number  and nvl(c.isactive, 0) not in (2, 3)  and c.branch_id <> " & brno & "  and (c.cust_id) not in  (select m.cust_id  from pledge_master m, pledge_status s, customer t  where t.cust_id = m.cust_id  and nvl(t.isactive, 0) not in (2, 3)  and t.branch_id <> " & brno & "  and m.pledge_no = s.pledge_no  and m.cust_id = c.cust_id  and m.branch_id = " & brno & "  and s.status_id  <> 0)"
            dt = oh.ExecuteDataSet(logdat)
        ElseIf typ = 6 Then
            logdat = "select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office,  p.pin_code,  c.Phone1,  c.Phone2,  c.share_no  from customer c, post_master p, identity_dtl d  where c.cust_id = d.cust_id  and d.id_number is not null  and upper(d.id_number) = upper('" & custid & "')  and nvl(c.isactive, 0) not in (2, 3)  and c.pin_serial = p.sr_number  and c.branch_id = " & brno & "  union  union  select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office,  p.pin_code,  c.Phone1,  c.Phone2,  c.share_no  from customer c, post_master p, identity_dtl d  where c.cust_id = d.cust_id  and upper(d.id_number) = upper('" & custid & "')  and c.pin_serial = p.sr_number  and nvl(c.isactive, 0) not in (2, 3)  and c.branch_id <> " & brno & "  and (c.cust_id) in (select m.cust_id  from pledge_master m, pledge_status s  where m.pledge_no = s.pledge_no  and m.cust_id = c.cust_id  and m.branch_id = " & brno & "  and s.status_id <> 0)  union  select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office,  p.pin_code,  substr(rtrim(ltrim(c.Phone1)), 1, 1) || '********' ||  substr(rtrim(ltrim(c.Phone1)), -3, 8) as Phone1,  substr(rtrim(ltrim(c.Phone2)), 1, 1) || '********' ||  substr(rtrim(ltrim(c.Phone2)), -3, 8) as Phone2,  c.share_no  from customer c, post_master p, identity_dtl d  where c.cust_id = d.cust_id  and upper(d.id_number) = upper('" & custid & "')  and c.pin_serial = p.sr_number  and nvl(c.isactive, 0) not in (2, 3)  and c.branch_id <> " & brno & "  and (c.cust_id) not in  (select m.cust_id  from pledge_master m, pledge_status s, customer t  where t.cust_id = m.cust_id  and nvl(t.isactive, 0) not in (2, 3)  and t.branch_id <> " & brno & "  and m.pledge_no = s.pledge_no  and m.cust_id = c.cust_id  and m.branch_id = " & brno & "  and s.status_id  <> 0)"
            dt = oh.ExecuteDataSet(logdat)
        Else
            dt = oh.ExecuteDataSet("select count(*)  from customer c, post_master p where c.card_no = '" & custid & "'   and c.pin_serial = p.sr_number   and nvl(c.isactive, 0) not in (2, 3)")
            If dt.Tables(0).Rows.Count > 0 Then
                logdat = "select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office,  p.pin_code,  c.Phone1,  c.share_no  from customer c, post_master p  where c.card_no = '" & custid & "'  and c.pin_serial = p.sr_number  and c.branch_id = " & brno & "  union  select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office,  p.pin_code,  c.Phone1,  c.Phone2,  c.share_no  from customer c, post_master p  where c.card_no = '" & custid & "'  and c.pin_serial = p.sr_number  and nvl(c.isactive, 0) not in (2, 3)  and c.branch_id <> " & brno & "  and (c.cust_id) in (select m.cust_id  from pledge_master m, pledge_status s  where m.pledge_no = s.pledge_no  and m.cust_id = c.cust_id  and m.branch_id = " & brno & "  and s.status_id <> 0)  union  select c.cust_id,  c.name,  c.fat_hus,  c.house_name,  c.locality,  p.post_office,  p.pin_code,  substr(rtrim(ltrim(c.Phone1)), 1, 1) || '********' ||  substr(rtrim(ltrim(c.Phone1)), -3, 8) as Phone1,  substr(rtrim(ltrim(c.Phone2)), 1, 1) || '********' ||  substr(rtrim(ltrim(c.Phone2)), -3, 8) as Phone2,  c.share_no  from customer c, post_master p  where c.card_no = '" & custid & "'  and c.pin_serial = p.sr_number  and nvl(c.isactive, 0) not in (2, 3)  and c.branch_id <> " & brno & "  and (c.cust_id) not in  (select m.cust_id  from pledge_master m, pledge_status s, customer t  where t.cust_id = m.cust_id  and nvl(t.isactive, 0) not in (2, 3)  and t.branch_id <> " & brno & "  and m.pledge_no = s.pledge_no  and m.cust_id = c.cust_id  and m.branch_id = " & brno & "  and s.status_id  <> 0)"
                dt = oh.ExecuteDataSet(logdat)
            End If
        End If
        Return dt
    End Function

    '  <WebMethod()> _
    'Public Function DisplayCustomer(ByVal custid As String, ByVal brno As Integer) As DataSet
    '      Dim logdat As String = ""
    '      Dim dt As New DataSet
    '      logdat = "select c.cust_id,c.name,c.fat_hus,c.house_name,c.locality,p.post_office,p.pin_code,c.phone1,d.district_name,s.state_name,cd.country_name,im.identity_id,im.identity_name,id.id_number,c.share_no from customer c,post_master p,district_master d,state_master s,country_dtl cd,identity im,identity_dtl id where c.cust_id='" & custid & "' and c.pin_serial=p.sr_number and p.district_id=d.district_id and d.state_id=s.state_id and cd.country_id=s.country_id and c.cust_id=id.cust_id(+) and im.identity_id(+)=id.identity_id and c.branch_id=" & brno & ""
    '      dt = oh.ExecuteDataSet(logdat)
    '      Return dt
    '  End Function
    <WebMethod()> _
    Public Function DisplayCustomer(ByVal custid As String, ByVal brno As Integer) As DataSet
        Dim logdat As String = ""
        Dim dt As New DataSet
        logdat = "select c.cust_id,c.name,c.fat_hus,c.house_name,c.locality,p.post_office,p.pin_code,substr(rtrim(ltrim(c.Phone2)), 1, 1) || '********' ||  substr(rtrim(ltrim(c.Phone2)), -3, 8) as Phone2,d.district_name,s.state_name,cd.country_name,im.identity_id,im.identity_name,id.id_number,c.share_no,id.address_proof,c.branch_id,c.firm_id,c.isactive from customer c,post_master p,district_master d,state_master s,country_dtl cd,identity im,identity_dtl id where c.cust_id='" & custid & "' and c.pin_serial=p.sr_number and p.district_id=d.district_id and d.state_id=s.state_id and cd.country_id=s.country_id and c.cust_id=id.cust_id(+) and im.identity_id(+)=id.identity_id"
        dt = oh.ExecuteDataSet(logdat)
        Return dt
    End Function

    <WebMethod()> _
    Public Function AddCustomerPhoto(ByVal custid As String, ByVal cust_photo() As Byte, ByVal stat As Integer) As String
        Dim ods As New DataSet
        Dim dt As New DataTable
        Dim message As String = Nothing
        If stat = 1 Then
            Try
                ods = oh.ExecuteDataSet("select count(*) from customer_photo where cust_id='" & custid & "'")
                If ods.Tables(0).Rows(0)(0) > 0 Then
                    oh.ExecuteNonQuery("insert into dms.customer_photo_his select t.*,sysdate from customer_photo t where cust_id='" & custid & "'")
                    Dim sql1 As String = "update customer_photo set pledge_photo=:ph where cust_id='" & custid & "'"
                    Dim parm1(0) As OracleParameter
                    parm1(0) = New OracleParameter
                    parm1(0).ParameterName = "ph"
                    parm1(0).OracleType = OracleType.Blob
                    parm1(0).Direction = ParameterDirection.Input
                    parm1(0).Value = cust_photo
                    oh.ExecuteNonQuery(sql1, parm1)
                    message = "Complete2"
                Else
                    oh.ExecuteNonQuery("insert into customer_photo (cust_id) values ('" & custid & "')")
                    Dim sql As String = "update customer_photo set pledge_photo=:ph where cust_id='" & custid & "'"
                    Dim parm(0) As OracleParameter
                    parm(0) = New OracleParameter
                    parm(0).ParameterName = "ph"
                    parm(0).OracleType = OracleType.Blob
                    parm(0).Direction = ParameterDirection.Input
                    parm(0).Value = cust_photo
                    oh.ExecuteNonQuery(sql, parm)
                    message = "Complete"
                End If
            Catch ex As Exception
                message = ex.Message
            End Try
        Else
            Try
                ods = oh.ExecuteDataSet("select count(*) from customer_photo where cust_id='" & custid & "'")
                If ods.Tables(0).Rows(0)(0) > 0 Then
                    oh.ExecuteNonQuery("insert into dms.customer_photo_his select t.*,sysdate from customer_photo t where cust_id='" & custid & "'")
                    Dim sql1 As String = "update customer_photo set kyc_photo=:ph where cust_id='" & custid & "'"
                    Dim parm1(0) As OracleParameter
                    parm1(0) = New OracleParameter
                    parm1(0).ParameterName = "ph"
                    parm1(0).OracleType = OracleType.Blob
                    parm1(0).Direction = ParameterDirection.Input
                    parm1(0).Value = cust_photo
                    oh.ExecuteNonQuery(sql1, parm1)
                    message = "Complete2"
                Else
                    oh.ExecuteNonQuery("insert into customer_photo (cust_id) values ('" & custid & "')")
                    Dim sql As String = "update customer_photo set kyc_photo=:ph where cust_id='" & custid & "'"
                    Dim parm(0) As OracleParameter
                    parm(0) = New OracleParameter
                    parm(0).ParameterName = "ph"
                    parm(0).OracleType = OracleType.Blob
                    parm(0).Direction = ParameterDirection.Input
                    parm(0).Value = cust_photo
                    oh.ExecuteNonQuery(sql, parm)
                    message = "Complete"
                End If
                If message = "Complete2" Or message = "Complete" Then
                    Dim parm_coll(0) As OracleParameter

                    parm_coll(0) = New OracleParameter("custid", OracleType.VarChar, 16)
                    parm_coll(0).Value = custid
                    parm_coll(0).Direction = ParameterDirection.Input
                    oh.ExecuteNonQuery("Proc_Kyc_WorkAlert_Rmv", parm_coll)
                End If
            Catch ex As Exception
                message = ex.Message
            End Try
        End If
        Return message
    End Function

    <WebMethod()> _
    Public Function customerKYCDisplaydata(ByVal Adhar_ID As String, ByVal rrn_n As String, ByVal branch_id As String) As DataSet
        Dim odh As New Helper.Oracle.OracleHelper
        Dim ods As New DataSet
        Dim oddt As New DataTable
        Try
            Dim sql As String = "SELECT ELEMENT(T.CUST_DTL, 1, '©') CUS_NAME,  to_char(to_date(ELEMENT(T.CUST_DTL, 2, '©'), 'dd-mm-yyyy'),  'dd-MON-yyyy') CUS_DOB,  DECODE(ELEMENT(T.CUST_DTL, 3, '©'), 'M', 'MALE', 'F', 'FEMALE') CUS_GENDER,           DECODE(ELEMENT(T.CUST_DTL, 8, '©'),  '--',  '',  ELEMENT(T.CUST_DTL, 8, '©')) CUS_HOUSE,  DECODE(ELEMENT(T.CUST_DTL, 10, '©'),  '--',  '',  ELEMENT(T.CUST_DTL, 10, '©')) CUS_STREET,  DECODE(ELEMENT(T.CUST_DTL, 12, '©'),  '--',  '',  ELEMENT(T.CUST_DTL, 12, '©')) CUS_LOCAL,  ELEMENT(T.CUST_DTL, 13, '©') CUST_DIST,  ELEMENT(T.CUST_DTL, 14, '©') CUST_STATE,  ELEMENT(T.CUST_DTL, 4, '©') CUST_MOBILE,  ELEMENT(T.CUST_DTL, 5, '©') CUST_MAIL,  ELEMENT(T.CUST_DTL, 6, '©') CUST_FAT,  ELEMENT(T.CUST_DTL, 15, '©') CUST_PIN  FROM TBL_EKYC_LOG T  WHERE T.VERIFY_ID = '" & rrn_n & "'"
            'Dim sql As String = "SELECT ELEMENT(T.CUST_DTL, 1, '©')CUS_NAME,ELEMENT(T.CUST_DTL, 2, '©') CUS_DOB,DECODE(ELEMENT(T.CUST_DTL, 3, '©'), 'M', 'MALE', 'F', 'FEMALE') CUS_GENDER,ELEMENT(T.CUST_DTL, 7, '©') || ',' ||DECODE(ELEMENT(T.CUST_DTL, 8, '©'),'--','',ELEMENT(T.CUST_DTL, 8, '©')) || ',' ||DECODE(ELEMENT(T.CUST_DTL, 9, '©') ,'--','',ELEMENT(T.CUST_DTL, 9, '©')) || ',' ||DECODE(ELEMENT(T.CUST_DTL, 10, '©'),'--','',ELEMENT(T.CUST_DTL, 10, '©')) || ',' ||DECODE(ELEMENT(T.CUST_DTL, 11, '©'),'--','',ELEMENT(T.CUST_DTL, 11, '©')) || ',' ||DECODE(ELEMENT(T.CUST_DTL, 12, '©'),'--','',ELEMENT(T.CUST_DTL, 12, '©')) CUS_ADD, ELEMENT(T.CUST_DTL, 13, '©') CUST_DIST,ELEMENT(T.CUST_DTL, 14, '©') CUST_STATE FROM TBL_EKYC_LOG T WHERE T.VERIFY_ID IN (SELECT T.VERIFY_ID FROM TBL_EKYC_LOG T WHERE T.CUSTID_STATUS = 0 AND T.STATUS = 'Y' AND T.EKYC_MODE = 'KUA' AND T.BRANCH_ID = 3038 AND (T.AADHAAR_NO, T.VERIFIED_DT) IN(SELECT E.AADHAAR_NO, MAX(E.VERIFIED_DT)FROM TBL_EKYC_LOG E WHERE E.CUSTID_STATUS = 0 AND E.STATUS = 'Y' AND E.EKYC_MODE = 'KUA' AND E.BRANCH_ID = '" + branch_id + "' AND E.AADHAAR_NO = '" + Adhar_ID + "' GROUP BY E.AADHAAR_NO))"
            '    AND E.BRANCH_ID = '" + branch_id + "' AND E.AADHAAR_NO = '" + Adhar_ID + "' GROUP BY E.AADHAAR_NO))"
            oddt = odh.ExecuteDataSet(sql).Tables(0).Copy
            oddt.TableName = "customer_dt1"
            ods.Tables.Add(oddt)
            Dim PINCODE As String = CStr(ods.Tables("customer_dt1").Rows(0)("CUST_PIN"))
            Dim SqlCountry As String = "select distinct s.country_id,s.state_id, s.state_name,d.district_id,d.district_name from state_master s, district_master d, POST_MASTER t where s.state_id = d.state_id and t.district_id = d.district_id and t.pin_code = " + PINCODE + ""
            oddt = odh.ExecuteDataSet(SqlCountry).Tables(0).Copy
            oddt.TableName = "customer_KYCdt1"
            ods.Tables.Add(oddt)
            Dim QueryPostOffice As String = "select PM.pin_code ||'@'||PM.sr_number as pincode ,PM.Post_Office from POST_MASTER PM where PM.PIN_CODE = " + PINCODE + ""
            oddt = odh.ExecuteDataSet(QueryPostOffice).Tables(0).Copy
            oddt.TableName = "customer_POSTKYCdt1"
            ods.Tables.Add(oddt)
        Catch ex As Exception

        End Try
        Return ods

        'Dim country_id, state_id, district_id As String

    End Function

    '<WebMethod()> _
    '    Public Function CheckCoMob(ByVal mob As String) As DataSet
    '    Dim dt As New DataSet
    '    dt = oh.ExecuteDataSet("select count(*) from (select t.device_no mob from TBL_INST_ASSIGN t  where t.devicetype = 2 union all select m.mobile_no mob  from mobile_master m) a where a.mob = '" & mob & "'")
    '    Return dt
    'End Function

    '<WebMethod()> _
    '    Public Function EmpMobCheck(ByVal mob As String) As String
    '    Dim dt As New DataTable
    '    Dim flg As Integer
    '    dt = oh.ExecuteDataSet("select g.mobile_no, g.emp_code  from emp_greeting_master g, emp_master e  where g.emp_code = e.EMP_CODE and e.STATUS_ID = 1 and g.mobile_no = '" & mob & "'").Tables(0)
    '    If dt.Rows.Count > 0 Then
    '        flg = dt.Rows(0)(1).ToString()
    '    Else
    '        flg = "0"
    '    End If
    '    Return flg
    'End Function


    <WebMethod()> _
    Public Function customermodiifyDisplaydata(ByVal customer_id As String, ByVal branch_id As Integer, ByVal firm_id As Integer) As DataSet
        Dim odh As New Helper.Oracle.OracleHelper
        Dim ods As New DataSet
        Dim oddt As New DataTable
        Dim country_id, state_id, district_id, rel_id, caste_id As String
        Try
            oddt = odh.ExecuteDataSet("select * from identity id where identity_id in (1,3,2,4,14,16,5) order by id.identity_name").Tables(0).Copy
            oddt.TableName = "identity"
            ods.Tables.Add(oddt)
            oddt = odh.ExecuteDataSet("select occupation_id,occupation_name from occupation_master om where om.status=1 order by om.occupation_name").Tables(0).Copy
            oddt.TableName = "occupation_master"
            ods.Tables.Add(oddt)
            oddt = odh.ExecuteDataSet("select type_id,descr from customer_type").Tables(0).Copy
            oddt.TableName = "customer_type"
            ods.Tables.Add(oddt)
            oddt = odh.ExecuteDataSet("select country_id,country_name from country_dtl order by country_id ").Tables(0).Copy
            oddt.TableName = "country_dtl"
            country_id = oddt.Rows(0)(0).ToString
            ods.Tables.Add(oddt)
            oddt = odh.ExecuteDataSet("select state_id,state_name from state_master st order by st.state_name").Tables(0).Copy
            oddt.TableName = "state_master"
            state_id = oddt.Rows(0)(0).ToString
            ods.Tables.Add(oddt)
            oddt = odh.ExecuteDataSet("select district_id,district_name from district_master where state_id in (select bm.state_id from branch_master bm where bm.branch_id=" & branch_id & ")  order by district_name").Tables(0).Copy
            oddt.TableName = "district_master"
            district_id = oddt.Rows(0)(0).ToString
            ods.Tables.Add(oddt)
            oddt = odh.ExecuteDataSet("select pin_code ||'@'||sr_number as pincode,post_office from post_master where district_id in (select bm.district_id from branch_master bm where bm.branch_id=" & branch_id & ") order by post_office").Tables(0).Copy
            oddt.TableName = "post_master"
            ods.Tables.Add(oddt)
            oddt = odh.ExecuteDataSet(modigetcusid(customer_id, firm_id, branch_id)).Tables(0).Copy
            oddt.TableName = "customer_dt1"
            ods.Tables.Add(oddt)
            'oddt = odh.ExecuteDataSet("select ct.descr,om.occupation_name,cd.email_id,cd.date_of_birth,cd.pan from customer_detail cd,customer_type ct,occupation_master om where cust_id='" & customer_id & "' and cd.cust_type=ct.type_id and cd.occupation_id=om.occupation_id").Tables(0).Copy
            oddt = odh.ExecuteDataSet("select (select ct.descr from customer_type ct where ct.type_id = cd.cust_type) descr, (select om.occupation_name from occupation_master om where om.occupation_id = cd.occupation_id) occupation_name, cd.email_id, cd.date_of_birth, cd.pan, cd.emp_code, (select emp_name from employee_master where emp_code=cd.emp_code) emp_name,cd.occupation_id,cd.gender,nvl(cd.MARITAL_STAT,0) MARITAL_STAT, cd.MOTHER_NAME, nvl(cd.Citizenship, 0) Citizenship, nvl(cd.nationality, 0) Nation, nvl(Resident, 0) Resident, nvl(FATHUS_PRE, 0) FATHUS_PRE, cd.cust_category, cd.religion, cd.caste, cd.addr_flg, cd.edu_qual, cd.need_for_loan, cd.income, cd.first_gl from customer_detail cd where cust_id = '" & customer_id & "'").Tables(0).Copy
            oddt.TableName = "customer_dt2"
            ods.Tables.Add(oddt)
            rel_id = oddt.Rows(0)("religion").ToString()
            caste_id = oddt.Rows(0)("caste").ToString()
            oddt = odh.ExecuteDataSet("select iid.identity_name,idt.id_number,idt.issue_dt,idt.exp_dt,idt.issue_plce,idt.descr,iid.identity_id,idt.exservice_status,idt.pension_order,nvl(idt.kycof,1) kycof from identity_dtl idt,identity iid where idt.cust_id='" & customer_id & "' and idt.identity_id=iid.identity_id").Tables(0).Copy
            oddt.TableName = "identity_values"
            ods.Tables.Add(oddt)
            oddt = odh.ExecuteDataSet("select t.type_id, t.type_name  from media_type_new t where t.type_id = 10 union all select type_id, type_name   from media_type_new m where m.type_id <> 10").Tables(0).Copy
            oddt.TableName = "media_type"
            ods.Tables.Add(oddt)
            oddt = odh.ExecuteDataSet("select media_id,media_name from media_master_new where type_id= " & ods.Tables("media_type").Rows(0)(0)).Tables(0).Copy
            oddt.TableName = "media_master"
            ods.Tables.Add(oddt)
            oddt = odh.ExecuteDataSet("select mem.media_id,mem.type_id from media_master_new mem,brand_awareness ba where ba.customer_id='" & customer_id & "' and ba.media_id=mem.media_id").Tables(0).Copy
            oddt.TableName = "media_master_list"
            ods.Tables.Add(oddt)
            oddt = odh.ExecuteDataSet("select card_no,issued_dt,issued_by,branch_id from customer_card_dtl where cust_id='" & customer_id & "' order by issued_dt desc").Tables(0).Copy
            oddt.TableName = "customer_card_dtl"
            ods.Tables.Add(oddt)
            oddt = oh.ExecuteDataSet("select t.status_id, t.description from STATUS_MASTER t where t.module_id = 890 and t.option_id = 1").Tables(0).Copy
            oddt.TableName = "lang"
            ods.Tables.Add(oddt)
            oddt = oh.ExecuteDataSet("select t.pref_lang from CUSTOMER t where t.cust_id = '" & customer_id & "'").Tables(0).Copy
            oddt.TableName = "Custlang"
            ods.Tables.Add(oddt)
            '------------------------------------- SEBIN JOSEPH -------------------------------------------------------------------------------------------------
            oddt = oh.ExecuteDataSet("select P.PEP_ID,P.PEP_DESCRIPTION from CUST_PEP P where P.ISACTIVE = 1 order by P.PEP_ID").Tables(0).Copy
            oddt.TableName = "CUST_PEP"
            ods.Tables.Add(oddt)

            oddt = oh.ExecuteDataSet("select t.pep_id from customer_detail t where t.cust_id = '" & customer_id & "'").Tables(0).Copy
            oddt.TableName = "CUST_PEP1"
            ods.Tables.Add(oddt)
            '---------------------------------------------------------------------------------------------------------------------------------------------
            oddt = oh.ExecuteDataSet("select t.status_id, t.description from STATUS_MASTER t where t.module_id = 892 and t.option_id = 1").Tables(0).Copy
            oddt.TableName = "MARITAL"
            ods.Tables.Add(oddt)

            oddt = oh.ExecuteDataSet("select t.status_id, t.description from STATUS_MASTER t where t.module_id = 893 and t.option_id = 1").Tables(0).Copy
            oddt.TableName = "CITIZEN"
            ods.Tables.Add(oddt)

            oddt = oh.ExecuteDataSet("select t.status_id, t.description from STATUS_MASTER t where t.module_id = 894 and t.option_id = 1").Tables(0).Copy
            oddt.TableName = "NATION"
            ods.Tables.Add(oddt)

            oddt = oh.ExecuteDataSet("select t.status_id, t.description from STATUS_MASTER t where t.module_id = 895 and t.option_id = 1").Tables(0).Copy
            oddt.TableName = "RESIDENT"
            ods.Tables.Add(oddt)

            oddt = oh.ExecuteDataSet("select prename_id||'~'||gender prename_id,prename from prename_master order by prename_id").Tables(0).Copy
            oddt.TableName = "prename_master"
            ods.Tables.Add(oddt)

            oddt = oh.ExecuteDataSet("select nvl(t.name_pre, '-1') || '~' || nvl(d.gender, 0)  from CUSTOMER t, customer_detail d where t.cust_id = d.cust_id   and t.cust_id = '" & customer_id & "'").Tables(0).Copy
            oddt.TableName = "prename_val"
            ods.Tables.Add(oddt)

            oddt = oh.ExecuteDataSet("select religion_id,religion from religion_master  order by religion_id").Tables(0).Copy
            oddt.TableName = "religion_master"
            ods.Tables.Add(oddt)

            oddt = oh.ExecuteDataSet("select t.status_id, t.description from STATUS_MASTER t where t.module_id = 896 and t.option_id = 1").Tables(0).Copy
            oddt.TableName = "EDUCATION"
            ods.Tables.Add(oddt)

            oddt = oh.ExecuteDataSet("select t.status_id, t.description from STATUS_MASTER t where t.module_id = 896 and t.option_id = 2").Tables(0).Copy
            oddt.TableName = "INCOME"
            ods.Tables.Add(oddt)

            oddt = oh.ExecuteDataSet("select t.status_id, t.description from STATUS_MASTER t where t.module_id = 896 and t.option_id = 3").Tables(0).Copy
            oddt.TableName = "NEEDFORLOAN"
            ods.Tables.Add(oddt)

            oddt = oh.ExecuteDataSet("select t.status_id, t.description from STATUS_MASTER t where t.module_id = 896 and t.option_id = 4").Tables(0).Copy
            oddt.TableName = "FIRSTLOAN"
            ods.Tables.Add(oddt)

            If rel_id <> "" Then
                oddt = oh.ExecuteDataSet("select caste_id,caste_name from caste_master  where religion_id=" & rel_id & " order by caste_id").Tables(0).Copy
                oddt.TableName = "caste_master"
                ods.Tables.Add(oddt)
            End If

        Catch ex As Exception

        End Try
        Return ods
    End Function

    Function modigetcusid(ByVal cust_id As String, ByVal firm_id As Int32, ByVal branch_id As Int64) As String
        Try
            Dim sql As String = Nothing
            Dim odh As New Helper.Oracle.OracleHelper
            Dim ods As New DataSet
            ods = odh.ExecuteDataSet("select PIN_SERIAL from customer where cust_id='" & cust_id & "' ")
            Dim odt As New DataTable
            odt = ods.Tables(0)
            If odt.Rows(0)(0) > 0 Then
                sql = "select distinct customer.cust_id,customer.name cust_name,customer.house_name,customer.LOCALITY,customer.phone1,customer.phone2,post_master.post_office,post_master.pin_code ||'@'||post_master.sr_number as pincode,district_master.district_id,customer.fat_hus,sm.state_id state,cd.country_name country,customer.locality from  customer customer,post_master,district_master,state_master sm,country_dtl cd where post_master.sr_number=customer.pin_serial and district_master.district_id=post_master.district_id and customer.cust_id='" & cust_id & "' and sm.state_id=district_master.state_id  and cd.country_id=sm.country_id"
            Else
                sql = "select distinct customer.cust_id,customer.name cust_name,customer.house_name,customer.LOCALITY,customer.phone1,customer.phone2,post_master.post_office,post_master.pin_code ||'@'||post_master.sr_number as pincode,district_master.district_id,customer.fat_hus,sm.state_id state,cd.country_name country,customer.locality from  customer customer,post_master,district_master,state_master sm,country_dtl cd where post_master.sr_number=customer.pin_serial and district_master.district_id=post_master.district_id and customer.cust_id='" & cust_id & "' and sm.state_id=district_master.state_id  and cd.country_id=sm.country_id"
            End If
            Return sql
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <WebMethod()> _
    Public Function cust_modi(ByVal input_value() As String, ByVal input_dates() As Date, ByVal type As Integer, ByVal modi_key As String) As String
        'Public Function cust_modi() As String
        '
        '

        Dim oh As New Helper.Oracle.OracleHelper
        Dim oh1 As New Helper.Oracle.OracleHelper
        Dim parm_coll(21) As OracleParameter

        'Dim update_value() As String
        'Dim input_value(19) As String
        'Dim input_dates(6) As Date
        'Dim type As Integer = 2
        'Dim modi_key As String = "0!0!0!0!0!0!1!1!1!0!0!1!1!0!1!0!1!1!1!1!1!1!"
        'update_value = modi_key.Split("!")


        ' custid in varchar2,
        ' fathus in varchar2,
        ' housenm in varchar2,
        ' loca in varchar2,
        ' pinsrl in number,
        ' custtype in number,
        ' occ_id in number,
        ' tele in varchar2,
        ' mob in varchar2,
        ' emailid in varchar2,
        ' id_type in number,
        ' id_no in varchar2,
        ' date_of_issue in date,
        ' date_of_expiry in date,
        ' place_of_issue in varchar2, 
        ' dob in date,
        ' descr_modi in varchar2,
        ' out_result out varchar2,
        ' update_modi in varchar2
        ' street in varchar2,



        'input_value(0) = "01002600008929"
        'input_value(1) = "OR     GIRISHKUMAR"
        'input_value(2) = "THONIPARAMBIL HOUSE"
        'input_value(3) = "VALAPAD"
        'input_value(4) = "680554@4016"
        'input_value(5) = "1"
        'input_value(6) = "2"
        'input_value(7) = "111111111111"
        'input_value(8) = "99999999999"
        'input_value(9) = "456464564566"
        'input_value(10) = "1"
        'input_value(11) = "eqwweqweqeqwe"
        'input_value(12) = "qwqweqweqwe"
        'input_value(13) = ""
        'input_value(14) = "NATTAKAM Street one"
        'input_value(15) = "12"
        'input_value(16) = "5"
        'input_dates(0) = #3/25/2003#
        'input_dates(1) = #3/25/2010#
        'input_dates(2) = #3/6/1980#
        'input_dates(3) = #12:00:00 AM#
        'input_dates(4) = #12:00:00 AM#
        'input_dates(5) = #12:00:00 AM#

        parm_coll(0) = New OracleParameter("custid", OracleType.VarChar, 16)
        parm_coll(0).Value = input_value(0)
        parm_coll(0).Direction = ParameterDirection.Input

        ' fathus in varchar2,
        parm_coll(1) = New OracleParameter("fathus", OracleType.VarChar, 100)
        parm_coll(1).Value = input_value(1)
        parm_coll(1).Direction = ParameterDirection.Input

        ' housenm in varchar2,
        parm_coll(2) = New OracleParameter("housenm", OracleType.VarChar, 100)
        parm_coll(2).Value = input_value(2)
        parm_coll(2).Direction = ParameterDirection.Input

        ' loca in varchar2,
        parm_coll(3) = New OracleParameter("loca", OracleType.VarChar, 40)
        parm_coll(3).Value = input_value(3)
        parm_coll(3).Direction = ParameterDirection.Input


        ' pinsrl in number,
        parm_coll(4) = New OracleParameter("pinsrl", OracleType.Number, 15)
        Dim pinsr() As String = input_value(4).Split("@")
        parm_coll(4).Value = CLng(pinsr(1))
        parm_coll(4).Direction = ParameterDirection.Input

        ' custtype in number,
        parm_coll(5) = New OracleParameter("custtype", OracleType.Number, 15)
        parm_coll(5).Value = CInt(input_value(5))
        parm_coll(5).Direction = ParameterDirection.Input

        ' occ_id in number,
        parm_coll(6) = New OracleParameter("occ_id", OracleType.Number, 5)
        parm_coll(6).Value = CInt(input_value(6))
        parm_coll(6).Direction = ParameterDirection.Input

        ' tele in varchar2,
        parm_coll(7) = New OracleParameter("tele", OracleType.VarChar, 40)
        parm_coll(7).Value = input_value(7)
        parm_coll(7).Direction = ParameterDirection.Input

        ' mob in varchar2,
        parm_coll(8) = New OracleParameter("mob", OracleType.VarChar, 40)
        parm_coll(8).Value = input_value(8)
        parm_coll(8).Direction = ParameterDirection.Input

        ' emailid in varchar2,
        parm_coll(9) = New OracleParameter("emailid", OracleType.VarChar, 40)
        If input_value(9) = "" Then
            parm_coll(9).Value = "NILL"
        Else
            parm_coll(9).Value = input_value(9)
        End If

        parm_coll(9).Direction = ParameterDirection.Input

        ' id_type in number,
        parm_coll(10) = New OracleParameter("id_type", OracleType.VarChar, 60)
        parm_coll(10).Value = CInt(input_value(10))
        parm_coll(10).Direction = ParameterDirection.Input

        'id_no in varchar2,
        parm_coll(11) = New OracleParameter("id_no", OracleType.VarChar, 80)
        parm_coll(11).Value = input_value(11)
        parm_coll(11).Direction = ParameterDirection.Input

        ' place_of_issue in varchar2, 
        parm_coll(14) = New OracleParameter("place_of_issue", OracleType.VarChar, 40)
        parm_coll(14).Value = input_value(12)
        parm_coll(14).Direction = ParameterDirection.Input

        ' descr_modi in varchar2,
        parm_coll(16) = New OracleParameter("descr_modi", OracleType.VarChar, 60)
        parm_coll(16).Value = input_value(13)
        parm_coll(16).Direction = ParameterDirection.Input

        ' street in varchar2,
        parm_coll(19) = New OracleParameter("p_street", OracleType.VarChar, 40)
        parm_coll(19).Value = input_value(14)
        parm_coll(19).Direction = ParameterDirection.Input


        ' p_media_id in Number,
        parm_coll(20) = New OracleParameter("p_media_id", OracleType.Number, 8)
        parm_coll(20).Value = CInt(input_value(15))
        parm_coll(20).Direction = ParameterDirection.Input

        ' p_module_id in Number,
        parm_coll(21) = New OracleParameter("p_module_id", OracleType.Number, 8)
        parm_coll(21).Value = CInt(input_value(16))
        parm_coll(21).Direction = ParameterDirection.Input

        ' date_of_issue in date,
        parm_coll(12) = New OracleParameter("date_of_issue", OracleType.DateTime, 22)
        parm_coll(12).Value = CDate(Format(input_dates(0), "dd-MMM-yyyy"))
        parm_coll(12).Direction = ParameterDirection.Input

        ' date_of_expiry in date,
        parm_coll(13) = New OracleParameter("date_of_expiry", OracleType.DateTime, 22)
        parm_coll(13).Value = CDate(Format(input_dates(1), "dd-MMM-yyyy"))
        parm_coll(13).Direction = ParameterDirection.Input


        ' dob in date,
        Dim st As Date
        st = CDate(input_dates(2))
        parm_coll(15) = New OracleParameter("dob", OracleType.DateTime, 11)
        parm_coll(15).Value = CDate(Format(st, "dd-MMM-yyyy"))
        parm_coll(15).Direction = ParameterDirection.Input


        ' out_result out varchar2
        parm_coll(17) = New OracleParameter("out_result", OracleType.VarChar, 100)
        parm_coll(17).Direction = ParameterDirection.Output

        parm_coll(18) = New OracleParameter("update_modi", OracleType.VarChar, 60)
        parm_coll(18).Value = modi_key
        parm_coll(18).Direction = ParameterDirection.Input

        Try
            If type = 1 Then
                oh1.ExecuteNonQuery("other_customer_modi", parm_coll)
                If parm_coll(17).Value = "0" Then
                    Return ("0")
                Else
                    Return parm_coll(17).Value
                End If
            ElseIf type = 2 Then
                oh1.ExecuteNonQuery("customer_modi", parm_coll)
                If parm_coll(17).Value = "0" Then
                    Return (0)
                Else
                    Return parm_coll(17).Value
                End If
            Else
                Return "You must Enter the type"
            End If
        Catch ex As Exception
            Return ex.Message.ToString
        End Try
    End Function


    <WebMethod()> _
    Public Function cust_modi_new1(ByVal input_value() As String, ByVal input_dates() As Date, ByVal type As Integer, ByVal modi_key As String) As String
        'Public Function cust_modi() As String
        '
        '

        Dim oh As New Helper.Oracle.OracleHelper
        Dim oh1 As New Helper.Oracle.OracleHelper
        Dim parm_coll(23) As OracleParameter

        'Dim update_value() As String
        'Dim input_value(19) As String
        'Dim input_dates(6) As Date
        'Dim type As Integer = 2
        'Dim modi_key As String = "0!0!0!0!0!0!1!1!1!0!0!1!1!0!1!0!1!1!1!1!1!1!"
        'update_value = modi_key.Split("!")


        ' custid in varchar2,
        ' fathus in varchar2,
        ' housenm in varchar2,
        ' loca in varchar2,
        ' pinsrl in number,
        ' custtype in number,
        ' occ_id in number,
        ' tele in varchar2,
        ' mob in varchar2,
        ' emailid in varchar2,
        ' id_type in number,
        ' id_no in varchar2,
        ' date_of_issue in date,
        ' date_of_expiry in date,
        ' place_of_issue in varchar2, 
        ' dob in date,
        ' descr_modi in varchar2,
        ' out_result out varchar2,
        ' update_modi in varchar2
        ' street in varchar2,



        'input_value(0) = "01002600008929"
        'input_value(1) = "OR     GIRISHKUMAR"
        'input_value(2) = "THONIPARAMBIL HOUSE"
        'input_value(3) = "VALAPAD"
        'input_value(4) = "680554@4016"
        'input_value(5) = "1"
        'input_value(6) = "2"
        'input_value(7) = "111111111111"
        'input_value(8) = "99999999999"
        'input_value(9) = "456464564566"
        'input_value(10) = "1"
        'input_value(11) = "eqwweqweqeqwe"
        'input_value(12) = "qwqweqweqwe"
        'input_value(13) = ""
        'input_value(14) = "NATTAKAM Street one"
        'input_value(15) = "12"
        'input_value(16) = "5"
        'input_dates(0) = #3/25/2003#
        'input_dates(1) = #3/25/2010#
        'input_dates(2) = #3/6/1980#
        'input_dates(3) = #12:00:00 AM#
        'input_dates(4) = #12:00:00 AM#
        'input_dates(5) = #12:00:00 AM#

        parm_coll(0) = New OracleParameter("custid", OracleType.VarChar, 16)
        parm_coll(0).Value = input_value(0)
        parm_coll(0).Direction = ParameterDirection.Input

        ' fathus in varchar2,
        parm_coll(1) = New OracleParameter("fathus", OracleType.VarChar, 100)
        parm_coll(1).Value = input_value(1)
        parm_coll(1).Direction = ParameterDirection.Input

        ' housenm in varchar2,
        parm_coll(2) = New OracleParameter("housenm", OracleType.VarChar, 100)
        parm_coll(2).Value = input_value(2)
        parm_coll(2).Direction = ParameterDirection.Input

        ' loca in varchar2,
        parm_coll(3) = New OracleParameter("loca", OracleType.VarChar, 40)
        parm_coll(3).Value = input_value(3)
        parm_coll(3).Direction = ParameterDirection.Input


        ' pinsrl in number,
        parm_coll(4) = New OracleParameter("pinsrl", OracleType.Number, 15)
        Dim pinsr() As String = input_value(4).Split("@")
        parm_coll(4).Value = CLng(pinsr(1))
        parm_coll(4).Direction = ParameterDirection.Input

        ' custtype in number,
        parm_coll(5) = New OracleParameter("custtype", OracleType.Number, 15)
        parm_coll(5).Value = CInt(input_value(5))
        parm_coll(5).Direction = ParameterDirection.Input

        ' occ_id in number,
        parm_coll(6) = New OracleParameter("occ_id", OracleType.Number, 5)
        parm_coll(6).Value = CInt(input_value(6))
        parm_coll(6).Direction = ParameterDirection.Input

        ' tele in varchar2,
        parm_coll(7) = New OracleParameter("tele", OracleType.VarChar, 40)
        parm_coll(7).Value = input_value(7)
        parm_coll(7).Direction = ParameterDirection.Input

        ' mob in varchar2,
        parm_coll(8) = New OracleParameter("mob", OracleType.VarChar, 40)
        parm_coll(8).Value = input_value(8)
        parm_coll(8).Direction = ParameterDirection.Input

        ' emailid in varchar2,
        parm_coll(9) = New OracleParameter("emailid", OracleType.VarChar, 40)
        If input_value(9) = "" Then
            parm_coll(9).Value = "NILL"
        Else
            parm_coll(9).Value = input_value(9)
        End If

        parm_coll(9).Direction = ParameterDirection.Input

        ' id_type in number,
        parm_coll(10) = New OracleParameter("id_type", OracleType.VarChar, 60)
        parm_coll(10).Value = CInt(input_value(10))
        parm_coll(10).Direction = ParameterDirection.Input

        'id_no in varchar2,
        parm_coll(11) = New OracleParameter("id_no", OracleType.VarChar, 80)
        parm_coll(11).Value = input_value(11)
        parm_coll(11).Direction = ParameterDirection.Input

        ' place_of_issue in varchar2, 
        parm_coll(14) = New OracleParameter("place_of_issue", OracleType.VarChar, 40)
        parm_coll(14).Value = input_value(12)
        parm_coll(14).Direction = ParameterDirection.Input

        ' descr_modi in varchar2,
        parm_coll(16) = New OracleParameter("descr_modi", OracleType.VarChar, 60)
        parm_coll(16).Value = input_value(13)
        parm_coll(16).Direction = ParameterDirection.Input

        ' street in varchar2,
        parm_coll(19) = New OracleParameter("p_street", OracleType.VarChar, 40)
        parm_coll(19).Value = input_value(14)
        parm_coll(19).Direction = ParameterDirection.Input


        ' p_media_id in Number,
        parm_coll(20) = New OracleParameter("p_media_id", OracleType.Number, 8)
        parm_coll(20).Value = CInt(input_value(15))
        parm_coll(20).Direction = ParameterDirection.Input

        ' p_module_id in Number,
        parm_coll(21) = New OracleParameter("p_module_id", OracleType.Number, 8)
        parm_coll(21).Value = CInt(input_value(16))
        parm_coll(21).Direction = ParameterDirection.Input

        ' date_of_issue in date,
        parm_coll(12) = New OracleParameter("date_of_issue", OracleType.DateTime, 22)
        parm_coll(12).Value = CDate(Format(input_dates(0), "dd-MMM-yyyy"))
        parm_coll(12).Direction = ParameterDirection.Input

        ' date_of_expiry in date,
        parm_coll(13) = New OracleParameter("date_of_expiry", OracleType.DateTime, 22)
        parm_coll(13).Value = CDate(Format(input_dates(1), "dd-MMM-yyyy"))
        parm_coll(13).Direction = ParameterDirection.Input


        ' dob in date,
        Dim st As Date
        st = CDate(input_dates(2))
        parm_coll(15) = New OracleParameter("dob", OracleType.DateTime, 11)
        parm_coll(15).Value = CDate(Format(st, "dd-MMM-yyyy"))
        parm_coll(15).Direction = ParameterDirection.Input


        ' out_result out varchar2
        parm_coll(17) = New OracleParameter("out_result", OracleType.VarChar, 100)
        parm_coll(17).Direction = ParameterDirection.Output

        parm_coll(18) = New OracleParameter("update_modi", OracleType.VarChar, 60)
        parm_coll(18).Value = modi_key
        parm_coll(18).Direction = ParameterDirection.Input

        parm_coll(22) = New OracleParameter("ex_status", OracleType.Number, 2)
        parm_coll(22).Value = CInt(input_value(17))
        parm_coll(22).Direction = ParameterDirection.Input

        parm_coll(23) = New OracleParameter("ex_no", OracleType.VarChar, 40)
        parm_coll(23).Value = CStr(input_value(18))
        parm_coll(23).Direction = ParameterDirection.Input

        Try
            If type = 1 Then
                oh1.ExecuteNonQuery("other_customer_modi_new1", parm_coll)
                If parm_coll(17).Value = "0" Then
                    Return ("0")
                Else
                    Return parm_coll(17).Value
                End If
            ElseIf type = 2 Then
                oh1.ExecuteNonQuery("customer_modi_new1", parm_coll)
                If parm_coll(17).Value = "0" Then
                    Return (0)
                Else
                    Return parm_coll(17).Value
                End If
            Else
                Return "You must Enter the type"
            End If
        Catch ex As Exception
            Return ex.Message.ToString
        End Try
    End Function

    <WebMethod()> _
    Public Function cust_modi_new2(ByVal input_value() As String, ByVal input_dates() As Date, ByVal type As Integer, ByVal modi_key As String) As String
        'Public Function cust_modi() As String
        '
        '

        Dim oh As New Helper.Oracle.OracleHelper
        Dim oh1 As New Helper.Oracle.OracleHelper
        Dim parm_coll(26) As OracleParameter

        'Dim update_value() As String
        'Dim input_value(19) As String
        'Dim input_dates(6) As Date
        'Dim type As Integer = 2
        'Dim modi_key As String = "0!0!0!0!0!0!1!1!1!0!0!1!1!0!1!0!1!1!1!1!1!1!"
        'update_value = modi_key.Split("!")


        ' custid in varchar2,
        ' fathus in varchar2,
        ' housenm in varchar2,
        ' loca in varchar2,
        ' pinsrl in number,
        ' custtype in number,
        ' occ_id in number,
        ' tele in varchar2,
        ' mob in varchar2,
        ' emailid in varchar2,
        ' id_type in number,
        ' id_no in varchar2,
        ' date_of_issue in date,
        ' date_of_expiry in date,
        ' place_of_issue in varchar2, 
        ' dob in date,
        ' descr_modi in varchar2,
        ' out_result out varchar2,
        ' update_modi in varchar2
        ' street in varchar2,



        'input_value(0) = "01002600008929"
        'input_value(1) = "OR     GIRISHKUMAR"
        'input_value(2) = "THONIPARAMBIL HOUSE"
        'input_value(3) = "VALAPAD"
        'input_value(4) = "680554@4016"
        'input_value(5) = "1"
        'input_value(6) = "2"
        'input_value(7) = "111111111111"
        'input_value(8) = "99999999999"
        'input_value(9) = "456464564566"
        'input_value(10) = "1"
        'input_value(11) = "eqwweqweqeqwe"
        'input_value(12) = "qwqweqweqwe"
        'input_value(13) = ""
        'input_value(14) = "NATTAKAM Street one"
        'input_value(15) = "12"
        'input_value(16) = "5"
        'input_dates(0) = #3/25/2003#
        'input_dates(1) = #3/25/2010#
        'input_dates(2) = #3/6/1980#
        'input_dates(3) = #12:00:00 AM#
        'input_dates(4) = #12:00:00 AM#
        'input_dates(5) = #12:00:00 AM#

        parm_coll(0) = New OracleParameter("custid", OracleType.VarChar, 16)
        parm_coll(0).Value = input_value(0)
        parm_coll(0).Direction = ParameterDirection.Input

        ' fathus in varchar2,
        parm_coll(1) = New OracleParameter("fathus", OracleType.VarChar, 100)
        parm_coll(1).Value = input_value(1)
        parm_coll(1).Direction = ParameterDirection.Input

        ' housenm in varchar2,
        parm_coll(2) = New OracleParameter("housenm", OracleType.VarChar, 100)
        parm_coll(2).Value = input_value(2)
        parm_coll(2).Direction = ParameterDirection.Input

        ' loca in varchar2,
        parm_coll(3) = New OracleParameter("loca", OracleType.VarChar, 40)
        parm_coll(3).Value = input_value(3)
        parm_coll(3).Direction = ParameterDirection.Input


        ' pinsrl in number,
        parm_coll(4) = New OracleParameter("pinsrl", OracleType.Number, 15)
        Dim pinsr() As String = input_value(4).Split("@")
        If IsNumeric(pinsr(1)) Then
            parm_coll(4).Value = CLng(pinsr(1))
        Else
            parm_coll(4).Value = DBNull.Value
        End If

        parm_coll(4).Direction = ParameterDirection.Input

        ' custtype in number,
        parm_coll(5) = New OracleParameter("custtype", OracleType.Number, 15)
        If IsNumeric(input_value(5)) Then
            parm_coll(5).Value = CInt(input_value(5))
        Else
            parm_coll(5).Value = DBNull.Value
        End If
        parm_coll(5).Direction = ParameterDirection.Input

        ' occ_id in number,
        parm_coll(6) = New OracleParameter("occ_id", OracleType.Number, 5)
        If IsNumeric(input_value(6)) Then
            parm_coll(6).Value = CInt(input_value(6))
        Else
            parm_coll(6).Value = DBNull.Value
        End If

        parm_coll(6).Direction = ParameterDirection.Input

        ' tele in varchar2,
        parm_coll(7) = New OracleParameter("tele", OracleType.VarChar, 40)
        parm_coll(7).Value = input_value(7)
        parm_coll(7).Direction = ParameterDirection.Input

        ' mob in varchar2,
        parm_coll(8) = New OracleParameter("mob", OracleType.VarChar, 40)
        parm_coll(8).Value = input_value(8)
        parm_coll(8).Direction = ParameterDirection.Input

        ' emailid in varchar2,
        parm_coll(9) = New OracleParameter("emailid", OracleType.VarChar, 40)
        If input_value(9) = "" Then
            parm_coll(9).Value = "NILL"
        Else
            parm_coll(9).Value = input_value(9)
        End If

        parm_coll(9).Direction = ParameterDirection.Input

        ' id_type in number,
        parm_coll(10) = New OracleParameter("id_type", OracleType.VarChar, 60)
        If IsNumeric(input_value(10)) Then
            parm_coll(10).Value = CInt(input_value(10))
        Else
            parm_coll(10).Value = DBNull.Value
        End If
        parm_coll(10).Direction = ParameterDirection.Input

        'id_no in varchar2,
        parm_coll(11) = New OracleParameter("id_no", OracleType.VarChar, 80)
        parm_coll(11).Value = input_value(11)
        parm_coll(11).Direction = ParameterDirection.Input

        ' place_of_issue in varchar2, 
        parm_coll(14) = New OracleParameter("place_of_issue", OracleType.VarChar, 40)
        parm_coll(14).Value = input_value(12)
        parm_coll(14).Direction = ParameterDirection.Input

        ' descr_modi in varchar2,
        parm_coll(16) = New OracleParameter("descr_modi", OracleType.VarChar, 60)
        parm_coll(16).Value = input_value(13)
        parm_coll(16).Direction = ParameterDirection.Input

        ' street in varchar2,
        parm_coll(19) = New OracleParameter("p_street", OracleType.VarChar, 40)
        parm_coll(19).Value = input_value(14)
        parm_coll(19).Direction = ParameterDirection.Input


        ' p_media_id in Number,
        parm_coll(20) = New OracleParameter("p_media_id", OracleType.Number, 8)
        If IsNumeric(input_value(15)) Then
            parm_coll(20).Value = CInt(input_value(15))
        Else
            parm_coll(20).Value = DBNull.Value
        End If
        parm_coll(20).Direction = ParameterDirection.Input

        ' p_module_id in Number,
        parm_coll(21) = New OracleParameter("p_module_id", OracleType.Number, 8)
        If IsNumeric(input_value(16)) Then
            parm_coll(21).Value = CInt(input_value(16))
        Else
            parm_coll(21).Value = DBNull.Value
        End If

        parm_coll(21).Direction = ParameterDirection.Input

        ' date_of_issue in date,
        parm_coll(12) = New OracleParameter("date_of_issue", OracleType.DateTime, 22)
        parm_coll(12).Value = CDate(Format(input_dates(0), "dd-MMM-yyyy"))
        parm_coll(12).Direction = ParameterDirection.Input

        ' date_of_expiry in date,
        parm_coll(13) = New OracleParameter("date_of_expiry", OracleType.DateTime, 22)
        parm_coll(13).Value = CDate(Format(input_dates(1), "dd-MMM-yyyy"))
        parm_coll(13).Direction = ParameterDirection.Input


        ' dob in date,
        Dim st As Date
        st = CDate(input_dates(2))
        parm_coll(15) = New OracleParameter("dob", OracleType.DateTime, 11)
        parm_coll(15).Value = CDate(Format(st, "dd-MMM-yyyy"))
        parm_coll(15).Direction = ParameterDirection.Input


        ' out_result out varchar2
        parm_coll(17) = New OracleParameter("out_result", OracleType.VarChar, 100)
        parm_coll(17).Direction = ParameterDirection.Output

        parm_coll(18) = New OracleParameter("update_modi", OracleType.VarChar, 60)
        parm_coll(18).Value = modi_key
        parm_coll(18).Direction = ParameterDirection.Input

        parm_coll(22) = New OracleParameter("ex_status", OracleType.Number, 2)
        If IsNumeric(input_value(17)) Then
            parm_coll(22).Value = CInt(input_value(17))
        Else
            parm_coll(22).Value = DBNull.Value
        End If

        parm_coll(22).Direction = ParameterDirection.Input

        parm_coll(23) = New OracleParameter("ex_no", OracleType.VarChar, 40)
        parm_coll(23).Value = CStr(input_value(18))
        parm_coll(23).Direction = ParameterDirection.Input

        parm_coll(24) = New OracleParameter("Empcode", OracleType.Number, 7)
        If IsNumeric(input_value(19)) Then
            parm_coll(24).Value = CInt(input_value(19))
        Else
            parm_coll(24).Value = DBNull.Value
        End If
        parm_coll(24).Direction = ParameterDirection.Input

        parm_coll(25) = New OracleParameter("Empname", OracleType.VarChar, 40)
        parm_coll(25).Value = CStr(input_value(20))
        parm_coll(25).Direction = ParameterDirection.Input

        parm_coll(26) = New OracleParameter("typeid", OracleType.VarChar)
        If IsNumeric(input_value(21)) Then
            parm_coll(26).Value = CInt(input_value(21))
        Else
            parm_coll(26).Value = DBNull.Value
        End If
        parm_coll(26).Direction = ParameterDirection.Input



        Try
            If type = 1 Then
                oh1.ExecuteNonQuery("other_customer_modi_new2", parm_coll)
                If parm_coll(17).Value = "0" Then
                    Return ("0")
                Else
                    Return parm_coll(17).Value
                End If
            ElseIf type = 2 Then
                oh1.ExecuteNonQuery("customer_modi_new2", parm_coll)
                If parm_coll(17).Value = "0" Then
                    Return (0)
                Else
                    Return parm_coll(17).Value
                End If
            Else
                Return "You must Enter the type"
            End If
        Catch ex As Exception
            Return ex.Message.ToString
        End Try

    End Function

    <WebMethod()> _
    Public Function AddStock(ByVal firmId As Integer, ByVal branchId As Integer, ByVal rcptFrom As Integer, ByVal rcptTo As Integer, ByVal qty As Integer, ByVal category As Integer, ByVal userId As Integer) As String
        Dim oh1 As New Helper.Oracle.OracleHelper
        Dim result As String
        Dim parm_coll(8) As OracleParameter
        Try
            parm_coll(0) = New OracleParameter("fmno", OracleType.Number, 5)
            parm_coll(0).Value = firmId
            parm_coll(0).Direction = ParameterDirection.Input

            parm_coll(1) = New OracleParameter("brno", OracleType.Number, 10)
            parm_coll(1).Value = branchId
            parm_coll(1).Direction = ParameterDirection.Input

            parm_coll(2) = New OracleParameter("rcpt_from", OracleType.Number, 16)
            parm_coll(2).Value = rcptFrom
            parm_coll(2).Direction = ParameterDirection.Input

            parm_coll(3) = New OracleParameter("rcpt_to", OracleType.Number, 16)
            parm_coll(3).Value = rcptTo
            parm_coll(3).Direction = ParameterDirection.Input

            parm_coll(4) = New OracleParameter("quantity", OracleType.Number, 5)
            parm_coll(4).Value = qty
            parm_coll(4).Direction = ParameterDirection.Input

            parm_coll(5) = New OracleParameter("catgry", OracleType.Number, 2)
            parm_coll(5).Value = category
            parm_coll(5).Direction = ParameterDirection.Input

            parm_coll(6) = New OracleParameter("useid", OracleType.VarChar, 25)
            parm_coll(6).Value = userId
            parm_coll(6).Direction = ParameterDirection.Input

            parm_coll(7) = New OracleParameter("errStat", OracleType.Number, 10)
            parm_coll(7).Direction = ParameterDirection.Output

            parm_coll(8) = New OracleParameter("errMsg", OracleType.VarChar, 100)
            parm_coll(8).Direction = ParameterDirection.Output

            oh1.ExecuteNonQuery("Customerstock_insertion", parm_coll)
            result = (parm_coll(7).Value).ToString() + "-" + (parm_coll(8).Value).ToString()
        Catch ex As Exception
            result = ex.Message
        End Try
        Return result
    End Function

    <WebMethod()> _
    Public Function RequestFromBranch(ByVal firmId As Integer, ByVal branchId As Integer, ByVal qty As Integer, ByVal category As Integer, ByVal userId As Integer) As String
        Dim oh1 As New Helper.Oracle.OracleHelper
        Dim result As String
        Dim parm_coll(6) As OracleParameter
        Try
            parm_coll(0) = New OracleParameter("fmno", OracleType.Number, 5)
            parm_coll(0).Value = firmId
            parm_coll(0).Direction = ParameterDirection.Input

            parm_coll(1) = New OracleParameter("brno", OracleType.Number, 10)
            parm_coll(1).Value = branchId
            parm_coll(1).Direction = ParameterDirection.Input

            parm_coll(2) = New OracleParameter("quanty", OracleType.Number, 20)
            parm_coll(2).Value = qty
            parm_coll(2).Direction = ParameterDirection.Input

            parm_coll(3) = New OracleParameter("catgry", OracleType.Number, 2)
            parm_coll(3).Value = category
            parm_coll(3).Direction = ParameterDirection.Input

            parm_coll(4) = New OracleParameter("useid", OracleType.VarChar, 25)
            parm_coll(4).Value = userId
            parm_coll(4).Direction = ParameterDirection.Input

            parm_coll(5) = New OracleParameter("errStat", OracleType.Number, 10)
            parm_coll(5).Direction = ParameterDirection.Output

            parm_coll(6) = New OracleParameter("errMsg", OracleType.VarChar, 100)
            parm_coll(6).Direction = ParameterDirection.Output

            oh1.ExecuteNonQuery("customerRequest", parm_coll)
            result = (parm_coll(5).Value).ToString() + "-" + (parm_coll(6).Value).ToString()
        Catch ex As Exception
            result = ex.Message
        End Try
        Return result
    End Function

    <WebMethod()> _
    Public Function seriesChecking(ByVal firmId As Integer, ByVal branchId As Integer, ByVal catgory As Integer, ByVal barcode As String) As Integer
        Dim sql As String
        Dim message As String

        sql = "select count(*) from customer_card where category=" & catgory & " and branch_id=" & branchId & " and status=1 and barcode='NULL'|| lpad(" & barcode & ",6,'0')"
        Dim dt As New DataTable
        dt = oh.ExecuteDataSet(sql).Tables(0)
        If dt.Rows(0)(0) > 0 Then
            sql = "select substr(barcode,5,14) from customer_card where category=" & catgory & " and branch_id=" & branchId & " and status=1 and barcode='NULL'|| lpad(" & barcode & ",6,'0')"
            dt = oh.ExecuteDataSet(sql).Tables(0)
            If CInt(dt.Rows(0)(0)) = barcode Then
                message = 1 '"Valid"
            Else
                message = 0 ' "Not Valid"
            End If
        Else
            message = 0 ' "Not Valid"
        End If
        Return message
    End Function

    <WebMethod()> _
    Public Function normal_vouch(ByVal trans_id As Integer, ByVal fmno As Integer, ByVal brno As Integer) As DataSet
        Dim oh As New Helper.Oracle.OracleHelper
        Dim dt As New DataSet
        ' dt = oh.ExecuteDataSet("select substr(c.firm_name,11,length(c.firm_name)) firm_name,substr(d.branch_name,0,20) branch_name,a.transno as trans_id,a.account_no,substr(b.account_name,0,25) account,substr(a.descr,0,20) descr,decode(type,'D',a.amount) debit,decode(type,'C',a.amount) credit,narration,e.emp_code || '(' ||substr(e.emp_name,0,10)|| ')' as userid from account_transaction a,account_profile b,firm_master c,branch_master d,employee_master e where a.firm_id=c.firm_id and a.branch_id=d.branch_id and b.ho_status<>2 and a.account_no=b.account_no and e.emp_code=substr(a.user_id,0,5) and a.transno=" & trans_id & " and a.branch_id= " & brno & " and a.firm_id= " & fmno)
        dt = oh.ExecuteDataSet("select substr(c.firm_name,11,length(c.firm_name)) firm_name,substr(d.branch_name,0,20) branch_name,a.trans_id as trans_id,a.account_no,substr(b.account_name,0,25) account,substr(a.descr,0,28) descr,decode(type,'D',a.amount) debit,decode(type,'C',a.amount) credit,narration,substr(e.emp_name,0,10)|| '(' ||e.emp_code || ')' as userid from transaction_detail a,account_profile b,firm_master c,branch_master d,employee_master e,account_status f where a.firm_id=c.firm_id and a.branch_id=d.branch_id and b.ho_status<>2 and a.account_no=b.account_no and e.emp_code=substr(a.user_id,0,5) and a.trans_id=" & trans_id & " and a.branch_id= " & brno & " and a.firm_id= " & fmno & " and a.firm_id=f.firm_id and a.branch_id=f.branch_id and a.account_no=f.account_no and f.status_id not in(2,3)union all select substr(c.firm_name,11,length(c.firm_name)) firm_name,substr(d.branch_name,0,20) branch_name,a.trans_id as trans_id,a.parent_acc,substr(b.account_name,0,25) account,a.account_no || ' ' ||substr(a.descr,0,20) descr,decode(a.type,'D',a.amount) debit,decode(a.type,'C',a.amount) credit,' ' as narration,' ' as userid from subsidary_transaction a,subsidary_master b,firm_master c,branch_master d,account_status f where a.branch_id=b.branch_id and a.firm_id=b.firm_id and a.parent_acc=b.parent_acc and a.account_no=b.account_no and a.firm_id=c.firm_id and a.branch_id=d.branch_id and a.trans_id=" & trans_id & " and a.branch_id= " & brno & " and a.firm_id= " & fmno & " and a.firm_id=f.firm_id and a.branch_id=f.branch_id and a.parent_acc=f.account_no and f.status_id in(2,3) order by userid,narration")
        Return dt
    End Function

    <WebMethod()> _
    Public Function vouch_tst(ByVal trans_id As Integer, ByVal fmno As Integer, ByVal brno As Integer) As String
        Dim oh As New Helper.Oracle.OracleHelper
        Dim s As String
        Dim op(4) As OracleParameter
        op(0) = New OracleParameter("fmno", OracleType.Number, 3)
        op(0).Value = fmno
        op(0).Direction = ParameterDirection.Input
        op(1) = New OracleParameter("brno", OracleType.Number, 5)
        op(1).Value = brno
        op(1).Direction = ParameterDirection.Input
        op(2) = New OracleParameter("trans", OracleType.Number, 8)
        op(2).Value = trans_id
        op(2).Direction = ParameterDirection.Input
        op(3) = New OracleParameter("nar", OracleType.VarChar, 300)
        op(3).Direction = ParameterDirection.Output
        op(4) = New OracleParameter("cshid", OracleType.VarChar, 200)
        op(4).Direction = ParameterDirection.Output
        oh.ExecuteNonQuery("vouch_tst", op)
        s = op(3).Value & "*" & op(4).Value
        Return s
    End Function

    <WebMethod()> _
    Public Function getDate() As Date
        Dim oh As New Helper.Oracle.OracleHelper
        Dim dt As New DataTable
        Dim brdt As Date
        dt = oh.ExecuteDataSet("select to_date(sysdate) from dual").Tables(0)
        brdt = dt.Rows(0)(0)
        Return brdt
    End Function

    <WebMethod()> _
    Public Function dep_printer_vch(ByVal brid As Integer) As String
        Dim dt, dt1 As New DataTable
        Dim sql As String = "select parmtr_value from deposit_printer where parmtr_id=9 and firm_id=7 and module_id=5 and catgry=2 and branch_id=" & brid
        dt = oh.ExecuteDataSet(sql).Tables(0)
        Dim sql1 As String = "select parmtr_value from deposit_printer where parmtr_id=10 and firm_id=7 and module_id=5 and catgry=2 and branch_id=" & brid
        dt1 = oh.ExecuteDataSet(sql1).Tables(0)
        Return dt1.Rows(0)(0) & dt.Rows(0)(0)
    End Function

    <WebMethod()> _
    Public Function executeQuery(ByVal sql As String) As String
        Dim result As String
        Dim oh As New Helper.Oracle.OracleHelper
        result = oh.ExecuteNonQuery(sql)
        Return result
    End Function

    <WebMethod()> _
    Public Function cash_vouch(ByVal trans_id As Integer, ByVal fmno As Integer, ByVal brno As Integer) As DataSet
        Dim sql As String = "select cust_name customer,cash_id cashid,amount amount from cash_transaction where trans_id=" & trans_id & " and branch_id= " & brno & " and firm_id= " & fmno
        Dim dt As New DataSet
        Dim oh As New Helper.Oracle.OracleHelper
        dt = oh.ExecuteDataSet(sql)
        Return dt
    End Function

    <WebMethod()> _
    Public Function AddShare(ByVal firmId As Integer, ByVal branchId As Integer, ByVal customerId As String, ByVal custName As String, ByVal userId As String) As String
        Dim oh1 As New Helper.Oracle.OracleHelper
        Dim result As String
        Dim parm_coll(7) As OracleParameter
        Try
            parm_coll(0) = New OracleParameter("fmno", OracleType.Number, 5)
            parm_coll(0).Value = firmId
            parm_coll(0).Direction = ParameterDirection.Input

            parm_coll(1) = New OracleParameter("brno", OracleType.Number, 10)
            parm_coll(1).Value = branchId
            parm_coll(1).Direction = ParameterDirection.Input

            parm_coll(2) = New OracleParameter("custid", OracleType.VarChar, 14)
            parm_coll(2).Value = customerId
            parm_coll(2).Direction = ParameterDirection.Input

            parm_coll(3) = New OracleParameter("custnm", OracleType.VarChar, 40)
            parm_coll(3).Value = custName
            parm_coll(3).Direction = ParameterDirection.Input

            parm_coll(4) = New OracleParameter("userid", OracleType.VarChar, 30)
            parm_coll(4).Value = userId
            parm_coll(4).Direction = ParameterDirection.Input

            parm_coll(5) = New OracleParameter("transidno", OracleType.Number, 8)
            parm_coll(5).Direction = ParameterDirection.Output

            parm_coll(6) = New OracleParameter("errStat", OracleType.Number, 10)
            parm_coll(6).Direction = ParameterDirection.Output

            parm_coll(7) = New OracleParameter("errMsg", OracleType.VarChar, 100)
            parm_coll(7).Direction = ParameterDirection.Output

            oh1.ExecuteNonQuery("CustomerShareEntry", parm_coll)
            result = Convert.ToString(parm_coll(7).Value) + "+" + Convert.ToString(parm_coll(6).Value) + "+" + Convert.ToString(parm_coll(5).Value)
        Catch ex As Exception
            result = ex.Message
        End Try
        Return result
    End Function

    <WebMethod()> _
    Public Function neftupdation(ByVal userid As String) As String
        Dim pr(2) As OracleParameter
        pr(0) = New OracleParameter("usrid", OracleType.VarChar, 6)
        pr(0).Value = userid
        pr(1) = New OracleParameter("grp_id", OracleType.Int16, 16)
        pr(1).Direction = ParameterDirection.Output
        pr(2) = New OracleParameter("err_stat", OracleType.Number, 2)
        pr(2).Direction = ParameterDirection.Output
        oh.ExecuteNonQuery("neftupdation", pr)
        Dim a As String
        a = pr(1).Value & "*" & pr(2).Value
        Return a
    End Function
    <WebMethod()> _
    Public Function neftdtl(ByVal grpid As Integer) As DataSet
        Dim ds As DataSet
        ds = oh.ExecuteDataSet("select '' trn_head_A,c.beneficiary_branch as trn_body_B,a.amount,to_date(a.send_date) as valuedt,'UTIB0000046' as senderifsc,e.parmtr_value as sendcustacc,d.firm_name as sendcustnm ,c.ifsc_code,c.beneficiary_account as sb_account,ltrim(c.cust_name,102) as cust_name,'CURRENT ACCOUNT' as sen_acc_type ,f.account_name as rec_acc_type,'' as codeword,'' as add_information,'' as more_infn,'' as mur_mur,b.fat_hus||','||b.house_name||','||b.locality as address from neft_master a,customer b,neft_customer c,firm_master d,general_parameter e,neft_current_account f where a.cust_id=c.cust_id and a.cust_id=c.cust_id and a.branch_id=b.branch_id and a.cust_id=b.cust_id and a.firm_id=d.firm_id and e.parmtr_id=56 and e.module_id=1 and e.firm_id=c.firm_id and f.acc_type=c.acc_type and c.verify_status='T' and a.send_transid is not null and a.status_id=0 and a.group_id=" & grpid & " union all select '' trn_head_A,c.beneficiary_branch as trn_body_B,a.amount,to_date(a.send_date) as valuedt,'UTIB0000046' as senderifsc,e.parmtr_value as sendcustacc,d.firm_name as sendcustnm ,c.ifsc_code,c.beneficiary_account as sb_account,ltrim(c.cust_name,102) as cust_name,'CURRENT ACCOUNT' as sen_acc_type ,f.account_name as rec_acc_type,'' as codeword,'' as add_information,'' as more_infn,'' as mur_mur,b.house||','||b.locality as address from neft_master a,tbl_rent_customer b,neft_customer c,firm_master d,general_parameter e,neft_current_account f where a.cust_id=c.cust_id and a.cust_id=c.cust_id and a.cust_id=b.rent_id and a.firm_id=d.firm_id and e.parmtr_id=56 and e.module_id=1 and  e.firm_id=c.firm_id and f.acc_type=c.acc_type and a.firm_id=b.firm_id and c.verify_status='T' and a.send_transid is not null and a.status_id=0 and a.group_id=" & grpid & "")
        Return ds
    End Function

    <WebMethod()> _
    Public Function brstate(ByVal brid As Integer) As DataSet
        Dim st As DataSet
        st = oh.ExecuteDataSet("select distinct state_name,a.state_id from state_master a where a.state_id=" & brid & " UNION select distinct state_name,a.state_id from state_master a where a.state_id<>" & brid & "")
        Return st
    End Function

    <WebMethod()> _
    Public Function brdistrict(ByVal sid As Integer, ByVal brid As Integer) As DataSet
        Dim st As DataSet
        st = oh.ExecuteDataSet("select distinct district_name,a. district_id from district_master a,branch_master b where a.state_id=" & sid & " and a.district_id=b.district_id and b.branch_id=" & brid & " UNION select distinct district_name,a. district_id from district_master a where a.state_id=" & sid & " ")
        Return st
    End Function
    <WebMethod()> _
    Public Function bank(ByVal dist_id As Integer) As DataSet
        Dim ds As DataSet
        ds = oh.ExecuteDataSet("select ifsc_code,abbr||','||branch as bank from neft_bank_mst where dist_id=" & dist_id & " order by bankname||','||branch")
        Return ds
    End Function

    <WebMethod()> _
    Public Function fillcurrentaccount() As DataSet
        Dim ds As DataSet
        ds = oh.ExecuteDataSet("select acc_type,account_name from neft_current_account order by acc_type")
        Return ds
    End Function

    <WebMethod()> _
    Public Function neft_sbacc(ByVal accno As String, ByVal custid As String) As DataSet
        Dim dt As New DataSet
        dt = oh.ExecuteDataSet("select count(*) count from neft_customer where  cust_id='" & custid & "'")
        Return dt
    End Function

    '<WebMethod()> _
    '       Public Function neft_add(ByVal confdata As String, ByVal cust() As Byte) As String
    '    Dim neft_data() As String = confdata.Split("*")
    '    'rec_acc_typ*brid*custid*          ifsc       *accno*custname*fmid**branch_name
    '    '0          *26  *01002600107268*ANDB0001221 *234123*JISHA*     1 **
    '    Dim pr(9) As OracleParameter
    '    pr(0) = New OracleParameter("brid", OracleType.Number, 4)
    '    pr(0).Value = CInt(neft_data(1))
    '    pr(1) = New OracleParameter("fmid", OracleType.Number, 4)
    '    pr(1).Value = CInt(neft_data(6))
    '    pr(2) = New OracleParameter("custid", OracleType.VarChar, 14)
    '    pr(2).Value = neft_data(2)
    '    pr(3) = New OracleParameter("ifsc", OracleType.VarChar, 12)
    '    pr(3).Value = neft_data(3)
    '    pr(4) = New OracleParameter("accno", OracleType.VarChar, 14)
    '    pr(4).Value = neft_data(4)
    '    pr(5) = New OracleParameter("rec_acc_typ", OracleType.VarChar, 14)
    '    pr(5).Value = neft_data(0)
    '    pr(6) = New OracleParameter("custname", OracleType.VarChar, 35)
    '    pr(6).Value = neft_data(5)
    '    pr(7) = New OracleParameter("branch_name", OracleType.VarChar, 35)
    '    pr(7).Value = neft_data(8)
    '    pr(8) = New OracleParameter("er_no", OracleType.Number, 1)
    '    pr(8).Direction = ParameterDirection.InputOutput
    '    pr(9) = New OracleParameter("mobile_no", OracleType.Number, 35)
    '    pr(9).Value = 0

    '    oh.ExecuteNonQuery("neft_add", pr)
    '    Dim sql1 As String = "update neft_customer set id_proof=:ph where cust_id='" & neft_data(2) & "'"
    '    Dim parm1(0) As OracleParameter
    '    parm1(0) = New OracleParameter
    '    parm1(0).ParameterName = "ph"
    '    parm1(0).OracleType = OracleType.Blob
    '    parm1(0).Direction = ParameterDirection.Input
    '    parm1(0).Value = cust
    '    oh.ExecuteNonQuery(sql1, parm1)
    '    Return pr(8).Value
    'End Function

    <WebMethod()> _
    Public Function neft_add(ByVal confdata As String, ByVal cust() As Byte) As String
        Dim neft_data() As String = confdata.Split("*")
        '0  1    2               3              4             5     67  8 
        '10*28*02002800101897*CNRB0001097*1097101002278*DHARMAN K K*1**PERINJANAM
        Dim pr(9) As OracleParameter
        pr(0) = New OracleParameter("brid", OracleType.Number, 4)
        pr(0).Value = CInt(neft_data(1))
        pr(1) = New OracleParameter("fmid", OracleType.Number, 4)
        pr(1).Value = CInt(neft_data(6))
        pr(2) = New OracleParameter("custid", OracleType.VarChar, 14)
        pr(2).Value = neft_data(2)
        pr(3) = New OracleParameter("ifsc", OracleType.VarChar, 12)
        pr(3).Value = neft_data(3)
        pr(4) = New OracleParameter("accno", OracleType.VarChar, 18)
        pr(4).Value = neft_data(4)
        pr(5) = New OracleParameter("rec_acc_typ", OracleType.VarChar, 14)
        pr(5).Value = neft_data(0)
        pr(6) = New OracleParameter("custname", OracleType.VarChar, 35)
        pr(6).Value = neft_data(9)
        pr(7) = New OracleParameter("branch_name", OracleType.VarChar, 35)
        pr(7).Value = neft_data(8)
        pr(8) = New OracleParameter("mobile_no", OracleType.Number, 13)
        pr(8).Value = 0
        pr(9) = New OracleParameter("er_no", OracleType.Number, 1)
        pr(9).Direction = ParameterDirection.InputOutput

        oh.ExecuteNonQuery("neft_add", pr)

        If pr(9).Value = 0 Then
            Dim sql1 As String = "update neft_customer set id_proof=:ph where cust_id='" & neft_data(2) & "'"
            Dim parm1(0) As OracleParameter
            parm1(0) = New OracleParameter
            parm1(0).ParameterName = "ph"
            parm1(0).OracleType = OracleType.Blob
            parm1(0).Direction = ParameterDirection.Input
            parm1(0).Value = cust
            oh.ExecuteNonQuery(sql1, parm1)
        End If
        Return pr(9).Value
    End Function

    <WebMethod()> _
    Public Function checkEmployee(ByVal empcd As Integer) As String
        Dim sql, result As String
        sql = "select nvl(count(*),0) from employee_master where emp_code=" & empcd & " and status_id=1"
        Dim dt As New DataTable
        dt = oh.ExecuteDataSet(sql).Tables(0)
        If dt.Rows(0)(0) > 0 Then
            If empcd > 10000 Then
                sql = "select emp_name from employee_master where emp_code=" & empcd & " and status_id=1"
                dt = oh.ExecuteDataSet(sql).Tables(0)
                result = "0" & "*" & dt.Rows(0)(0)
            Else
                result = "1*1"
            End If
        Else
            result = "1*1"
        End If
        Return result
    End Function
    <WebMethod()> _
    Public Function check_DSA_BA_User(ByVal empcd As Integer, ByVal MediaId As Integer) As String

        Dim dt As New DataTable
        If (MediaId = 14) Then
            sqlDSA = "select nvl(count(*),0) from TBL_ADD_BUSINESSAGENT t where t.ba_code=" & empcd & " and t.status = 1"
        ElseIf (MediaId = 29) Then
            sqlDSA = "select nvl(count(*),0) from TBL_DSA_MASTER  t where t.dsa_code=" & empcd & "  and t.status=1"
        End If

        'sql = "select nvl(count(*),0) from employee_master where emp_code=" & empcd & " and status_id=1"

        dt = oh.ExecuteDataSet(sqlDSA).Tables(0)
        If dt.Rows(0)(0) > 0 Then
            If empcd > 0 Then
                If (MediaId = 14) Then
                    sqlDSA = "select t.brid,BM.BRANCH_NAME, t.ba_code, t.ba_name, t.address, t.phone, '' as email from TBL_ADD_BUSINESSAGENT t inner join Branch_master BM on T.BRID=Bm.Branch_Id where t.ba_code =" & empcd & " and t.status = 1 "
                ElseIf (MediaId = 29) Then
                    sqlDSA = "select t.brid,BM.BRANCH_NAME,t.dsa_code,t.dsa_name,t.address,t.mobile,t.email from TBL_DSA_MASTER t inner join Branch_master BM on T.BRID=Bm.Branch_Id Where t.dsa_code = " & empcd & " and t.status=1"
                End If
                'sql = "select emp_name from employee_master where emp_code=" & empcd & " and status_id=1"
                dt = oh.ExecuteDataSet(sqlDSA).Tables(0)
                DSABAresult = dt.Rows(0)(0) & "*" & dt.Rows(0)(1) & "*" & dt.Rows(0)(2) & "*" & dt.Rows(0)(3) & "*" & dt.Rows(0)(4) & "*" & dt.Rows(0)(5) & "*" & dt.Rows(0)(6)
            Else
                DSABAresult = "0*0"
            End If
        Else
            DSABAresult = "0*0"
        End If
        Return DSABAresult
    End Function

    <WebMethod()> _
    Public Function NEFTDATA() As DataSet
        Dim ds As DataSet
        ds = oh.ExecuteDataSet("select '' trn_head_A,c.beneficiary_branch as trn_body_B,a.amount,to_date(a.send_date) as valuedt,'UTIB0000046' as senderifsc,e.parmtr_value as sendcustacc,d.firm_name as sendcustnm ,c.ifsc_code,c.beneficiary_account as sb_account,ltrim(c.cust_name,102) as cust_name,'CURRENT ACCOUNT' as sen_acc_type ,f.account_name as rec_acc_type,'' as codeword,'' as add_information,'' as more_infn,'' as mur_mur,b.fat_hus||','||b.house_name||','||b.locality as address from neft_master a,customer b,neft_customer c,firm_master d,general_parameter e,neft_current_account f where a.cust_id=c.cust_id and a.cust_id=c.cust_id and a.branch_id=b.branch_id and a.cust_id=b.cust_id and a.firm_id=d.firm_id and e.parmtr_id=56 and e.module_id=1 and e.firm_id=c.firm_id and f.acc_type=c.acc_type and c.verify_status='T' and a.send_date is null and a.status_id=1 union all select '' trn_head_A,c.beneficiary_branch as trn_body_B,a.amount,to_date(a.send_date) as valuedt,'UTIB0000046' as senderifsc,e.parmtr_value as sendcustacc,d.firm_name as sendcustnm ,c.ifsc_code,c.beneficiary_account as sb_account,ltrim(c.cust_name,102) as cust_name,'CURRENT ACCOUNT' as sen_acc_type ,f.account_name as rec_acc_type,'' as codeword,'' as add_information,'' as more_infn,'' as mur_mur,b.house||','||b.locality as address from neft_master a,tbl_rent_customer b,neft_customer c,firm_master d,general_parameter e,neft_current_account f where a.cust_id=c.cust_id and a.cust_id=c.cust_id and a.cust_id=b.rent_id and a.firm_id=d.firm_id and e.parmtr_id=56 and  e.module_id=1 and e.firm_id=c.firm_id and f.acc_type=c.acc_type and a.firm_id=b.firm_id and c.verify_status='T' and a.send_date is null and a.status_id=1")
        Return ds
    End Function
    '<WebMethod()> _
    '   Public Function neft_modi(ByVal confdata As String, ByVal cust() As Byte) As String
    '    Dim neft_data() As String = confdata.Split("*")
    '    Dim pr(10) As OracleParameter
    '    pr(0) = New OracleParameter("brid", OracleType.Number, 4)
    '    pr(0).Value = CInt(neft_data(1))
    '    pr(1) = New OracleParameter("fmid", OracleType.Number, 4)
    '    pr(1).Value = CInt(neft_data(6))
    '    pr(2) = New OracleParameter("custid", OracleType.VarChar, 14)
    '    pr(2).Value = neft_data(2)
    '    pr(3) = New OracleParameter("ifsc", OracleType.VarChar, 12)
    '    pr(3).Value = neft_data(3)
    '    pr(4) = New OracleParameter("accno", OracleType.VarChar, 16)
    '    pr(4).Value = neft_data(4)
    '    pr(5) = New OracleParameter("rec_acc_typ", OracleType.VarChar, 14)
    '    pr(5).Value = neft_data(0)
    '    pr(6) = New OracleParameter("custname", OracleType.VarChar, 40)
    '    pr(6).Value = neft_data(5)
    '    pr(7) = New OracleParameter("branch_name", OracleType.VarChar, 35)
    '    pr(7).Value = neft_data(8)
    '    pr(8) = New OracleParameter("mobile_no", OracleType.Number, 13)
    '    pr(8).Value = neft_data(9)
    '    pr(9) = New OracleParameter("usid", OracleType.Number, 6)
    '    pr(9).Value = neft_data(10)
    '    pr(10) = New OracleParameter("er_no", OracleType.Number, 1)
    '    pr(10).Direction = ParameterDirection.InputOutput

    '    oh.ExecuteNonQuery("neft_modi", pr)
    '    Dim sql1 As String = "update neft_customer set id_proof=:ph where cust_id='" & neft_data(2) & "'"
    '    Dim parm1(0) As OracleParameter
    '    parm1(0) = New OracleParameter
    '    parm1(0).ParameterName = "ph"
    '    parm1(0).OracleType = OracleType.Blob
    '    parm1(0).Direction = ParameterDirection.Input
    '    parm1(0).Value = cust
    '    oh.ExecuteNonQuery(sql1, parm1)
    '    Return pr(9).Value
    'End Function
    <WebMethod()> _
    Public Function neft_modi(ByVal confdata As String) As String
        Dim neft_data() As String = confdata.Split("*")
        Dim pr(10) As OracleParameter
        pr(0) = New OracleParameter("brid", OracleType.Number, 4)
        pr(0).Value = CInt(neft_data(1))
        pr(1) = New OracleParameter("fmid", OracleType.Number, 4)
        pr(1).Value = CInt(neft_data(6))
        pr(2) = New OracleParameter("custid", OracleType.VarChar, 14)
        pr(2).Value = neft_data(2)
        pr(3) = New OracleParameter("ifsc", OracleType.VarChar, 12)
        pr(3).Value = neft_data(3)
        pr(4) = New OracleParameter("accno", OracleType.VarChar, 25)
        pr(4).Value = neft_data(4)
        pr(5) = New OracleParameter("rec_acc_typ", OracleType.VarChar, 14)
        pr(5).Value = neft_data(0)
        pr(6) = New OracleParameter("custname", OracleType.VarChar, 40)
        pr(6).Value = neft_data(5)
        pr(7) = New OracleParameter("branch_name", OracleType.VarChar, 35)
        pr(7).Value = neft_data(8)
        pr(8) = New OracleParameter("mobile_no", OracleType.Number, 13)
        pr(8).Value = neft_data(9)
        pr(9) = New OracleParameter("usid", OracleType.Number, 6)
        pr(9).Value = neft_data(10)
        pr(10) = New OracleParameter("er_no", OracleType.Number, 1)
        pr(10).Direction = ParameterDirection.InputOutput

        oh.ExecuteNonQuery("neft_modi", pr)
        'Dim sql1 As String = "update neft_customer set id_proof=:ph where cust_id='" & neft_data(2) & "'"
        'Dim parm1(0) As OracleParameter
        'parm1(0) = New OracleParameter
        'parm1(0).ParameterName = "ph"
        'parm1(0).OracleType = OracleType.Blob
        'parm1(0).Direction = ParameterDirection.Input
        'parm1(0).Value = cust
        'oh.ExecuteNonQuery(sql1, parm1)
        Return pr(10).Value
    End Function
    <WebMethod()> _
    Public Function deposit_neft(ByVal custid As String) As DataSet
        Dim neft As DataSet
        neft = oh.ExecuteDataSet("select distinct(b.cust_id)||'*'||b.cust_name||'*'||b.ifsc_code||'*'||b.beneficiary_account||'*'||b.beneficiary_branch||'*'||b.verify_status||'*'||c.bankname||'*'||d.account_name||'*'||b.mobile_number as neft,b.cust_id as ds from neft_customer b,neft_bank_mst c,neft_current_account d where b.ifsc_code=c.ifsc_code and b.acc_type=d.acc_type and b.cust_id='" & custid & "'")
        Return neft
    End Function
    <WebMethod()> _
    Public Function modibrdistrict(ByVal sid As Integer, ByVal distid As Integer) As DataSet
        Dim st As DataSet
        st = oh.ExecuteDataSet("select distinct district_name,a. district_id from district_master a,neft_bank_mst b where a.state_id=" & sid & " and a.district_id=b.dist_id and b.dist_id=" & distid & " UNION all select distinct district_name,a. district_id from district_master a,branch_master b where a.state_id=" & sid & " and a.district_id=b.district_id and a.district_id<>" & distid & "")
        Return st
    End Function
    <WebMethod()> _
    Public Function modibank(ByVal dist_id As Integer, ByVal ifsc As String) As DataSet
        Dim ds As DataSet
        ds = oh.ExecuteDataSet("select ifsc_code,bankname||','||branch as bank from neft_bank_mst where ifsc_code='" & ifsc & "' union all select ifsc_code,bankname||','||branch as bank from neft_bank_mst where dist_id=" & dist_id & " and ifsc_code<>'" & ifsc & "' ")
        Return ds
    End Function
    <WebMethod()> _
    Public Function Neftpendinglist(ByVal fdt As Date, ByVal tdt As Date, ByVal ModuleId As Integer) As DataSet
        Dim ds As DataSet
        If ModuleId = 0 Then
            ds = oh.ExecuteDataSet("select substr(c.branch_name,0,20) as branch_name,a.doc_id,a.cust_id,b.beneficiary_account,b.cust_name,a.amount,to_char(a.value_date, 'dd/mm/yyyy') as tra_dt,to_char(a.send_date, 'dd/mm/yyyy') as sendt,'Neft Pending Report' as rpt,d.mod_descr as modtype, F.FIRM_ABBR,b.ifsc_code from neft_master a, neft_customer b,branch_master c, firm_master f,mod_master d where a.cust_id = b.cust_id and a.status_id =1 and a.branch_id=c.branch_id and to_date(a.value_date)>= to_date('" & Format(fdt, "dd/MMM/yyyy") & "') and to_date(a.value_date)<= to_date('" & Format(tdt, "dd/MMM/yyyy") & "') and f.firm_id=a.firm_id and a.module_id=d.module_id(+) group by c.branch_name,a.doc_id,a.cust_id,b.beneficiary_account,b.cust_name,a.amount,to_char(a.value_date, 'dd/mm/yyyy'),to_char(a.send_date, 'dd/mm/yyyy'),d.mod_descr, F.FIRM_ABBR,b.ifsc_code order by c.branch_name,d.mod_descr,to_char(a.value_date, 'dd/mm/yyyy')")
        Else
            ds = oh.ExecuteDataSet("select substr(c.branch_name,0,20) as branch_name,a.doc_id,a.cust_id,b.beneficiary_account,b.cust_name,a.amount,to_char(a.value_date, 'dd/mm/yyyy') as tra_dt,to_char(a.send_date, 'dd/mm/yyyy') as sendt,'Neft Pending Report' as rpt,d.mod_descr as modtype, F.FIRM_ABBR,b.ifsc_code from neft_master a, neft_customer b,branch_master c, firm_master f,mod_master d where a.cust_id = b.cust_id and a.status_id =1 and a.branch_id=c.branch_id and to_date(a.value_date)>= to_date('" & Format(fdt, "dd/MMM/yyyy") & "') and to_date(a.value_date)<= to_date('" & Format(tdt, "dd/MMM/yyyy") & "') and a.module_id=" & ModuleId & " and f.firm_id=a.firm_id and a.module_id=d.module_id(+) group by c.branch_name,a.doc_id,a.cust_id,b.beneficiary_account,b.cust_name,a.amount,to_char(a.value_date, 'dd/mm/yyyy'),to_char(a.send_date, 'dd/mm/yyyy'),d.mod_descr,F.FIRM_ABBR,b.ifsc_code order by c.branch_name,to_char(a.value_date, 'dd/mm/yyyy')")
        End If
        Return ds
    End Function
    <WebMethod()> _
    Public Function Neftreceivinglist(ByVal fdt As Date, ByVal tdt As Date, ByVal ModuleId As Integer) As DataSet
        Dim ds As DataSet
        If ModuleId = 0 Then
            ds = oh.ExecuteDataSet("select substr(c.branch_name,0,20) as branch_name,a.doc_id,a.cust_id,b.beneficiary_account,b.cust_name,a.amount,to_char(a.value_date, 'dd/mm/yyyy') as tra_dt,to_char(a.send_date, 'dd/mm/yyyy') as sendt,'Neft Receiving Report' as rpt,d.mod_descr as modtype, F.FIRM_ABBR,b.ifsc_code from neft_master a, neft_customer b,branch_master c, firm_master f ,mod_master d where a.cust_id = b.cust_id and a.status_id =0 and to_date(a.send_date) is not null and a.branch_id=c.branch_id  and to_date(a.send_date)>= to_date('" & Format(fdt, "dd/MMM/yyyy") & "') and to_date(a.send_date)<= to_date('" & Format(tdt, "dd/MMM/yyyy") & "') and f.firm_id=a.firm_id and a.module_id=d.module_id(+) group by c.branch_name,a.doc_id,a.cust_id,b.beneficiary_account,b.cust_name,a.amount,to_char(a.value_date, 'dd/mm/yyyy'),to_char(a.send_date, 'dd/mm/yyyy'),d.mod_descr,F.FIRM_ABBR,b.ifsc_code order by c.branch_name,d.mod_descr,to_char(a.value_date, 'dd/mm/yyyy')")
        Else
            ds = oh.ExecuteDataSet("select substr(c.branch_name,0,20) as branch_name,a.doc_id,a.cust_id,b.beneficiary_account,b.cust_name,a.amount,to_char(a.value_date, 'dd/mm/yyyy') as tra_dt,to_char(a.send_date, 'dd/mm/yyyy') as sendt,'Neft Receiving Report' as rpt,d.mod_descr as modtype, F.FIRM_ABBR,b.ifsc_code from neft_master a, neft_customer b,branch_master c, firm_master f,mod_master d where a.cust_id = b.cust_id and a.status_id =0 and to_date(a.send_date) is not null and a.branch_id=c.branch_id  and to_date(a.send_date)>= to_date('" & Format(fdt, "dd/MMM/yyyy") & "') and to_date(a.send_date)<= to_date('" & Format(tdt, "dd/MMM/yyyy") & "')  and a.module_id =" & ModuleId & " and f.firm_id=a.firm_id and a.module_id=d.module_id(+) group by c.branch_name,a.doc_id,a.cust_id,b.beneficiary_account,b.cust_name,a.amount,to_char(a.value_date, 'dd/mm/yyyy'),to_char(a.send_date, 'dd/mm/yyyy'),d.mod_descr,F.FIRM_ABBR,b.ifsc_code union all select substr(c.branch_name,0,20) as branch_name,a.doc_id,a.cust_id,b.beneficiary_account,b.cust_name,a.amount,to_char(a.value_date, 'dd/mm/yyyy') as tra_dt,to_char(a.send_date, 'dd/mm/yyyy') as sendt,'Neft Receiving Report' as rpt,d.mod_descr as modtype, F.FIRM_ABBR,b.ifsc_code from neft_master_his a, neft_customer b,branch_master c, firm_master f,mod_master d  where a.cust_id = b.cust_id and a.status_id =0 and to_date(a.send_date) is not null and a.branch_id=c.branch_id  and to_date(a.send_date)>= to_date('" & Format(fdt, "dd/MMM/yyyy") & "') and to_date(a.send_date)<= to_date('" & Format(tdt, "dd/MMM/yyyy") & "')  and a.module_id =" & ModuleId & " and f.firm_id=a.firm_id and a.module_id=d.module_id(+) group by c.branch_name,a.doc_id,a.cust_id,b.beneficiary_account,b.cust_name,a.amount,to_char(a.value_date, 'dd/mm/yyyy'),to_char(a.send_date, 'dd/mm/yyyy'),d.mod_descr,F.FIRM_ABBR,b.ifsc_code order by branch_name,tra_dt")
        End If

        Return ds
    End Function

    <WebMethod()> _
    Public Function customeraddCerDtls(ByVal confstr As String, ByVal FirmID As Integer, ByVal BranchID As Integer, ByVal User_id As String)
        Dim datastr() As String = confstr.Split("¥")
        Dim pr(4) As OracleParameter
        Dim message As String = ""
        Dim oh1 As New Helper.Oracle.OracleHelper
        Try
            pr(0) = New OracleParameter("CustData", OracleType.VarChar, 200)
            pr(0).Value = CStr(confstr)
            pr(0).Direction = ParameterDirection.Input
            'Data :1.CerId,2.CerNo,3.CustName,4.CustId

            pr(1) = New OracleParameter("FirmID", OracleType.Number, 5)
            pr(1).Value = CInt(FirmID)
            pr(1).Direction = ParameterDirection.Input

            pr(2) = New OracleParameter("BranchID", OracleType.Number, 6)
            pr(2).Value = CInt(BranchID)
            pr(2).Direction = ParameterDirection.Input

            pr(3) = New OracleParameter("ErrStat", OracleType.Number, 3)
            pr(3).Direction = ParameterDirection.Output

            pr(4) = New OracleParameter("ErrMsg", OracleType.VarChar, 50)
            pr(4).Direction = ParameterDirection.Output

            oh1.ExecuteNonQuery("Add_CustLanDtlsConfirm", pr)

            message = pr(4).Value.ToString

        Catch ex As Exception
            message = ex.Message
        End Try
        Return message

    End Function


    <WebMethod()> _
    Public Function neftdtlpaythru(ByVal grpid As Integer, ByVal st As Integer) As DataSet

        'NEFT CONFIRMATION NEFTDTLPAYTHRU

        Dim ds As DataSet
        If st = 2 Then  'NT'
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,to_char(a.send_transid) as corporationid,a.amount,to_char(to_date(a.send_date),'dd/mm/yyyy') as valuedt,'UTIB0000046' as senderifsc,c.ifsc_code beneficiaryifsc,c.beneficiary_account as ben_account,ltrim(c.cust_name, 102) as ben_name,to_char(a.send_transid) as sender_information,'' as email,'' as emailbody,e.parmtr_value as debaccno,'11' as sen_acc_type,c.acc_type as rec_acc_type from neft_master a, neft_customer c, firm_master d, general_parameter e where a.cust_id = c.cust_id and a.firm_id = d.firm_id and e.parmtr_id = 58 and e.module_id=1 and e.firm_id = a.firm_id and c.verify_status = 'T' and a.send_transid is not null and a.status_id = 0 and a.typestatus <> 'DC' and a.group_id = " & grpid & " and a.module_id in (2,5,6,20,88) order by paymentidentifier,corporationid ")
        ElseIf st = 3 Then
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,to_char(a.send_transid) as corporationid,a.amount,to_char(to_date(a.send_date),'dd/mm/yyyy') as valuedt,'UTIB0000046' as senderifsc,c.ifsc_code beneficiaryifsc,c.beneficiary_account as ben_account,ltrim(c.cust_name, 102) as ben_name,to_char(a.send_transid) as sender_information,'' as email,'' as emailbody,e.parmtr_value as debaccno,'11' as sen_acc_type,c.acc_type as rec_acc_type from neft_master a, neft_customer c, firm_master d, general_parameter e where a.cust_id = c.cust_id and a.firm_id = d.firm_id and e.parmtr_id = 56 and e.module_id=1 and e.firm_id = a.firm_id and c.verify_status = 'T' and a.send_transid is not null and a.status_id = 0 and a.typestatus <> 'DC' and a.group_id = " & grpid & " and a.module_id=44 order by paymentidentifier,corporationid ")
        ElseIf st = 4 Then   'DC
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,to_char(a.send_transid) as corporationid,a.amount,to_char(to_date(a.send_date),'dd/mm/yyyy') as valuedt,c.beneficiary_account as senderifsc,'MANAPPURAM FINANCE  LIMITED' as beneficiaryifsc,'' as ben_account,'' as ben_name,'912020067992955' as sender_information,to_char(a.send_transid) as email,'' as emailbody,'' as debaccno,'' as sen_acc_type,c.beneficiary_account as rec_acc_type from neft_master a, neft_customer c where a.cust_id = c.cust_id and c.verify_status = 'T' and a.send_transid is not null and to_date(a.send_date) is not null and a.status_id = 0 and a.typestatus = 'DC' and a.group_id = " & grpid & " and a.module_id  in (2,5,6,20,88) /*not in (1,39,44,90,91,33)*/ order by paymentidentifier,corporationid ")
            ' NEFT PAYTHRU EMPLOYEE SALARY
        ElseIf st = 5 Then ' PAYTHRU EMPLOYEE
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,to_char(a.send_transid) as corporationid,a.amount,to_char(to_date(a.send_date),'dd/mm/yyyy') as valuedt,c.beneficiary_account as senderifsc,'MANAPPURAM FINANCE  LIMITED' as beneficiaryifsc,'' as ben_account,'' as ben_name,'912020022744584' as sender_information,to_char(a.send_transid) as email,'' as emailbody,'' as debaccno,'' as sen_acc_type,c.beneficiary_account as rec_acc_type from neft_master a, neft_customer c where a.branch_id = c.branch_id and a.cust_id = c.cust_id and c.verify_status = 'T' and a.send_transid is not null and to_date(a.send_date) is not null and a.status_id = 0 and a.typestatus = 'DC' and a.group_id = " & grpid & " and a.module_id =90 and a.firm_id<>24  order by paymentidentifier,corporationid ")
        ElseIf st = 6 Then ' PAYTHRU JEWELLERY EMPLOYEE--------------Change to Bonus
            'ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,to_char(a.send_transid) as corporationid,a.amount,to_char(to_date(a.send_date),'dd/mm/yyyy') as valuedt,c.beneficiary_account as senderifsc,'MANAPPURAM FINANCE  LIMITED' as beneficiaryifsc,'' as ben_account,'' as ben_name,'910020002134864' as sender_information,c.beneficiary_account as email,'' as emailbody,'' as debaccno,'' as sen_acc_type,c.beneficiary_account as rec_acc_type from neft_master a, neft_customer c where a.branch_id = c.branch_id and a.cust_id = c.cust_id and c.verify_status = 'T' and a.send_transid is not null and to_date(a.send_date) is not null and a.status_id = 0 and a.typestatus = 'DC' and a.group_id = " & grpid & " and a.module_id=90 and a.firm_id=24 order by paymentidentifier,corporationid ")
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,to_char(a.send_transid) as corporationid,a.amount,to_char(to_date(a.send_date),'dd/mm/yyyy') as valuedt,c.beneficiary_account as senderifsc,'MANAPPURAM FINANCE  LIMITED' as beneficiaryifsc,'' as ben_account,'' as ben_name,'910020002134864' as sender_information,to_char(a.send_transid) as email,'' as emailbody,'' as debaccno,'' as sen_acc_type,c.beneficiary_account as rec_acc_type from neft_master a, neft_customer c where a.branch_id = c.branch_id and a.cust_id = c.cust_id and c.verify_status = 'T' and a.send_transid is not null and to_date(a.send_date) is not null and a.status_id = 0 and a.typestatus = 'DC' and a.group_id = " & grpid & " and a.module_id=53 and a.firm_id=1 and rownum<1000 order by paymentidentifier,corporationid ")
        ElseIf st = 7 Then ' PAYTHRU sec
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,to_char(a.send_transid) as corporationid,a.amount,to_char(to_date(a.send_date),'dd/mm/yyyy') as valuedt,c.beneficiary_account as senderifsc,'MANAPPURAM FINANCE  LIMITED' as beneficiaryifsc,'' as ben_account,'' as ben_name,'912020022744584' as sender_information,to_char(a.send_transid) as email,'' as emailbody,'' as debaccno,'' as sen_acc_type,c.beneficiary_account as rec_acc_type from neft_master a, neft_customer c where a.branch_id = c.branch_id and a.cust_id = c.cust_id and c.verify_status = 'T' and a.send_transid is not null and to_date(a.send_date) is not null and a.status_id = 0 and a.typestatus = 'DC' and a.group_id = " & grpid & " and a.module_id =33 and a.firm_id<>24  order by paymentidentifier,corporationid ")
        ElseIf st = 8 Then ' PAYTHRU pt
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,to_char(a.send_transid) as corporationid,a.amount,to_char(to_date(a.send_date),'dd/mm/yyyy') as valuedt,c.beneficiary_account as senderifsc,'MANAPPURAM FINANCE  LIMITED' as beneficiaryifsc,'' as ben_account,'' as ben_name,'912020022744584' as sender_information,to_char(a.send_transid) as email,'' as emailbody,'' as debaccno,'' as sen_acc_type,c.beneficiary_account as rec_acc_type from neft_master a, neft_customer c where a.branch_id = c.branch_id and a.cust_id = c.cust_id and c.verify_status = 'T' and a.send_transid is not null and to_date(a.send_date) is not null and a.status_id = 0 and a.typestatus = 'DC' and a.group_id = " & grpid & " and a.module_id =91 and a.firm_id<>24  order by paymentidentifier,corporationid ")

        ElseIf st = 9 Then ' NEFT  EMPLOYEE
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,to_char(a.send_transid) as corporationid,a.amount,to_char(to_date(a.send_date),'dd/mm/yyyy') as valuedt,'UTIB0000046' as senderifsc,c.ifsc_code beneficiaryifsc,c.beneficiary_account as ben_account,ltrim(c.cust_name, 102) as ben_name,to_char(a.send_transid) as sender_information,'' as email,'' as emailbody,e.parmtr_value as debaccno,'11' as sen_acc_type,c.acc_type as rec_acc_type from neft_master a, neft_customer c, firm_master d, general_parameter e where a.cust_id = c.cust_id and a.firm_id = d.firm_id and e.parmtr_id = 57 and e.module_id=1 and e.firm_id = a.firm_id and c.verify_status = 'T' and a.send_transid is not null and a.status_id = 0 and a.typestatus <> 'DC'  and a.module_id =90 and a.firm_id<>24 and a.group_id = " & grpid & "  order by paymentidentifier,corporationid ")
        ElseIf st = 10 Then ' NEFT  EMPLOYEE jewellery--------------Change to Bonus
            'ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,to_char(a.send_transid) as corporationid,a.amount,to_char(to_date(a.send_date),'dd/mm/yyyy') as valuedt,'UTIB0000046' as senderifsc,c.ifsc_code beneficiaryifsc,c.beneficiary_account as ben_account,ltrim(c.cust_name, 102) as ben_name,decode(a.typestatus,'NE','NEFT TRANSFER','RT','RTGS TRANSFER') as sender_information,'' as email,'' as emailbody,e.parmtr_value as debaccno,'11' as sen_acc_type,c.acc_type as rec_acc_type from neft_master a, neft_customer c, firm_master d, general_parameter e where a.cust_id = c.cust_id and a.firm_id = d.firm_id and e.parmtr_id = 56 and e.module_id=1 and e.firm_id = a.firm_id and c.verify_status = 'T' and a.send_transid is not null and a.status_id = 0 and a.typestatus <> 'DC'  and a.module_id =90 and a.firm_id=24 and a.group_id = " & grpid & "  order by paymentidentifier,corporationid ")
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,to_char(a.send_transid) as corporationid,a.amount,to_char(to_date(a.send_date),'dd/mm/yyyy') as valuedt,'UTIB0000046' as senderifsc,c.ifsc_code beneficiaryifsc,c.beneficiary_account as ben_account,ltrim(c.cust_name, 102) as ben_name,to_char(a.send_transid) as sender_information,'' as email,'' as emailbody,e.parmtr_value as debaccno,'11' as sen_acc_type,c.acc_type as rec_acc_type from neft_master a, neft_customer c, firm_master d, general_parameter e where a.cust_id = c.cust_id and a.firm_id = d.firm_id and e.parmtr_id = 56 and e.module_id=1 and e.firm_id = a.firm_id and c.verify_status = 'T' and a.send_transid is not null and a.status_id = 0 and a.typestatus <> 'DC'  and a.module_id =53 and a.firm_id=1 and a.group_id = " & grpid & " and rownum<1000 order by paymentidentifier,corporationid ")
        ElseIf st = 11 Then ' NEFT sec
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,to_char(a.send_transid) as corporationid,a.amount,to_char(to_date(a.send_date),'dd/mm/yyyy') as valuedt,'UTIB0000046' as senderifsc,c.ifsc_code beneficiaryifsc,c.beneficiary_account as ben_account,ltrim(c.cust_name, 102) as ben_name,to_char(a.send_transid) as sender_information,'' as email,'' as emailbody,e.parmtr_value as debaccno,'11' as sen_acc_type,c.acc_type as rec_acc_type from neft_master a, neft_customer c, firm_master d, general_parameter e where a.cust_id = c.cust_id and a.firm_id = d.firm_id and e.parmtr_id = 57 and e.module_id=1 and e.firm_id = a.firm_id and c.verify_status = 'T' and a.send_transid is not null and a.status_id = 0 and a.typestatus <> 'DC'  and a.module_id =33 and a.firm_id<>24 and a.group_id = " & grpid & "  order by paymentidentifier,corporationid ")
        ElseIf st = 12 Then ' NEFT pt
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,to_char(a.send_transid) as corporationid,a.amount,to_char(to_date(a.send_date),'dd/mm/yyyy') as valuedt,'UTIB0000046' as senderifsc,c.ifsc_code beneficiaryifsc,c.beneficiary_account as ben_account,ltrim(c.cust_name, 102) as ben_name,to_char(a.send_transid) as sender_information,'' as email,'' as emailbody,e.parmtr_value as debaccno,'11' as sen_acc_type,c.acc_type as rec_acc_type from neft_master a, neft_customer c, firm_master d, general_parameter e where a.cust_id = c.cust_id and a.firm_id = d.firm_id and e.parmtr_id = 57 and e.module_id=1 and e.firm_id = a.firm_id and c.verify_status = 'T' and a.send_transid is not null and a.status_id = 0 and a.typestatus <> 'DC'  and a.module_id =91 and a.firm_id<>24 and a.group_id = " & grpid & "  order by paymentidentifier,corporationid ")

        ElseIf st = 13 Then ' PAYTHRU TDS
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,to_char(a.send_transid) as corporationid,a.amount,to_char(to_date(a.send_date),'dd/mm/yyyy') as valuedt,c.beneficiary_account as senderifsc,'MANAPPURAM FINANCE  LIMITED' as beneficiaryifsc,'' as ben_account,'' as ben_name,'910020002134864' as sender_information,to_char(a.send_transid) as email,'' as emailbody,'' as debaccno,'' as sen_acc_type,c.beneficiary_account as rec_acc_type from neft_master a, neft_customer c where a.branch_id = c.branch_id and a.cust_id = c.cust_id and c.verify_status = 'T' and a.send_transid is not null and to_date(a.send_date) is not null and a.status_id = 0 and a.typestatus = 'DC' and a.group_id = " & grpid & " and a.module_id =64  order by paymentidentifier,corporationid ")
        ElseIf st = 14 Then ' NEFT TDS
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,to_char(a.send_transid) as corporationid,a.amount,to_char(to_date(a.send_date),'dd/mm/yyyy') as valuedt,'UTIB0000046' as senderifsc,c.ifsc_code beneficiaryifsc,c.beneficiary_account as ben_account,ltrim(c.cust_name, 102) as ben_name,to_char(a.send_transid) as sender_information,'' as email,'' as emailbody,e.parmtr_value as debaccno,'11' as sen_acc_type,c.acc_type as rec_acc_type from neft_master a, neft_customer c, firm_master d, general_parameter e where a.cust_id = c.cust_id and a.firm_id = d.firm_id and e.parmtr_id = 56 and e.module_id=1 and e.firm_id = a.firm_id and c.verify_status = 'T' and a.send_transid is not null and a.status_id = 0 and a.typestatus <> 'DC'  and a.module_id =64 and  a.group_id = " & grpid & "  order by paymentidentifier,corporationid ")

        ElseIf st = 15 Then ' PAYTHRU MONEY TRANSFER
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,to_char(a.send_transid) as corporationid,a.amount,to_char(to_date(a.send_date),'dd/mm/yyyy') as valuedt,c.beneficiary_account as senderifsc,'MANAPPURAM FINANCE  LIMITED' as beneficiaryifsc,'' as ben_account,'' as ben_name,'910020002134864' as sender_information,to_char(a.send_transid) as email,'' as emailbody,'' as debaccno,'' as sen_acc_type,c.beneficiary_account as rec_acc_type from neft_master a, neft_customer c where  a.cust_id = c.cust_id  and a.send_transid is not null and to_date(a.send_date) is not null and a.status_id = 0 and a.typestatus = 'DC' and a.group_id = " & grpid & " and a.module_id =15  order by paymentidentifier,corporationid ")
        ElseIf st = 16 Then ' NEFT MONEY TRANSFER 
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  to_char(a.send_transid) as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  ltrim(c.cust_name, 102) as ben_name,  to_char(a.send_transid) as sender_information,  '' as email,  '' as emailbody,  e.parmtr_value as debaccno,  '11' as sen_acc_type,  c.acc_type as rec_acc_type  from neft_master a, neft_customer c, firm_master d, general_parameter e  where a.cust_id = c.cust_id  and a.firm_id = d.firm_id  and e.parmtr_id = 56  and e.module_id = 1  and e.firm_id = a.firm_id and a.send_transid is not null  and a.status_id = 0  and a.typestatus <> 'DC'  and a.module_id = 15  and a.group_id = " & grpid & "  order by paymentidentifier, corporationid")

        ElseIf st = 17 Then ' PAYTHRU RENT 
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,to_char(a.send_transid) as corporationid,a.amount,to_char(to_date(a.send_date),'dd/mm/yyyy') as valuedt,c.beneficiary_account as senderifsc,'MANAPPURAM FINANCE  LIMITED' as beneficiaryifsc,'' as ben_account,'' as ben_name,'910020002134864' as sender_information,to_char(a.send_transid) as email,'' as emailbody,'' as debaccno,'' as sen_acc_type,c.beneficiary_account as rec_acc_type from neft_master a, neft_customer c where a.branch_id = c.branch_id and a.cust_id = c.cust_id and c.verify_status = 'T' and a.send_transid is not null and to_date(a.send_date) is not null and a.status_id = 0 and a.typestatus = 'DC' and a.group_id = " & grpid & " and a.module_id =34  order by paymentidentifier,corporationid ")
        ElseIf st = 18 Then ' NEFT / RTGS  RENT 
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,to_char(a.send_transid) as corporationid,a.amount,to_char(to_date(a.send_date),'dd/mm/yyyy') as valuedt,'UTIB0000046' as senderifsc,c.ifsc_code beneficiaryifsc,c.beneficiary_account as ben_account,ltrim(c.cust_name, 102) as ben_name,to_char(a.send_transid) as sender_information,'' as email,'' as emailbody,e.parmtr_value as debaccno,'11' as sen_acc_type,c.acc_type as rec_acc_type from neft_master a, neft_customer c, firm_master d, general_parameter e where a.cust_id = c.cust_id and a.firm_id = d.firm_id and e.parmtr_id = 56 and e.module_id=1 and e.firm_id = a.firm_id and c.verify_status = 'T' and a.send_transid is not null and a.status_id = 0 and a.typestatus <> 'DC'  and a.module_id =34 and  a.group_id = " & grpid & "  order by paymentidentifier,corporationid ")

        ElseIf st = 19 Then ' PAYTHRU Store 
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,to_char(a.send_transid) as corporationid,a.amount,to_char(to_date(a.send_date),'dd/mm/yyyy') as valuedt,c.beneficiary_account as senderifsc,'MANAPPURAM FINANCE  LIMITED' as beneficiaryifsc,'' as ben_account,'' as ben_name,'910020002134864' as sender_information,to_char(a.send_transid) as email,'' as emailbody,'' as debaccno,'' as sen_acc_type,c.beneficiary_account as rec_acc_type from neft_master a, neft_customer c where a.branch_id = c.branch_id and a.cust_id = c.cust_id and c.verify_status = 'T' and a.send_transid is not null and to_date(a.send_date) is not null and a.status_id = 0 and a.typestatus = 'DC' and a.group_id = " & grpid & " and a.module_id =39  order by paymentidentifier,corporationid ")
        ElseIf st = 20 Then ' NEFT / RTGS  Store 
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,to_char(a.send_transid) as corporationid,a.amount,to_char(to_date(a.send_date),'dd/mm/yyyy') as valuedt,'UTIB0000046' as senderifsc,c.ifsc_code beneficiaryifsc,c.beneficiary_account as ben_account,ltrim(c.cust_name, 102) as ben_name,to_char(a.send_transid) as sender_information,'' as email,'' as emailbody,e.parmtr_value as debaccno,'11' as sen_acc_type,c.acc_type as rec_acc_type from neft_master a, neft_customer c, firm_master d, general_parameter e where a.cust_id = c.cust_id and a.firm_id = d.firm_id and e.parmtr_id = 56 and e.module_id=1 and e.firm_id = a.firm_id and c.verify_status = 'T' and a.send_transid is not null and a.status_id = 0 and a.typestatus <> 'DC'  and a.module_id =39 and  a.group_id = " & grpid & "  order by paymentidentifier,corporationid ")

        ElseIf st = 21 Then ' PAYTHRU ADVERTISEMENT 
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,to_char(a.send_transid) as corporationid,a.amount,to_char(to_date(a.send_date),'dd/mm/yyyy') as valuedt,c.beneficiary_account as senderifsc,'MANAPPURAM FINANCE  LIMITED' as beneficiaryifsc,'' as ben_account,'' as ben_name,'910020002134864' as sender_information,to_char(a.send_transid) as email,'' as emailbody,'' as debaccno,'' as sen_acc_type,c.beneficiary_account as rec_acc_type from neft_master a, neft_customer c where a.branch_id = c.branch_id and a.cust_id = c.cust_id and c.verify_status = 'T' and a.send_transid is not null and to_date(a.send_date) is not null and a.status_id = 0 and a.typestatus = 'DC' and a.group_id = " & grpid & " and a.module_id =94  order by paymentidentifier,corporationid ")
        ElseIf st = 22 Then ' NEFT / RTGS  ADVERTISEMENT 
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,to_char(a.send_transid) as corporationid,a.amount,to_char(to_date(a.send_date),'dd/mm/yyyy') as valuedt,'UTIB0000046' as senderifsc,c.ifsc_code beneficiaryifsc,c.beneficiary_account as ben_account,ltrim(c.cust_name, 102) as ben_name,to_char(a.send_transid) as sender_information,'' as email,'' as emailbody,e.parmtr_value as debaccno,'11' as sen_acc_type,c.acc_type as rec_acc_type from neft_master a, neft_customer c, firm_master d, general_parameter e where a.cust_id = c.cust_id and a.firm_id = d.firm_id and e.parmtr_id = 56 and e.module_id=1 and e.firm_id = a.firm_id and c.verify_status = 'T' and a.send_transid is not null and a.status_id = 0 and a.typestatus <> 'DC'  and a.module_id =94 and  a.group_id = " & grpid & "  order by paymentidentifier,corporationid ")

        ElseIf st = 23 Then ' PAYTHRU Goldloan AUCTION SURPLUS PAYMENT
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,to_char(a.send_transid) as corporationid,a.amount,to_char(to_date(a.send_date),'dd/mm/yyyy') as valuedt,c.beneficiary_account as senderifsc,'MANAPPURAM FINANCE  LIMITED' as beneficiaryifsc,'' as ben_account,'' as ben_name,'0084073000004418' as sender_information,to_char(a.send_transid) as email,'' as emailbody,'' as debaccno,'' as sen_acc_type,c.beneficiary_account as rec_acc_type from neft_master a, neft_customer c where  a.cust_id = c.cust_id  and a.send_transid is not null and to_date(a.send_date) is not null and a.status_id = 0 and a.typestatus = 'DC' and a.group_id = " & grpid & " and a.module_id =7  order by paymentidentifier,corporationid ")
        ElseIf st = 24 Then ' NEFT / RTGS  Goldloan AUCTION SURPLUS PAYMENT
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,to_char(a.send_transid) as corporationid,a.amount,to_char(to_date(a.send_date),'dd/mm/yyyy') as valuedt,'UTIB0000046' as senderifsc,c.ifsc_code beneficiaryifsc,c.beneficiary_account as ben_account,ltrim(c.cust_name, 102) as ben_name,to_char(a.send_transid) as sender_information,'' as email,'' as emailbody,e.parmtr_value as debaccno,'11' as sen_acc_type,c.acc_type as rec_acc_type from neft_master a, neft_customer c, firm_master d, general_parameter e where a.cust_id = c.cust_id and a.firm_id = d.firm_id and e.parmtr_id = 56 and e.module_id=2 and e.firm_id = a.firm_id   and a.send_transid is not null and a.status_id = 0 and a.typestatus <> 'DC'  and a.module_id =7 and  a.group_id = " & grpid & "  order by paymentidentifier,corporationid ")

        ElseIf st = 25 Then ' PAYTHRU HP PAYMENT
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,to_char(a.send_transid) as corporationid,a.amount,to_char(to_date(a.send_date),'dd/mm/yyyy') as valuedt,c.beneficiary_account as senderifsc,'MANAPPURAM FINANCE  LIMITED' as beneficiaryifsc,'' as ben_account,'' as ben_name,'910020002134864' as sender_information,to_char(a.send_transid) as email,'' as emailbody,'' as debaccno,'' as sen_acc_type,c.beneficiary_account as rec_acc_type from neft_master a, neft_customer c where  a.cust_id = c.cust_id and c.verify_status = 'T' and a.send_transid is not null and to_date(a.send_date) is not null and a.status_id = 0 and a.typestatus = 'DC' and a.group_id = " & grpid & " and a.module_id =150  order by paymentidentifier,corporationid ")
        ElseIf st = 26 Then ' NEFT / RTGS  HP PAYMENT
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,to_char(a.send_transid) as corporationid,a.amount,to_char(to_date(a.send_date),'dd/mm/yyyy') as valuedt,'UTIB0000046' as senderifsc,c.ifsc_code beneficiaryifsc,c.beneficiary_account as ben_account,ltrim(c.cust_name, 102) as ben_name,to_char(a.send_transid) as sender_information,'' as email,'' as emailbody,e.parmtr_value as debaccno,'11' as sen_acc_type,c.acc_type as rec_acc_type from neft_master a, neft_customer c, firm_master d, general_parameter e where a.cust_id = c.cust_id and a.firm_id = d.firm_id and e.parmtr_id = 56 and e.module_id=1 and e.firm_id = a.firm_id and c.verify_status = 'T' and a.send_transid is not null and a.status_id = 0 and a.typestatus <> 'DC'  and a.module_id =150 and  a.group_id = " & grpid & "  order by paymentidentifier,corporationid ")
        ElseIf st = 27 Then  'DEPOSIT MAAFIN PAYMENT PAYTHRU
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier, to_char(a.send_transid) as corporationid,a.amount,to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,c.beneficiary_account as senderifsc,b.firm_name as beneficiaryifsc,'' as ben_account,'' as ben_name,e.parmtr_name as sender_information,c.beneficiary_account as email,'' as emailbody,'' as debaccno,'' as sen_acc_type,c.beneficiary_account as rec_acc_type from neft_master a, neft_customer c,general_parameter e,firm_master b where a.firm_id=4 and a.cust_id = c.cust_id and e.parmtr_id = 56 and b.firm_id=a.firm_id and e.module_id = 1 and e.firm_id = a.firm_id and c.verify_status = 'T' and a.send_transid is not null  and to_date(a.send_date) is not null and a.status_id = 0 and a.typestatus = 'DC'  and a.group_id = " & grpid & "  order by paymentidentifier, corporationid")
        ElseIf st = 28 Then 'DEPOSIT MAAFIN PAYMENT NEFT/RTGS
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  to_char(a.send_transid) as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  ltrim(c.cust_name, 102) as ben_name,  to_char(a.send_transid) as sender_information,  '' as email,  '' as emailbody,  e.parmtr_value as debaccno,  '11' as sen_acc_type,  c.acc_type as rec_acc_type  from neft_master       a,  neft_customer     c,  firm_master       d,  general_parameter e  where a.firm_id=4 and a.cust_id = c.cust_id  and a.firm_id = d.firm_id  and e.parmtr_id = 56  and e.module_id = 1  and e.firm_id = a.firm_id  and c.verify_status = 'T'  and a.send_transid is not null  and a.status_id = 0  and a.typestatus <> 'DC'  and a.group_id = " & grpid & "  order by paymentidentifier, corporationid")

        ElseIf st = 29 Then ' PAYTHRU MAGRO MARCH 17 , 2012, modified mar 19 2012
            'ds = oh.ExecuteDataSet("select to_char(e.parmtr_value) as account_no,  a.amount,  '' as TranParticulars,  c.ifsc_code ifsc_code,  to_char(c.beneficiary_account) as Benefeciary_Account,  ltrim(c.cust_name, 102) as Benefeciary_Name,  c1.fat_hus || ',' || c1.house_name || ',' || c1.locality || ',' ||  c1.street || ',Pincode:' ||  (select p.pin_code  from post_master p  where p.sr_number = c1.pin_serial) as address,  '' as Senders_to_Receiver,  '' as Senders_to_Receiver1,  '' as Charge_Account    from neft_master       a,  neft_customer     c,  firm_master       d,  general_parameter e,  customer          c1  where a.cust_id = c.cust_id  and a.firm_id = d.firm_id  and e.parmtr_id = 56  and e.module_id = 1  and e.firm_id = a.firm_id  and c.verify_status = 'T'  and a.send_transid is not null  and a.status_id = 0  and a.typestatus <> 'DC'  and a.group_id = " & grpid & "  and c.cust_id = c1.cust_id ")

            ds = oh.ExecuteDataSet("select to_char(e.parmtr_value) as account_no,       to_char(a.amount)amount,       '' as TranParticulars,       c.ifsc_code ifsc_code,       to_char(regexp_replace(c.beneficiary_account,'[,-.]','')) as Benefeciary_Account,       ltrim(c.cust_name, 102) as Benefeciary_Name,       regexp_replace(c1.bankname,'[,-.]','')||' '|| regexp_replace(c1.branch ,'[,-.]','') address,       '' as Senders_to_Receiver,       '' as Senders_to_Receiver1,       '' as Charge_Account  from  neft_master       a,        neft_customer     c,        firm_master       d,        general_parameter e,        neft_bank_mst     c1  where a.cust_id = c.cust_id   and a.firm_id = d.firm_id   and e.parmtr_id = 56   and e.module_id = 1   and e.firm_id = a.firm_id   and c.verify_status = 'T'   and a.send_transid is not null   and a.status_id = 0   and a.typestatus <> 'DC'   and a.group_id = " & grpid & "   and c.bank_id= c1.bank_id   and c.ifsc_code=c1.ifsc_code")

        ElseIf st = 30 Then ' NEFT / RTGS  MAGRO MARCH 17 , 2012, modified mar 19 2012
            'ds = oh.ExecuteDataSet("select to_char(e.parmtr_value) as account_no,  a.amount,  '' as TranParticulars,  c.ifsc_code ifsc_code,  to_char(c.beneficiary_account) as Benefeciary_Account,  ltrim(c.cust_name, 102) as Benefeciary_Name,  c1.fat_hus || ',' || c1.house_name || ',' || c1.locality || ',' ||  c1.street || ',Pincode:' ||  (select p.pin_code  from post_master p  where p.sr_number = c1.pin_serial) as address,  '' as Senders_to_Receiver,  '' as Senders_to_Receiver1,  '' as Charge_Account    from neft_master       a,  neft_customer     c,  firm_master       d,  general_parameter e,  customer          c1  where a.cust_id = c.cust_id  and a.firm_id = d.firm_id  and e.parmtr_id = 56  and e.module_id = 1  and e.firm_id = a.firm_id  and c.verify_status = 'T'  and a.send_transid is not null  and a.status_id = 0  and a.typestatus <> 'DC'  and a.group_id = " & grpid & "  and c.cust_id = c1.cust_id")

            ds = oh.ExecuteDataSet("select to_char(e.parmtr_value) as account_no,       to_char(a.amount)amount,       '' as TranParticulars,       c.ifsc_code ifsc_code,       to_char(regexp_replace(c.beneficiary_account,'[,-.]','')) as Benefeciary_Account,       ltrim(c.cust_name, 102) as Benefeciary_Name,       regexp_replace(c1.bankname,'[,-.]','')||' '|| regexp_replace(c1.branch ,'[,-.]','') address,       '' as Senders_to_Receiver,       '' as Senders_to_Receiver1,       '' as Charge_Account  from  neft_master       a,        neft_customer     c,        firm_master       d,        general_parameter e,        neft_bank_mst     c1  where a.cust_id = c.cust_id   and a.firm_id = d.firm_id   and e.parmtr_id = 56   and e.module_id = 1   and e.firm_id = a.firm_id   and c.verify_status = 'T'   and a.send_transid is not null   and a.status_id = 0   and a.typestatus <> 'DC'   and a.group_id = " & grpid & "   and c.bank_id= c1.bank_id   and c.ifsc_code=c1.ifsc_code")

            ''ElseIf st = 29 Then  'DEPOSIT MAGRO PAYMENT PAYTHRU
            ''    ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier, a.corporate_id as corporationid,a.amount,to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,c.beneficiary_account as senderifsc,b.firm_name as beneficiaryifsc,'' as ben_account,'' as ben_name,e.parmtr_name as sender_information,c.beneficiary_account as email,'' as emailbody,'' as debaccno,'' as sen_acc_type,c.beneficiary_account as rec_acc_type from neft_master a, neft_customer c,general_parameter e,firm_master b where a.firm_id=5 and a.cust_id = c.cust_id and e.parmtr_id = 56 and b.firm_id=a.firm_id and e.module_id = 1 and e.firm_id = a.firm_id and c.verify_status = 'T' and a.send_transid is not null  and to_date(a.send_date) is not null and a.status_id = 0 and a.typestatus = 'DC'  and a.group_id = " & grpid & "  order by paymentidentifier, corporationid")
            ''ElseIf st = 30 Then  'DEPOSIT MAGRO PAYMENT NEFT/RTGS
            ''    ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  ltrim(c.cust_name, 102) as ben_name,  decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information,  '' as email,  '' as emailbody,  e.parmtr_value as debaccno,  '11' as sen_acc_type,  c.acc_type as rec_acc_type  from neft_master       a,  neft_customer     c,  firm_master       d,  general_parameter e  where a.firm_id=5 and a.cust_id = c.cust_id  and a.firm_id = d.firm_id  and e.parmtr_id = 56  and e.module_id = 1  and e.firm_id = a.firm_id  and c.verify_status = 'T'  and a.send_transid is not null  and a.status_id = 0  and a.typestatus <> 'DC'  and a.group_id = " & grpid & "  order by paymentidentifier, corporationid")

        ElseIf st = 31 Then  'DEPOSIT MABEN PAYMENT PAYTHRU APR20 2012
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier, to_char(a.send_transid) as corporationid,a.amount,to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,c.beneficiary_account as senderifsc,b.firm_name as beneficiaryifsc,'' as ben_account,'' as ben_name,e.parmtr_name as sender_information,c.beneficiary_account as email,'' as emailbody,'' as debaccno,'' as sen_acc_type,c.beneficiary_account as rec_acc_type from neft_master a, neft_customer c,general_parameter e,firm_master b where a.firm_id=2 and a.cust_id = c.cust_id and e.parmtr_id = 56 and b.firm_id=a.firm_id and e.module_id = 1 and e.firm_id = a.firm_id and c.verify_status = 'T' and a.send_transid is not null  and to_date(a.send_date) is not null and a.status_id = 0 and a.typestatus = 'DC'  and a.group_id = " & grpid & "  order by paymentidentifier, corporationid")
        ElseIf st = 32 Then 'DEPOSIT MABEN PAYMENT NEFT/RTGS APR20 2012
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  to_char(a.send_transid) as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  ltrim(c.cust_name, 102) as ben_name,  to_char(a.send_transid) as sender_information,  '' as email,  '' as emailbody,  e.parmtr_value as debaccno,  '11' as sen_acc_type,  c.acc_type as rec_acc_type  from neft_master       a,  neft_customer     c,  firm_master       d,  general_parameter e  where a.firm_id=2 and a.cust_id = c.cust_id  and a.firm_id = d.firm_id  and e.parmtr_id = 56  and e.module_id = 1  and e.firm_id = a.firm_id  and c.verify_status = 'T'  and a.send_transid is not null  and a.status_id = 0  and a.typestatus <> 'DC'  and a.group_id = " & grpid & "  order by paymentidentifier, corporationid")

        ElseIf st = 35 Then 'final settlement dc
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  to_char(a.send_transid) as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  ltrim(c.cust_name, 102) as ben_name,  to_char(a.send_transid) as sender_information,  '' as email,  '' as emailbody,  e.parmtr_value as debaccno,  '11' as sen_acc_type,  c.acc_type as rec_acc_type  from neft_master       a,  neft_customer     c,  firm_master       d,  general_parameter e  where a.firm_id=1 and a.cust_id = c.cust_id  and a.firm_id = d.firm_id  and e.parmtr_id = 57  and e.module_id = 1  and e.firm_id = a.firm_id  and c.verify_status = 'T'  and a.send_transid is not null  and a.status_id = 0  and a.typestatus = 'DC'  and a.group_id = " & grpid & "  order by paymentidentifier, corporationid")
        ElseIf st = 36 Then 'final settlement nt
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  to_char(a.send_transid) as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  ltrim(c.cust_name, 102) as ben_name,  to_char(a.send_transid) as sender_information,  '' as email,  '' as emailbody,  e.parmtr_value as debaccno,  '11' as sen_acc_type,  c.acc_type as rec_acc_type  from neft_master       a,  neft_customer     c,  firm_master       d,  general_parameter e  where a.firm_id=1 and a.cust_id = c.cust_id  and a.firm_id = d.firm_id  and e.parmtr_id = 57  and e.module_id = 1  and e.firm_id = a.firm_id  and c.verify_status = 'T'  and a.send_transid is not null  and a.status_id = 0  and a.typestatus <> 'DC'  and a.group_id = " & grpid & "  order by paymentidentifier, corporationid")

        ElseIf st = 37 Then 'BAS DC
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  to_char(a.send_transid) as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  ltrim(c.cust_name, 102) as ben_name,  to_char(a.send_transid) as sender_information,  '' as email,  '' as emailbody,  e.parmtr_value as debaccno,  '11' as sen_acc_type,  c.acc_type as rec_acc_type  from neft_master       a,  neft_customer     c,  firm_master       d,  general_parameter e  where a.firm_id=1 and a.cust_id = c.cust_id  and a.firm_id = d.firm_id  and e.parmtr_id = 56  and e.module_id = 1  and e.firm_id = a.firm_id  and c.verify_status = 'T'  and a.send_transid is not null  and a.status_id = 0  and a.typestatus = 'DC'  and a.group_id = " & grpid & "  order by paymentidentifier, corporationid")
        ElseIf st = 38 Then 'BAS nt
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  to_char(a.send_transid) as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  ltrim(c.cust_name, 102) as ben_name,  to_char(a.send_transid) as sender_information,  '' as email,  '' as emailbody,  e.parmtr_value as debaccno,  '11' as sen_acc_type,  c.acc_type as rec_acc_type  from neft_master       a,  neft_customer     c,  firm_master       d,  general_parameter e  where a.firm_id=1 and a.cust_id = c.cust_id  and a.firm_id = d.firm_id  and e.parmtr_id = 56  and e.module_id = 1  and e.firm_id = a.firm_id  and c.verify_status = 'T'  and a.send_transid is not null  and a.status_id = 0  and a.typestatus <> 'DC'  and a.group_id = " & grpid & "  order by paymentidentifier, corporationid")

        ElseIf st = 39 Then 'PNL DC
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,to_char(a.send_transid) as corporationid,a.amount,to_char(to_date(a.send_date),'dd/mm/yyyy') as valuedt,c.beneficiary_account as senderifsc,'MANAPPURAM FINANCE  LIMITED' as beneficiaryifsc,'' as ben_account,'' as ben_name,'910020002134864' as sender_information,to_char(a.send_transid) as email,'' as emailbody,'' as debaccno,'' as sen_acc_type,c.beneficiary_account as rec_acc_type from neft_master a, neft_customer c where a.cust_id = c.cust_id and c.verify_status = 'T' and a.send_transid is not null and to_date(a.send_date) is not null and a.status_id = 0 and a.typestatus = 'DC' and a.group_id = " & grpid & " and a.module_id  =10 order by paymentidentifier,corporationid ")
        ElseIf st = 40 Then 'PNL nt
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,to_char(a.send_transid) as corporationid,a.amount,to_char(to_date(a.send_date),'dd/mm/yyyy') as valuedt,'UTIB0000046' as senderifsc,c.ifsc_code beneficiaryifsc,c.beneficiary_account as ben_account,ltrim(c.cust_name, 102) as ben_name,to_char(a.send_transid) as sender_information,'' as email,'' as emailbody,e.parmtr_value as debaccno,'11' as sen_acc_type,c.acc_type as rec_acc_type from neft_master a, neft_customer c, firm_master d, general_parameter e where a.cust_id = c.cust_id and a.firm_id = d.firm_id and e.parmtr_id = 56 and e.module_id=1 and e.firm_id = a.firm_id and c.verify_status = 'T' and a.send_transid is not null and a.status_id = 0 and a.typestatus <> 'DC' and a.group_id = " & grpid & " and a.module_id =10 order by paymentidentifier,corporationid ")
        ElseIf st = 41 Then ' PAYTHRU GOLDLOAN
            'ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier, to_char(a.send_transid) as corporationid, a.amount, to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt, c.beneficiary_account as senderifsc, 'MANAPPURAM FINANCE  LIMITED' as beneficiaryifsc, '' as ben_account, '' as ben_name, b.account_no as sender_information, c.beneficiary_account as email, '' as emailbody, '' as debaccno, '' as sen_acc_type, c.beneficiary_account as rec_acc_type from neft_master a, neft_customer c,neft_mana_banks b where a.branch_id = c.branch_id and a.cust_id = c.cust_id and c.bank_id=b.bank_id and a.send_transid is not null and to_date(a.send_date) is not null and a.status_id = 0 and a.typestatus = 'DC' and a.group_id = " & grpid & " and a.module_id = 113 order by paymentidentifier, corporationid")
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,       to_char(a.send_transid) as corporationid,       a.amount,       to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,       b.ifsccode as senderifsc,       c.ifsc_code as beneficiaryifsc,       to_char(c.beneficiary_account) as ben_account,       ltrim(c.cust_name, 102) as ben_name,       'DIRECT CREDIT' as sender_information,       to_char(a.send_transid) as email,       '' as emailbody,       b.account_no as debaccno,       '11' as sen_acc_type,       c.acc_type as rec_acc_type  from neft_master a, neft_customer c, neft_mana_banks b where a.cust_id = c.cust_id   and c.bank_id = b.bank_id and b.isactive=1 and a.send_transid is not null   and to_date(a.send_date) is not null   and a.status_id = 0   and a.typestatus = 'DC'   and a.group_id = " & grpid & "   and a.module_id = 113 order by paymentidentifier, corporationid")
        ElseIf st = 42 Then ' NEFT GOLDLOAN 
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,to_char(a.send_transid) as corporationid,a.amount,to_char(to_date(a.send_date),'dd/mm/yyyy') as valuedt,'UTIB0000046' as senderifsc,c.ifsc_code beneficiaryifsc,c.beneficiary_account as ben_account,ltrim(c.cust_name, 102) as ben_name,to_char(a.send_transid) as sender_information,'' as email,'' as emailbody,e.parmtr_value as debaccno,'11' as sen_acc_type,c.acc_type as rec_acc_type from neft_master a, neft_customer c, firm_master d, general_parameter e where a.cust_id = c.cust_id and a.firm_id = d.firm_id and e.parmtr_id = 59 and e.module_id=1 and e.firm_id = a.firm_id and a.send_transid is not null and a.status_id = 0 and a.typestatus <> 'DC'  and a.module_id =113 and  a.group_id = " & grpid & "  order by paymentidentifier,corporationid ")
        ElseIf st = 43 Then ' NEFT GOLDLOAN AUCTIONEER DC
            ' ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier, to_char(a.send_transid) as corporationid, a.amount, to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt, 'UTIB0000046' as senderifsc, c.ifsc_code beneficiaryifsc, c.beneficiary_account as ben_account, ltrim(c.cust_name, 102) as ben_name, decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email, '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id and a.firm_id = 1 and e.parmtr_id = 94 and e.module_id = 7 and e.firm_id = a.firm_id and c.verify_status = 'T' and a.send_transid is not null and to_date(a.send_date) is not null and a.status_id = 0 and a.typestatus = 'DC' and a.module_id = 8 and a.group_id = " & grpid & " order by paymentidentifier, corporationid")
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier, to_char(a.send_transid) as corporationid,a.amount,to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,c.beneficiary_account senderifsc,'MANAPPURAM FINANCE  LIMITED' as beneficiaryifsc,'' as ben_account,'' as ben_name,e.parmtr_value as debaccno, to_char(a.send_transid) as email,'' as emailbody,'' as debaccno,'' as sen_acc_type,c.beneficiary_account as rec_acc_type             from mana0809.neft_master a, mana0809.neft_customer c, mana0809.general_parameter e  where a.cust_id = c.cust_id  and a.firm_id = 1  and e.parmtr_id = 94 and e.module_id = 7 and e.firm_id = a.firm_id and a.send_transid is not null and to_date(a.send_date) is not null  and a.status_id = 0 and a.typestatus = 'DC' and a.module_id = 8 and a.group_id = " & grpid & " order by paymentidentifier, corporationid")
        ElseIf st = 44 Then ' NEFT GOLDLOAN AUCTIONEER NEFT
            'ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier, to_char(a.send_transid) as corporationid, a.amount, to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt, 'UTIB0000046' as senderifsc, c.ifsc_code beneficiaryifsc, c.beneficiary_account as ben_account, ltrim(c.cust_name, 102) as ben_name, decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email, '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type from neft_master a, neft_customer c, firm_master d, general_parameter e where a.cust_id = c.cust_id and a.firm_id = d.firm_id and e.parmtr_id = 56 and e.module_id = 1 and e.firm_id = a.firm_id and c.verify_status = 'T' and a.send_transid is not null and to_date(a.send_date) is not null and a.status_id = 0 and a.typestatus <> 'DC' and a.module_id = 8 and a.group_id = " & grpid & " order by paymentidentifier, corporationid")
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier, to_char(a.send_transid) as corporationid, a.amount, to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt, 'UTIB0000046' as senderifsc, c.ifsc_code beneficiaryifsc, c.beneficiary_account as ben_account, ltrim(c.cust_name, 102) as ben_name, to_char(a.send_transid) as sender_information, '' as email, '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id and a.firm_id = 1 and e.parmtr_id = 94 and e.module_id = 7 and e.firm_id = a.firm_id  and a.send_transid is not null and to_date(a.send_date) is not null and a.status_id = 0 and a.typestatus <> 'DC' and a.module_id = 8 and a.group_id = " & grpid & " order by paymentidentifier, corporationid")
        ElseIf st = 45 Then 'PNL DC
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,to_char(a.send_transid) as corporationid,a.amount,to_char(to_date(a.send_date),'dd/mm/yyyy') as valuedt,c.beneficiary_account as senderifsc,'MANAPPURAM FINANCE  LIMITED' as beneficiaryifsc,'' as ben_account,'' as ben_name,'910020002134864' as sender_information,to_char(a.send_transid) as email,'' as emailbody,'' as debaccno,'' as sen_acc_type,c.beneficiary_account as rec_acc_type from neft_master a, neft_customer c where a.cust_id = c.cust_id and c.verify_status = 'T' and a.send_transid is not null and to_date(a.send_date) is not null and a.status_id = 0 and a.typestatus = 'DC' and a.group_id = " & grpid & " and a.module_id  =47 order by paymentidentifier,corporationid ")
        ElseIf st = 46 Then 'PNL nt
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,to_char(a.send_transid) as corporationid,a.amount,to_char(to_date(a.send_date),'dd/mm/yyyy') as valuedt,'UTIB0000046' as senderifsc,c.ifsc_code beneficiaryifsc,c.beneficiary_account as ben_account,ltrim(c.cust_name, 102) as ben_name,to_char(a.send_transid) as sender_information,'' as email,'' as emailbody,e.parmtr_value as debaccno,'11' as sen_acc_type,c.acc_type as rec_acc_type from neft_master a, neft_customer c, firm_master d, general_parameter e where a.cust_id = c.cust_id and a.firm_id = d.firm_id and e.parmtr_id = 56 and e.module_id=1 and e.firm_id = a.firm_id and c.verify_status = 'T' and a.send_transid is not null and a.status_id = 0 and a.typestatus <> 'DC' and a.group_id = " & grpid & " and a.module_id =47 order by paymentidentifier,corporationid ")

        ElseIf st = 47 Then 'Night Petrolling DC
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  to_char(a.send_transid) as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  c.beneficiary_account as senderifsc,  'MANAPPURAM FINANCE  LIMITED' as beneficiaryifsc,  '' as ben_account,  '' as ben_name,  '912020022744584' as sender_information,  to_char(a.send_transid) as email,  '' as emailbody,  '' as debaccno,  '' as sen_acc_type,  c.beneficiary_account as rec_acc_type  from neft_master a, neft_customer c  where a.branch_id = c.branch_id  and a.cust_id = c.cust_id  and c.verify_status = 'T'  and a.send_transid is not null  and to_date(a.send_date) is not null  and a.status_id = 0  and a.typestatus = 'DC'  and a.group_id = " & grpid & "  and a.module_id = 49  and a.firm_id <> 24  order by paymentidentifier, corporationid")
        ElseIf st = 48 Then 'Night Petrolling NT
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  to_char(a.send_transid) as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  ltrim(c.cust_name, 102) as ben_name,  to_char(a.send_transid) as sender_information,  '' as email,  '' as emailbody,  e.parmtr_value as debaccno,  '11' as sen_acc_type,  c.acc_type as rec_acc_type  from neft_master a, neft_customer c, firm_master d, general_parameter e  where a.cust_id = c.cust_id  and a.firm_id = d.firm_id  and e.parmtr_id = 57  and e.module_id = 1  and e.firm_id = a.firm_id  and c.verify_status = 'T'  and a.send_transid is not null  and a.status_id = 0  and a.typestatus <> 'DC'  and a.module_id = 49  and a.firm_id <> 24  and a.group_id = " & grpid & "  order by paymentidentifier, corporationid")
        ElseIf st = 49 Then 'PART TIME HK DC
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  to_char(a.send_transid) as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  c.beneficiary_account as senderifsc,  'MANAPPURAM FINANCE  LIMITED' as beneficiaryifsc,  '' as ben_account,  '' as ben_name,  '912020022744584' as sender_information,  to_char(a.send_transid) as email,  '' as emailbody,  '' as debaccno,  '' as sen_acc_type,  c.beneficiary_account as rec_acc_type  from neft_master a, neft_customer c  where a.branch_id = c.branch_id  and a.cust_id = c.cust_id  and c.verify_status = 'T'  and a.send_transid is not null  and to_date(a.send_date) is not null  and a.status_id = 0  and a.typestatus = 'DC'  and a.group_id = " & grpid & "  and a.module_id = 48  and a.firm_id <> 24  order by paymentidentifier, corporationid")
        ElseIf st = 50 Then 'PART TIME HK NT
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  to_char(a.send_transid) as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  ltrim(c.cust_name, 102) as ben_name,  to_char(a.send_transid) as sender_information,  '' as email,  '' as emailbody,  e.parmtr_value as debaccno,  '11' as sen_acc_type,  c.acc_type as rec_acc_type  from neft_master a, neft_customer c, firm_master d, general_parameter e  where a.cust_id = c.cust_id  and a.firm_id = d.firm_id  and e.parmtr_id = 57  and e.module_id = 1  and e.firm_id = a.firm_id  and c.verify_status = 'T'  and a.send_transid is not null  and a.status_id = 0  and a.typestatus <> 'DC'  and a.module_id = 48  and a.firm_id <> 24  and a.group_id = " & grpid & "  order by paymentidentifier, corporationid")
        ElseIf st = 51 Then 'Mortgage loan DC
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  to_char(a.send_transid) as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  c.beneficiary_account as senderifsc,  'MANAPPURAM FINANCE  LIMITED' as beneficiaryifsc,  '' as ben_account,  '' as ben_name,  '018005004750' as sender_information,  to_char(a.send_transid) as email,  '' as emailbody,  '' as debaccno,  '' as sen_acc_type,  c.beneficiary_account as rec_acc_type  from neft_master a, neft_customer c  where   a.cust_id = c.cust_id  and c.verify_status = 'T'  and a.send_transid is not null  and to_date(a.send_date) is not null  and a.status_id = 0  and a.typestatus = 'DC'  and a.group_id = " & grpid & "  and a.module_id = 45    order by paymentidentifier, corporationid")
        ElseIf st = 52 Then 'Mortgage loan NT
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  to_char(a.send_transid) as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'ICIC0000180' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  ltrim(c.cust_name, 102) as ben_name,  to_char(a.send_transid) as sender_information,  '' as email,  '' as emailbody,  e.parmtr_value as debaccno,  '11' as sen_acc_type,  c.acc_type as rec_acc_type  from neft_master a, neft_customer c, firm_master d, general_parameter e  where a.cust_id = c.cust_id  and a.firm_id = d.firm_id  and e.parmtr_id = 57  and e.module_id = 45  and e.firm_id = a.firm_id  and c.verify_status = 'T'  and a.send_transid is not null  and a.status_id = 0  and a.typestatus <> 'DC'  and a.module_id = 45  and a.firm_id <> 24  and a.group_id = " & grpid & "  order by paymentidentifier, corporationid")
        ElseIf st = 53 Then 'PANCARD DC
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  to_char(a.send_transid) as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  c.beneficiary_account as senderifsc,  'MANAPPURAM FINANCE  LIMITED' as beneficiaryifsc,  '' as ben_account,  '' as ben_name,  '910020002134864' as sender_information,  to_char(a.send_transid) as email,  '' as emailbody,  '' as debaccno,  '' as sen_acc_type,  c.beneficiary_account as rec_acc_type  from neft_master a, neft_customer c  where   a.cust_id = c.cust_id  and c.verify_status = 'T'  and a.send_transid is not null  and to_date(a.send_date) is not null  and a.status_id = 0  and a.typestatus = 'DC'  and a.group_id = " & grpid & "  and a.module_id = 52    order by paymentidentifier, corporationid")
        ElseIf st = 54 Then 'PANCARD NT
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  to_char(a.send_transid) as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  ltrim(c.cust_name, 102) as ben_name,  to_char(a.send_transid) as sender_information,  '' as email,  '' as emailbody,  e.parmtr_value as debaccno,  '11' as sen_acc_type,  c.acc_type as rec_acc_type  from neft_master a, neft_customer c, firm_master d, general_parameter e  where a.cust_id = c.cust_id  and a.firm_id = d.firm_id  and e.parmtr_id = 57  and e.module_id = 5  and e.firm_id = a.firm_id  and c.verify_status = 'T'  and a.send_transid is not null  and a.status_id = 0  and a.typestatus <> 'DC'  and a.module_id = 52  and a.firm_id =1  and a.group_id = " & grpid & "  order by paymentidentifier, corporationid")

        Else
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,to_char(a.send_transid) as corporationid,a.amount,to_char(to_date(a.send_date),'dd/mm/yyyy') as valuedt,c.beneficiary_account as senderifsc,'MANAPPURAM FINANCE  LIMITED' as beneficiaryifsc,'' as ben_account,'' as ben_name,'910020002134864' as sender_information,to_char(a.send_transid) as email,'' as emailbody,'' as debaccno,'' as sen_acc_type,c.beneficiary_account as rec_acc_type from neft_master a, neft_customer c where a.branch_id = c.branch_id and a.firm_id = c.firm_id and a.cust_id = c.cust_id and c.verify_status = 'T' and a.send_transid is not null and to_date(a.send_date) is not null and a.status_id = 0 and a.typestatus = 'DC' and a.group_id = " & grpid & " and a.module_id =44 order by paymentidentifier,corporationid ")
        End If
        Return ds
    End Function


    <WebMethod()> _
    Public Function NEFTDATApaythru(ByVal st As Integer) As DataSet
        Dim ds As DataSet

        If st = 2 Then
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,a.corporate_id as corporationid,a.amount,to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,'UTIB0000046' as senderifsc,c.ifsc_code beneficiaryifsc,c.beneficiary_account as ben_account,c.cust_name as ben_name,decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information,'' as email,'' as emailbody,e.parmtr_value as debaccno,'11' as sen_acc_type,c.acc_type as rec_acc_type from neft_master a, neft_customer c, general_parameter e where a.firm_id=1 and a.cust_id = C.cust_id and e.parmtr_id = 58 and e.module_id=1 and e.firm_id=a.firm_id and c.verify_status = 'T' and a.send_date is null and a.status_id = 1 and a.typestatus <> 'DC' and a.module_id in (2,5,6,20,88) order by paymentidentifier")
        ElseIf st = 3 Then
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,a.corporate_id as corporationid,a.amount,to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,'UTIB0000046' as senderifsc,c.ifsc_code beneficiaryifsc,c.beneficiary_account as ben_account,c.cust_name as ben_name,decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information,'' as email,'' as emailbody,e.parmtr_value as debaccno,'11' as sen_acc_type,c.acc_type as rec_acc_type from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id and a.branch_id = C.branch_id and a.cust_id = C.cust_id and e.parmtr_id = 56 and e.module_id=1 and e.firm_id=a.firm_id and c.verify_status = 'T' and a.send_date is null and a.status_id = 1 and a.typestatus <> 'DC' and a.module_id=44 order by paymentidentifier")
        ElseIf st = 4 Then
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,a.corporate_id as corporationid,a.amount,to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,c.beneficiary_account as senderifsc,'MANAPPURAM FINANCE  LIMITED' as beneficiaryifsc,'' as ben_account,'' as ben_name,'912020067992955' as sender_information,'TRANSFER FOR PAYTHRU' as email,'' as emailbody,'' as debaccno,'' as sen_acc_type, null as rec_acc_type from neft_master a, neft_customer c where a.firm_id=1 and a.cust_id = c.cust_id and c.verify_status = 'T' and a.send_date is null and a.status_id = 1 and a.typestatus = 'DC' and a.module_id in (2,5,6,20,88) /*not in (44,90,91,33)*/  order by paymentidentifier")
        ElseIf st = 5 Then 'Salary _ _ Customer DC
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  c.cust_name as ben_name, decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email, '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type  from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id  and a.branch_id = C.branch_id  and a.cust_id = C.cust_id  and e.parmtr_id = 57  and e.module_id = 1  and e.firm_id = a.firm_id  and e.firm_id<>24 and c.verify_status = 'T'  and to_date(a.send_date) is null  and a.status_id = 1  and a.typestatus = 'DC'  and a.module_id =90    and c.moduleid=90  and a.module_id=c.moduleid order by paymentidentifier")
        ElseIf st = 6 Then 'Salary _  _ Customer' JWELL DC
            'ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  c.cust_name as ben_name, decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email, '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type  from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id  and a.branch_id = C.branch_id  and a.cust_id = C.cust_id  and e.parmtr_id = 56  and e.module_id = 1  and e.firm_id = a.firm_id  and e.firm_id=24 and c.verify_status = 'T'  and to_date(a.send_date) is null  and a.status_id = 1  and a.typestatus = 'DC'  and a.module_id =90    and c.moduleid=90  and a.module_id=c.moduleid order by paymentidentifier")
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  c.cust_name as ben_name, decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email, '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type  from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id and a.Doc_Id=C.CUST_REF_ID and a.branch_id = C.branch_id and e.parmtr_id = 56  and e.module_id = 1  and e.firm_id = a.firm_id  and e.firm_id=1 and c.verify_status = 'T'  and to_date(a.send_date) is null  and a.status_id = 1  and a.typestatus = 'DC'  and a.module_id =53  and c.moduleid=90 and rownum<1000 order by paymentidentifier")
        ElseIf st = 7 Then 'Salary _ SEC SAL
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  c.cust_name as ben_name, decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email, '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type  from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id  and a.branch_id = C.branch_id  and a.cust_id = C.cust_id  and e.parmtr_id = 57  and e.module_id = 1  and e.firm_id = a.firm_id  and c.verify_status = 'T'  and to_date(a.send_date) is null  and a.status_id = 1  and a.typestatus = 'DC'  and a.module_id =33    and c.moduleid=33  and a.module_id=c.moduleid order by paymentidentifier")
        ElseIf st = 8 Then 'Salary _ PT
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  c.cust_name as ben_name, decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email, '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type  from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id  and a.branch_id = C.branch_id  and a.cust_id = C.cust_id  and e.parmtr_id = 57  and e.module_id = 1  and e.firm_id = a.firm_id  and e.firm_id<>24 and c.verify_status = 'T'  and to_date(a.send_date) is null  and a.status_id = 1  and a.typestatus = 'DC'  and a.module_id =91    and c.moduleid=91  and a.module_id=c.moduleid order by paymentidentifier")
        ElseIf st = 9 Then 'Salary _ Neft _ Customer
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  c.cust_name as ben_name, decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email, '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type  from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id  and a.branch_id = C.branch_id  and a.cust_id = C.cust_id  and e.parmtr_id = 57  and e.module_id = 1  and e.firm_id = a.firm_id  and e.firm_id<>24 and c.verify_status = 'T'  and to_date(a.send_date) is null  and a.status_id = 1  and a.typestatus <> 'DC'  and a.module_id =90    and c.moduleid=90  and a.module_id=c.moduleid order by paymentidentifier")
        ElseIf st = 10 Then 'Salary _ Neft _ Customer' JWELL
            'ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  c.cust_name as ben_name, decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email, '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type  from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id  and a.branch_id = C.branch_id  and a.cust_id = C.cust_id  and e.parmtr_id = 56  and e.module_id = 1  and e.firm_id = a.firm_id  and e.firm_id=24 and c.verify_status = 'T'  and to_date(a.send_date) is null  and a.status_id = 1  and a.typestatus <> 'DC'  and a.module_id =90    and c.moduleid=90  and a.module_id=c.moduleid order by paymentidentifier")
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  c.cust_name as ben_name, decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email, '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type  from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id and a.Doc_Id=C.CUST_REF_ID and a.branch_id = C.branch_id and e.parmtr_id = 56  and e.module_id = 1  and e.firm_id = a.firm_id  and e.firm_id=1 and c.verify_status = 'T'  and to_date(a.send_date) is null  and a.status_id = 1  and a.typestatus <> 'DC'  and a.module_id =53 and c.moduleid=90 and rownum<1000 order by paymentidentifier")
        ElseIf st = 11 Then 'Salary _ sec
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  c.cust_name as ben_name, decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email, '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type  from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id  and a.branch_id = C.branch_id  and a.cust_id = C.cust_id  and e.parmtr_id = 57  and e.module_id = 1  and e.firm_id = a.firm_id  and c.verify_status = 'T'  and to_date(a.send_date) is null  and a.status_id = 1  and a.typestatus <> 'DC'  and a.module_id =33    and c.moduleid=33  and a.module_id=c.moduleid order by paymentidentifier")
        ElseIf st = 12 Then 'Salary _ pt
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  c.cust_name as ben_name, decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email, '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type  from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id  and a.branch_id = C.branch_id  and a.cust_id = C.cust_id  and e.parmtr_id = 57  and e.module_id = 1  and e.firm_id = a.firm_id  and e.firm_id<>24 and c.verify_status = 'T'  and to_date(a.send_date) is null  and a.status_id = 1  and a.typestatus <> 'DC'  and a.module_id =91    and c.moduleid=91  and a.module_id=c.moduleid order by paymentidentifier")

            '---------------------------------------------------------------------------- ----------------------------------------------------------------------------
        ElseIf st = 13 Then 'TDS paythru
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  c.cust_name as ben_name, decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email, '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type  from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id  and a.branch_id = C.branch_id  and a.cust_id = C.cust_id  and e.parmtr_id = 56  and e.module_id = 1  and e.firm_id = a.firm_id  and e.firm_id<>24 and c.verify_status = 'T'  and to_date(a.send_date) is null  and a.status_id = 1  and a.typestatus = 'DC'   and a.module_id in (64,65) and c.moduleid=64 order by paymentidentifier")
        ElseIf st = 14 Then 'TDS neft
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  c.cust_name as ben_name, decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email, '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type  from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id  and a.branch_id = C.branch_id  and a.cust_id = C.cust_id  and e.parmtr_id = 56  and e.module_id = 1  and e.firm_id = a.firm_id  and e.firm_id<>24 and c.verify_status = 'T'  and to_date(a.send_date) is null  and a.status_id = 1  and a.typestatus <> 'DC'  and a.module_id in (64,65) and c.moduleid=64 order by paymentidentifier")
        ElseIf st = 15 Then 'Money transfer Pay thru
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  c.cust_name as ben_name,  decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email,  '' as emailbody,  e.parmtr_value as debaccno,  '11' as sen_acc_type,  c.acc_type as rec_acc_type  from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id and a.cust_id = C.cust_id  and e.parmtr_id = 56  and e.module_id = 1  and e.firm_id = a.firm_id  and e.firm_id =1 and to_date(a.send_date) is null  and a.status_id = 1  and a.typestatus = 'DC'  and a.module_id = 15  order by paymentidentifier")
        ElseIf st = 16 Then 'Money transfer neft
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  c.cust_name as ben_name,  decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information,  '' as email,  '' as emailbody,  e.parmtr_value as debaccno,  '11' as sen_acc_type,  c.acc_type as rec_acc_type  from neft_master a, neft_customer c, general_parameter e  where a.cust_id = c.cust_id  and e.parmtr_id = 56  and e.module_id = 1  and e.firm_id = a.firm_id and e.firm_id =1 and to_date(a.send_date) is null  and a.status_id = 1  and a.typestatus <> 'DC'  and a.module_id = 15  order by paymentidentifier")
            '---------------------------------------------------------------------------- ----------------------------------------------------------------------------
        ElseIf st = 17 Then 'RENT PAY THRU
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  c.cust_name as ben_name, decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email, '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type  from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id  and a.branch_id = C.branch_id  and a.cust_id = C.cust_id  and e.parmtr_id = 56  and e.module_id = 1  and e.firm_id = a.firm_id  and c.verify_status = 'T'  and to_date(a.send_date) is null  and a.status_id = 1  and a.typestatus = 'DC'   and a.module_id =34    and c.moduleid=34  and a.module_id=c.moduleid order by paymentidentifier")
        ElseIf st = 18 Then 'RENT NEFT/RTGS
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  c.cust_name as ben_name, decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email, '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type  from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id  and a.branch_id = C.branch_id  and a.cust_id = C.cust_id  and e.parmtr_id = 56  and e.module_id = 1  and e.firm_id = a.firm_id  and c.verify_status = 'T'  and to_date(a.send_date) is null  and a.status_id = 1  and a.typestatus <> 'DC'  and a.module_id =34   and c.moduleid=34  and a.module_id=c.moduleid order by paymentidentifier")

        ElseIf st = 19 Then 'Store PAY THRU
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  c.cust_name as ben_name, decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email, '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type  from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id  and a.branch_id = C.branch_id  and a.cust_id = C.cust_id  and e.parmtr_id = 56  and e.module_id = 1  and e.firm_id = a.firm_id  and c.verify_status = 'T'  and to_date(a.send_date) is null  and a.status_id = 1  and a.typestatus = 'DC'   and a.module_id =39    and c.moduleid=39  and a.module_id=c.moduleid order by paymentidentifier")
        ElseIf st = 20 Then 'Store NEFT/RTGS
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  c.cust_name as ben_name, decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email, '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type  from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id  and a.branch_id = C.branch_id  and a.cust_id = C.cust_id  and e.parmtr_id = 56  and e.module_id = 1  and e.firm_id = a.firm_id  and c.verify_status = 'T'  and to_date(a.send_date) is null  and a.status_id = 1  and a.typestatus <> 'DC'  and a.module_id =39   and c.moduleid=39  and a.module_id=c.moduleid order by paymentidentifier")

        ElseIf st = 21 Then 'ADVERTISEMENT PAY THRU
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  c.cust_name as ben_name, decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email, '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type  from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id  and a.branch_id = C.branch_id  and a.cust_id = C.cust_id  and e.parmtr_id = 56  and e.module_id = 1  and e.firm_id = a.firm_id  and c.verify_status = 'T'  and to_date(a.send_date) is null  and a.status_id = 1  and a.typestatus = 'DC'   and a.module_id =94    and c.moduleid=94  and a.module_id=c.moduleid order by paymentidentifier")
        ElseIf st = 22 Then 'ADVERTISEMENT NEFT/RTGS
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  c.cust_name as ben_name, decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email, '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type  from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id  and a.branch_id = C.branch_id  and a.cust_id = C.cust_id  and e.parmtr_id = 56  and e.module_id = 1  and e.firm_id = a.firm_id  and c.verify_status = 'T'  and to_date(a.send_date) is null  and a.status_id = 1  and a.typestatus <> 'DC'  and a.module_id =94   and c.moduleid=94  and a.module_id=c.moduleid order by paymentidentifier")

        ElseIf st = 23 Then 'AUCTION SURPLUS PAYMENT  GOLD LOAN PAY THRU
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,c.ifsc_code beneficiaryifsc, c.beneficiary_account as ben_account, c.cust_name as ben_name,decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email, '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type  from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id  and e.parmtr_id = 56   and e.module_id = 1   and e.firm_id = a.firm_id    and to_date(a.send_date) is null   and a.status_id = 1   and a.typestatus  = 'DC'  and a.module_id = 7  order by paymentidentifier")
        ElseIf st = 24 Then 'AUCTION SURPLUS PAYMENT GOLD LOAN NEFT/RTGS
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,c.ifsc_code beneficiaryifsc, c.beneficiary_account as ben_account, c.cust_name as ben_name,decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email, '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type  from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id  and e.parmtr_id = 56   and e.module_id = 1   and e.firm_id = a.firm_id   and to_date(a.send_date) is null   and a.status_id = 1   and a.typestatus <> 'DC'  and a.module_id = 7  order by paymentidentifier")

        ElseIf st = 25 Then 'hp PAYMENT  GOLD LOAN PAY THRU
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,c.ifsc_code beneficiaryifsc, c.beneficiary_account as ben_account, c.cust_name as ben_name,decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email, '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type  from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id  and e.parmtr_id = 56   and e.module_id = 1   and e.firm_id = a.firm_id   and c.verify_status = 'T'   and to_date(a.send_date) is null   and a.status_id = 1   and a.typestatus  = 'DC'  and a.module_id = 150  order by paymentidentifier")
        ElseIf st = 26 Then 'HP PAYMENT GOLD LOAN NEFT/RTGS
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,c.ifsc_code beneficiaryifsc, c.beneficiary_account as ben_account, c.cust_name as ben_name,decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email, '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type  from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id  and e.parmtr_id = 56   and e.module_id = 1   and e.firm_id = a.firm_id   and c.verify_status = 'T'   and to_date(a.send_date) is null   and a.status_id = 1   and a.typestatus <> 'DC'  and a.module_id = 150  order by paymentidentifier")
            'Added 08-feb-2012
        ElseIf st = 27 Then 'DEPOSIT MAAFIN PAYMENT PAYTHRU
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,a.corporate_id as corporationid,a.amount,to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,c.beneficiary_account as senderifsc, d.firm_name as beneficiaryifsc,'' as ben_account,'' as ben_name,e.parmtr_value as sender_information,'TRANSFER FOR PAYTHRU' as email,'' as emailbody,'' as debaccno,'' as sen_acc_type, null as rec_acc_type from neft_master a, neft_customer c,firm_master d,general_parameter e where e.firm_id=a.firm_id and e.parmtr_id=56 and e.module_id=1 and d.firm_id=a.firm_id and a.firm_id=4  and a.cust_id = c.cust_id and c.verify_status = 'T' and to_date(a.send_date) is null and a.status_id = 1 and a.typestatus = 'DC' and a.module_id in (2,5,6,20,88) order by paymentidentifier")
        ElseIf st = 28 Then 'DEPOSIT MAAFIN PAYMENT NEFT/RTGS
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,a.corporate_id as corporationid,a.amount,to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,'UTIB0000046' as senderifsc,c.ifsc_code beneficiaryifsc,c.beneficiary_account as ben_account,c.cust_name as ben_name,decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information,'' as email,'' as emailbody,e.parmtr_value as debaccno,'11' as sen_acc_type,c.acc_type as rec_acc_type from neft_master a, neft_customer c, general_parameter e where a.firm_id =4 and a.cust_id = C.cust_id and e.parmtr_id = 56 and e.module_id=1 and e.firm_id=a.firm_id and c.verify_status = 'T' and to_date(a.send_date) is null and a.status_id = 1 and a.typestatus <> 'DC' and a.module_id in (2,5,6,20,88) order by paymentidentifier")
        ElseIf st = 29 Then 'DEPOSIT MAGRO PAYMENT PAYTHRU
            'ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,a.corporate_id as corporationid,a.amount,to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,c.beneficiary_account as senderifsc, d.firm_name as beneficiaryifsc,'' as ben_account,'' as ben_name,'910020002134864' as sender_information,'TRANSFER FOR PAYTHRU' as email,'' as emailbody,'' as debaccno,'' as sen_acc_type, null as rec_acc_type from neft_master a, neft_customer c,firm_master d where d.firm_id=a.firm_id and a.firm_id=5  and a.cust_id = c.cust_id and c.verify_status = 'T' and to_date(a.send_date) is null and a.status_id = 1 and a.typestatus = 'DC' and a.module_id in (2,5,6,20,88) /*not in (44,90,91,33)*/  order by paymentidentifier")
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,a.corporate_id as corporationid,a.amount,to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,c.beneficiary_account as senderifsc, d.firm_name as beneficiaryifsc,'' as ben_account,'' as ben_name,e.parmtr_value as sender_information,'TRANSFER FOR PAYTHRU' as email,'' as emailbody,'' as debaccno,'' as sen_acc_type, null as rec_acc_type from neft_master a, neft_customer c,firm_master d,general_parameter e where e.firm_id=a.firm_id and e.parmtr_id=56 and e.module_id=1 and d.firm_id=a.firm_id and a.firm_id=5 and a.cust_id = c.cust_id and c.verify_status = 'T' and to_date(a.send_date) is null and a.status_id = 1 and a.typestatus = 'DC' and a.module_id in (2,5,6,20,88) order by paymentidentifier")
        ElseIf st = 30 Then 'DEPOSIT MAGRO PAYMENT NEFT/RTGS
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,a.corporate_id as corporationid,a.amount,to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,'UTIB0000046' as senderifsc,c.ifsc_code beneficiaryifsc,c.beneficiary_account as ben_account,c.cust_name as ben_name,decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information,'' as email,'' as emailbody,e.parmtr_value as debaccno,'11' as sen_acc_type,c.acc_type as rec_acc_type from neft_master a, neft_customer c, general_parameter e where a.firm_id =5 and a.cust_id = C.cust_id and e.parmtr_id = 56 and e.module_id=1 and e.firm_id=a.firm_id and c.verify_status = 'T' and to_date(a.send_date) is null and a.status_id = 1 and a.typestatus <> 'DC' and a.module_id in (2,5,6,20,88) order by paymentidentifier")
            'Added 08-feb-2012
        ElseIf st = 31 Then 'DEPOSIT MABEN PAYMENT PAYTHRU -Added on April 16 2012
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,a.corporate_id as corporationid,a.amount,to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,c.beneficiary_account as senderifsc, d.firm_name as beneficiaryifsc,'' as ben_account,'' as ben_name,e.parmtr_value as sender_information,'TRANSFER FOR PAYTHRU' as email,'' as emailbody,'' as debaccno,'' as sen_acc_type, null as rec_acc_type from neft_master a, neft_customer c,firm_master d,general_parameter e where e.firm_id=a.firm_id and e.parmtr_id=56 and e.module_id=1 and d.firm_id=a.firm_id and a.firm_id=2  and a.cust_id = c.cust_id and c.verify_status = 'T' and to_date(a.send_date) is null and a.status_id = 1 and a.typestatus = 'DC' and a.module_id in (2,5,6,20,88) order by paymentidentifier")
        ElseIf st = 32 Then 'DEPOSIT MABEN PAYMENT NEFT/RTGS -Added on April 16 2012
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,a.corporate_id as corporationid,a.amount,to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,'UTIB0000046' as senderifsc,c.ifsc_code beneficiaryifsc,c.beneficiary_account as ben_account,c.cust_name as ben_name,decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information,'' as email,'' as emailbody,e.parmtr_value as debaccno,'11' as sen_acc_type,c.acc_type as rec_acc_type from neft_master a, neft_customer c, general_parameter e where a.firm_id =2 and a.cust_id = C.cust_id and e.parmtr_id = 56 and e.module_id=1 and e.firm_id=a.firm_id and c.verify_status = 'T' and to_date(a.send_date) is null and a.status_id = 1 and a.typestatus <> 'DC' and a.module_id in (2,5,6,20,88) order by paymentidentifier")
        ElseIf st = 33 Then 'TRUSTEE ACC PAYTHRU -Added on JUL 10 2012
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,a.corporate_id as corporationid,a.amount,to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,c.beneficiary_account as senderifsc, d.firm_name as beneficiaryifsc,'' as ben_account,'' as ben_name,e.parmtr_value as sender_information,'TRANSFER FOR PAYTHRU' as email,'' as emailbody,'' as debaccno,'' as sen_acc_type, null as rec_acc_type from neft_master a, neft_customer c,firm_master d,general_parameter e where e.firm_id=a.firm_id and e.parmtr_id=56 and e.module_id=92 and d.firm_id=a.firm_id and a.firm_id=1  and a.cust_id = c.cust_id and c.verify_status = 'T' and to_date(a.send_date) is null and a.status_id = 1 and a.typestatus = 'DC' and a.module_id=92 order by paymentidentifier")
        ElseIf st = 34 Then  'TRUSTEE ACC PAYTHRU -Added on JUL 10 2012
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,a.corporate_id as corporationid,a.amount,to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,'DLXB0000001' as senderifsc,c.ifsc_code beneficiaryifsc,c.beneficiary_account as ben_account,c.cust_name as ben_name,decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information,'' as email,'' as emailbody,e.parmtr_value as debaccno,'11' as sen_acc_type,c.acc_type as rec_acc_type from neft_master a, neft_customer c, general_parameter e where a.firm_id =1 and a.cust_id = C.cust_id and e.parmtr_id = 56 and e.module_id=92 and e.firm_id=a.firm_id and c.verify_status = 'T' and to_date(a.send_date) is null and a.status_id = 1 and a.typestatus <> 'DC' and a.module_id=92 order by paymentidentifier")
        ElseIf st = 35 Then 'Employee Final Settlement Pay thru
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  c.cust_name as ben_name, decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email, '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type  from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id  and a.branch_id = C.branch_id  and a.cust_id = C.cust_id  and e.parmtr_id = 57  and e.module_id = 1  and e.firm_id = a.firm_id  and c.verify_status = 'T'  and to_date(a.send_date) is null  and a.status_id = 1  and a.typestatus = 'DC' and a.module_id =98 and c.moduleid=98 and a.module_id=c.moduleid order by paymentidentifier")
        ElseIf st = 36 Then 'Employee Final Settlement neft
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  c.cust_name as ben_name, decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email, '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type  from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id  and a.branch_id = C.branch_id  and a.cust_id = C.cust_id  and e.parmtr_id = 57  and e.module_id = 1  and e.firm_id = a.firm_id  and c.verify_status = 'T'  and to_date(a.send_date) is null  and a.status_id = 1  and a.typestatus <> 'DC' and a.module_id =98 and c.moduleid=98 and a.module_id=c.moduleid order by paymentidentifier")
        ElseIf st = 37 Then 'BAS Pay thru
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  c.cust_name as ben_name, decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email, '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type  from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id   and a.cust_id = C.cust_id  and e.parmtr_id = 56  and e.module_id = 1  and e.firm_id = a.firm_id  and c.verify_status = 'T'  and to_date(a.send_date) is null  and a.status_id = 1  and a.typestatus = 'DC' and a.module_id =55 and c.moduleid=55 and a.module_id=c.moduleid order by paymentidentifier")
        ElseIf st = 38 Then 'BAS neft
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  c.cust_name as ben_name, decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email, '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type  from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id   and a.cust_id = C.cust_id  and e.parmtr_id = 56  and e.module_id = 1  and e.firm_id = a.firm_id  and c.verify_status = 'T'  and to_date(a.send_date) is null  and a.status_id = 1  and a.typestatus <> 'DC' and a.module_id =55 and c.moduleid=55 and a.module_id=c.moduleid order by paymentidentifier")
        ElseIf st = 39 Then 'PNL Pay thru
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,a.corporate_id as corporationid,a.amount,to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,c.beneficiary_account as senderifsc,'MANAPPURAM FINANCE  LIMITED' as beneficiaryifsc,'' as ben_account,'' as ben_name,'910020002134864' as sender_information,'TRANSFER FOR PAYTHRU' as email,'' as emailbody,'' as debaccno,'' as sen_acc_type, null as rec_acc_type from neft_master a, neft_customer c where a.firm_id not in (4,5,2) and a.cust_id = c.cust_id and c.verify_status = 'T' and to_date(a.send_date) is null and a.status_id = 1 and a.typestatus = 'DC' and a.module_id =10  order by paymentidentifier")
        ElseIf st = 40 Then 'PNL neft
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,a.corporate_id as corporationid,a.amount,to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,'UTIB0000046' as senderifsc,c.ifsc_code beneficiaryifsc,c.beneficiary_account as ben_account,c.cust_name as ben_name,decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information,'' as email,'' as emailbody,e.parmtr_value as debaccno,'11' as sen_acc_type,c.acc_type as rec_acc_type from neft_master a, neft_customer c, general_parameter e where a.firm_id not in (4,5,2) and a.cust_id = C.cust_id and e.parmtr_id = 56 and e.module_id=1 and e.firm_id=a.firm_id and c.verify_status = 'T' and to_date(a.send_date) is null and a.status_id = 1 and a.typestatus <> 'DC' and a.module_id =10 order by paymentidentifier")
        ElseIf st = 41 Then 'GoldLoan Pay thru
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  c.cust_name as ben_name, decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email, '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type  from neft_master a, neft_customer c, general_parameter e where a.cust_id = C.cust_id  and e.parmtr_id = 59  and e.module_id = 1  and e.firm_id = a.firm_id  and e.firm_id<>24 and to_date(a.send_date) is null  and a.status_id = 1  and a.typestatus = 'DC'   and a.module_id =113  and a.corporate_id is null  order by paymentidentifier")
        ElseIf st = 42 Then 'GoldLoan neft
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  c.cust_name as ben_name, decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email, '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type  from neft_master a, neft_customer c, general_parameter e where a.cust_id = C.cust_id  and e.parmtr_id = 59  and e.module_id = 1  and e.firm_id = a.firm_id  and e.firm_id<>24 and to_date(a.send_date) is null  and a.status_id = 1  and a.typestatus <> 'DC'  and a.module_id =113  and a.corporate_id is null  order by paymentidentifier")
        ElseIf st = 43 Then 'Auctioneer Pay thru
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier, a.corporate_id as corporationid, a.amount, to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt, 'UTIB0000046' as senderifsc, c.ifsc_code beneficiaryifsc, c.beneficiary_account as ben_account, c.cust_name as ben_name, decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email, '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id and a.branch_id = c.branch_id and to_date(a.send_date) is null and a.status_id = 1 and a.typestatus = 'DC' and a.module_id = 8 and e.parmtr_id = 94 and e.module_id = 7 and e.firm_id = 1 order by paymentidentifier")
        ElseIf st = 44 Then 'Auctioneer Neft
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier, a.corporate_id as corporationid, a.amount, to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt, 'UTIB0000046' as senderifsc, c.ifsc_code beneficiaryifsc, c.beneficiary_account as ben_account, c.cust_name as ben_name, decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email, '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id and a.branch_id = c.branch_id and to_date(a.send_date) is null and a.status_id = 1 and a.typestatus <> 'DC' and a.module_id = 8 and e.parmtr_id = 94 and e.module_id = 7 and e.firm_id = 1 order by paymentidentifier")
        ElseIf st = 45 Then 'CDL Pay thru
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,a.corporate_id as corporationid,a.amount,to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,c.beneficiary_account as senderifsc,'MANAPPURAM FINANCE  LIMITED' as beneficiaryifsc,'' as ben_account,'' as ben_name,'910020002134864' as sender_information,'TRANSFER FOR PAYTHRU' as email,'' as emailbody,'' as debaccno,'' as sen_acc_type, null as rec_acc_type from neft_master a, neft_customer c where a.firm_id=1 and a.cust_id = c.cust_id and c.verify_status = 'T' and to_date(a.send_date) is null and a.status_id = 1 and a.typestatus = 'DC' and a.module_id =47  order by paymentidentifier")
        ElseIf st = 46 Then 'CDL neft
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,a.corporate_id as corporationid,a.amount,to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,'UTIB0000046' as senderifsc,c.ifsc_code beneficiaryifsc,c.beneficiary_account as ben_account,c.cust_name as ben_name,decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information,'' as email,'' as emailbody,e.parmtr_value as debaccno,'11' as sen_acc_type,c.acc_type as rec_acc_type from neft_master a, neft_customer c, general_parameter e where a.firm_id=1 and a.cust_id = C.cust_id and e.parmtr_id = 56 and e.module_id=1 and e.firm_id=a.firm_id and c.verify_status = 'T' and to_date(a.send_date) is null and a.status_id = 1 and a.typestatus <> 'DC' and a.module_id =47 order by paymentidentifier")
        ElseIf st = 47 Then 'Night Petrolling DC
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  c.cust_name as ben_name, decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email, '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type  from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id  and a.branch_id = C.branch_id  and a.cust_id = C.cust_id  and e.parmtr_id = 57  and e.module_id = 1  and e.firm_id = a.firm_id  and c.verify_status = 'T'  and to_date(a.send_date) is null  and a.status_id = 1  and a.typestatus = 'DC'  and a.module_id =49    and c.moduleid=49  and a.module_id=c.moduleid order by paymentidentifier")
        ElseIf st = 48 Then 'Night Petrolling NT
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  c.cust_name as ben_name, decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email, '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type  from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id  and a.branch_id = C.branch_id  and a.cust_id = C.cust_id  and e.parmtr_id = 57  and e.module_id = 1  and e.firm_id = a.firm_id  and c.verify_status = 'T'  and to_date(a.send_date) is null  and a.status_id = 1  and a.typestatus <> 'DC'  and a.module_id =49    and c.moduleid=49  and a.module_id=c.moduleid order by paymentidentifier")
        ElseIf st = 49 Then 'PART TIME HK DC
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  c.cust_name as ben_name, decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email, '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type  from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id  and a.branch_id = C.branch_id  and a.cust_id = C.cust_id  and e.parmtr_id = 57  and e.module_id = 1  and e.firm_id = a.firm_id  and c.verify_status = 'T'  and to_date(a.send_date) is null  and a.status_id = 1  and a.typestatus = 'DC'  and a.module_id =48    and c.moduleid=48  and a.module_id=c.moduleid order by paymentidentifier")
        ElseIf st = 50 Then 'PART TIME HK NT
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  c.cust_name as ben_name, decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email, '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type  from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id  and a.branch_id = C.branch_id  and a.cust_id = C.cust_id  and e.parmtr_id = 57  and e.module_id = 1  and e.firm_id = a.firm_id  and c.verify_status = 'T'  and to_date(a.send_date) is null  and a.status_id = 1  and a.typestatus <> 'DC'  and a.module_id =48    and c.moduleid=48  and a.module_id=c.moduleid order by paymentidentifier")
        ElseIf st = 51 Then 'Mortgage loan DC
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'ICIC0000180' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  c.cust_name as ben_name,  decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information,  '' as email,  '' as emailbody,  e.parmtr_value as debaccno,  '11' as sen_acc_type,  c.acc_type as rec_acc_type  from neft_master a, neft_customer c, general_parameter e  where a.cust_id = c.cust_id      and a.cust_id = C.cust_id  and e.parmtr_id = 57  and e.module_id = 45  and e.firm_id = a.firm_id       and c.verify_status = 'T'  and to_date(a.send_date) is null  and a.status_id = 1  and a.typestatus = 'DC'  and a.module_id = 45  order by paymentidentifier")
        ElseIf st = 52 Then 'Mortgage loan NT
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'ICIC0000180' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  c.cust_name as ben_name,  decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information,  '' as email,  '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id and e.parmtr_id = 57  and e.module_id = 45 and c.verify_status = 'T'  and to_date(a.send_date) is null  and a.status_id = 1  and a.typestatus <> 'DC'  and a.module_id = 45  order by paymentidentifier")
        ElseIf st = 53 Then 'PAN CARD DC
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  c.cust_name as ben_name,  decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information,  '' as email,  '' as emailbody,  e.parmtr_value as debaccno,  '11' as sen_acc_type,  c.acc_type as rec_acc_type  from neft_master a, neft_customer c, general_parameter e  where a.cust_id = c.cust_id      and a.cust_id = C.cust_id  and e.parmtr_id = 57  and e.module_id = 5  and e.firm_id = a.firm_id       and c.verify_status = 'T'  and to_date(a.send_date) is null  and a.status_id = 1  and a.typestatus = 'DC'  and a.module_id = 52  order by paymentidentifier")
        ElseIf st = 54 Then 'PAN CARD NT
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,  a.corporate_id as corporationid,  a.amount,  to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,  'UTIB0000046' as senderifsc,  c.ifsc_code beneficiaryifsc,  c.beneficiary_account as ben_account,  c.cust_name as ben_name,  decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information,  '' as email,  '' as emailbody, e.parmtr_value as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type from neft_master a, neft_customer c, general_parameter e where a.cust_id = c.cust_id and e.parmtr_id = 57  and e.module_id = 5 and c.verify_status = 'T'  and to_date(a.send_date) is null  and a.status_id = 1  and a.typestatus <> 'DC'  and a.module_id = 52  order by paymentidentifier")

        Else
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,a.corporate_id as corporationid,a.amount,to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt,c.beneficiary_account as senderifsc,'MANAPPURAM FINANCE  LIMITED' as beneficiaryifsc,'' as ben_account,'' as ben_name,'912020067992955' as sender_information,'TRANSFER FOR PAYTHRU' as email,'' as emailbody,'' as debaccno,'' as sen_acc_type, null as rec_acc_type from neft_master a, neft_customer c where a.branch_id = c.branch_id and a.firm_id = c.firm_id and a.cust_id = c.cust_id and c.verify_status = 'T' and to_date(a.send_date) is null and a.status_id = 1 and a.typestatus = 'DC' and a.module_id=44 order by paymentidentifier")
        End If
        Return ds

    End Function

    <WebMethod()> _
    Public Function paythruupdation(ByVal userid As String, ByVal catgory As Integer, ByVal bankid As Integer) As String
        Dim pr(5) As OracleParameter
        pr(0) = New OracleParameter("UserID", OracleType.VarChar, 15)
        pr(0).Value = userid
        pr(1) = New OracleParameter("category", OracleType.Number, 1)
        pr(1).Value = catgory
        pr(2) = New OracleParameter("GroupID", OracleType.Int16, 16)
        pr(2).Direction = ParameterDirection.Output
        pr(3) = New OracleParameter("TranID", OracleType.Number, 8)
        pr(3).Direction = ParameterDirection.Output
        pr(4) = New OracleParameter("ErrorStatus", OracleType.Number, 2)
        pr(4).Direction = ParameterDirection.Output
        pr(5) = New OracleParameter("bankid", OracleType.Number, 4)
        pr(5).Value = bankid
        oh.ExecuteNonQuery("paythruupdationDC", pr)
        Dim a As String
        a = pr(2).Value & "*" & pr(4).Value & "*" & pr(3).Value
        Return a
    End Function

    <WebMethod()> _
    Public Function cardcheck(ByVal cardid As String, ByVal branchId As Integer) As Integer
        Dim sql As String
        Dim result As Integer
        sql = "select count(*) cnt from customer_card_dtl where card_no='" & cardid & "'"
        Dim dt As New DataTable
        dt = oh.ExecuteDataSet(sql).Tables(0)
        If dt.Rows(0)(0) > 0 Then
            result = 0
        Else
            sql = "select count(*) cnt from store_item_dtl where item_id='0100600390' and  barcode='" & cardid & "' and out_location=" & branchId & " and location_id=2"
            dt = oh.ExecuteDataSet(sql).Tables(0)
            If dt.Rows(0)(0) > 0 Then
                result = 1
            Else
                result = 2
            End If
        End If
        Return result
    End Function

    <WebMethod()> _
    Public Function dep_printer() As String
        Dim dt, dt1 As New DataTable
        Dim sql As String = "select parmtr_value from deposit_printer where parmtr_id=9 and firm_id=7 and module_id=5 and catgry=1 and branch_id=9999"
        dt = oh.ExecuteDataSet(sql).Tables(0)
        Dim sql1 As String = "select parmtr_value from deposit_printer where parmtr_id=10 and firm_id=7 and module_id=5 and catgry=1 and branch_id=9999"
        dt1 = oh.ExecuteDataSet(sql1).Tables(0)
        Return dt1.Rows(0)(0) & dt.Rows(0)(0)
    End Function
    <WebMethod()> _
    Public Function voucherlist(ByVal trid) As DataSet
        Dim ds As DataSet
        ' ds = oh.ExecuteDataSet("select a.doc_id  as requestno,substr(c.branch_name, 0, 20) as branch,b.beneficiary_account as bankaccno,a.amount amount,to_char(a.value_date, 'dd/mm/yyyy') as tradt,to_char(a.send_date, 'dd/mm/yyyy') as senddt from neft_master a, neft_customer b, branch_master c where a.cust_id = b.cust_id and a.status_id = 0 and to_date(a.send_date) is not null and a.branch_id = b.branch_id and b.branch_id = c.branch_id and a.group_id = " & trid & "  group by a.doc_id,c.branch_name,b.beneficiary_account,a.amount,a.value_date,a.send_date,a.corporate_id order by a.corporate_id")
        ds = oh.ExecuteDataSet("select a.doc_id  as requestno,substr(c.branch_name, 0, 20) as branch,b.beneficiary_account as bankaccno,sum(a.amount) amount,to_char(a.value_date, 'dd/mm/yyyy') as tradt,to_char(a.send_date, 'dd/mm/yyyy') as senddt from neft_master a, neft_customer b, branch_master c where a.cust_id = b.cust_id and a.status_id = 0 and to_date(a.send_date) is not null and b.branch_id = c.branch_id and a.group_id = " & trid & "  group by a.doc_id,c.branch_name,b.beneficiary_account,a.amount,a.value_date,a.send_date,a.corporate_id order by a.corporate_id")
        Return ds
    End Function

    <WebMethod()> _
    Public Function Moddtl() As DataSet
        Dim ds As DataSet
        ds = oh.ExecuteDataSet("select 'ALL' as Mod_descr,0 as Module_id from dual union all select t.mod_descr,t.module_id from mod_master t where module_id in (5,6,20,44,39,92,10) order by module_id")
        Return ds
    End Function
    <WebMethod()> _
    Public Function Voucherid(ByVal Groupid As Integer) As String
        Dim dt As New DataTable
        Dim chkout As String = ""
        dt = oh.ExecuteDataSet("select distinct send_transid from neft_master where group_id=" & Groupid & " and status_id=0 and to_date(send_date)=to_date(sysdate) ").Tables(0)
        If dt.Rows.Count = 1 Then
            chkout = dt.Rows(0)(0)
        ElseIf dt.Rows.Count = 2 Then
            chkout = dt.Rows(0)(0) & " , " & dt.Rows(1)(0)
        ElseIf dt.Rows.Count = 3 Then
            chkout = dt.Rows(0)(0) & " , " & dt.Rows(1)(0) & "," & dt.Rows(2)(0)
        Else
            chkout = "Other"
        End If
        Return chkout
    End Function
    <WebMethod()> _
    Public Function SecondApplicantsList(ByVal cust_id As String) As DataSet
        Dim ds As DataSet
        ds = oh.ExecuteDataSet("select a.document_id as DocumentID,a.nominee_id,b.cust_id,a.name,a.fat_hus,a.house,'ConfimedNewposits' as ConfimSt,b.dep_dt from sub_applicants a, deposit_mst b where b.cust_id = '" & cust_id & "' and a.category = 1 and a.document_id = b.doc_id union all select a.verify_id as DocumentID,a.nominee_id,b.cust_id,a.name,a.fat_hus,a.house,'OnlyApplication' as ConfimSt,b.dep_dt from tmp_sub_applicants a, deposit_verification b where a.category = 1 and b.cust_id = '" & cust_id & "' and a.verify_id = b.verify_id and b.doc_id is null")
        Return ds
    End Function
    <WebMethod()> _
    Public Function Receipt_RetrurnBranchFill() As DataSet
        Dim dt As New DataSet
        dt = oh.ExecuteDataSet("select distinct a.branch_id,b.branch_name from BRANCH_RECEIPT_RETURN a,BRANCH_MASTER b where a.branch_id = b.branch_id and a.verified_by is null and a.verified_dt is null  order by b.branch_name")
        Return dt
    End Function
    <WebMethod()> _
    Public Function Receipt_TypeFill(ByVal BranchID As Integer) As DataSet
        Dim dt As New DataSet
        dt = oh.ExecuteDataSet("select distinct a.status_id, a.descr from COMMON_CONTROLS a,BRANCH_RECEIPT_RETURN b where a.module_id = 0 and a.param_id = 8 and a.status_id = b.status and b.branch_id = " & BranchID & " order by a.status_id")
        Return dt
    End Function
    <WebMethod()> _
    Public Function Receipt_ReturnDataFill(ByVal BranchID As Integer, ByVal TypeID As Integer) As DataSet
        Dim dt As New DataSet
        dt = oh.ExecuteDataSet("select a.barcode from BRANCH_RECEIPT_RETURN a where a.branch_id = " & BranchID & " and a.status = " & TypeID & " and a.verified_by is null and a.verified_dt is null order by a.barcode")
        Return dt
    End Function
    <WebMethod()> _
    Public Function Receipt_ReturnConfirm(ByVal BranchID As Integer, ByVal TypeID As Integer, ByVal UserID As String) As String
        Dim Params(3) As OracleParameter
        Dim DataStr As String = Nothing
        Params(0) = New OracleParameter("BranchID", OracleType.Number, 5)
        Params(0).Value = BranchID
        Params(1) = New OracleParameter("TypeID", OracleType.Number, 4)
        Params(1).Value = TypeID
        Params(2) = New OracleParameter("UserID", OracleType.VarChar, 50)
        Params(2).Value = UserID
        Params(3) = New OracleParameter("OutMessage", OracleType.VarChar, 100)
        Params(3).Direction = ParameterDirection.Output
        oh.ExecuteNonQuery("Stp_DepReceipt_ReturnConfirm", Params)
        DataStr = Params(3).Value
        Return DataStr
    End Function
    <WebMethod()> _
    Public Function NCDPANReminderletter() As DataSet
        Dim dt As New DataSet
        dt = oh.ExecuteDataSet("select d.doc_id as DocumentID,d.cust_name as CustomerName,c.fat_hus as FatherHus,c.house_name as Housename,c.locality || ',' || c.street as Locality,p.post_office || '-' || p.pin_code as Postoffice,a.district_name || ',' || s.state_name as DistState,b.country_name as Country1,d.cust_id as CustomerID from deposit_mst d,customer c,post_master p,district_master  a,state_master  s,country_dtl b,customer_detail z where d.dep_dt<'01-dec-2010' and d.status_id=1 and d.module_id in(5) and d.tds_code not in (0,1) and  c.pin_serial = p.sr_number and p.district_id = a.district_id and a.state_id = s.state_id and s.country_id = b.country_id and d.cust_id = c.cust_id and d.cust_id = z.cust_id and (not regexp_like(substr(z.pan, 0, 5), '^[[:upper:]]{5}$')  or not regexp_like(substr(z.pan, 6, 4), '^[[:digit:]]{4}$')  or not regexp_like(substr(z.pan, 10, 1), '^[[:upper:]]{1}$') or z.pan is null or substr(z.pan,4,1) not like case when d.tds_code in (3,8,9) then 'P' when d.tds_code in (5) then 'F' when d.tds_code in (6) then 'C' end )")
        Return dt
    End Function
    '<WebMethod()> _
    'Public Function NeftRecommbranchFill(ByVal Stattus As Integer) As DataSet
    '    Dim DT As New DataSet
    '    DT = oh.ExecuteDataSet("select -1 as branch_id, '-------Select-------' as branch_name from dual union select b.branch_id, b.branch_name from branch_master b, common_approvals n where b.branch_id = n.branch_id and n.status_id=" & Stattus & " and n.option_id=11")
    '    Return DT
    'End Function
    '<WebMethod()> _
    '  Public Function NeftRecommDataFill(ByVal brid As Integer, ByVal status As Integer) As DataSet
    '    Dim DT As New DataSet
    '    If status = 0 Then
    '        DT = oh.ExecuteDataSet("select '-------Select-------' as branch_id, '-1' as Neft_details from dual union select a.cust_id||'  '||a.name || ' ' || a.fat_hus || ',' ||a.house_name || ',' || a.locality||'  '||n.user_id||'  '||n.tra_dt ,a.cust_id||'µ'||a.name || 'µ' || a.fat_hus || ',' ||a.house_name || ',' || a.locality||'µ'||n.user_id||'µ'||n.tra_dt from branch_master b,common_approvals n,customer a where n.status_id =0 and n.branch_id = " & brid & " and n.doc_id=a.cust_id and b.branch_id = n.branch_id")
    '    Else
    '        DT = oh.ExecuteDataSet("select '-------Select-------' as branch_id, '-1' as Neft_details from dual union select a.cust_id||'  '||a.name || ' ' || a.fat_hus || ',' ||a.house_name || ',' || a.locality||'  '||n.user_id||'  '||n.tra_dt ,a.cust_id||'µ'||a.name || 'µ' || a.fat_hus || ',' ||a.house_name || ',' || a.locality||'µ'||n.recommend_by||'µ'||n.recommend_dt from branch_master b,common_approvals n,customer a where n.status_id =1 and n.branch_id = " & brid & " and n.doc_id=a.cust_id and b.branch_id = n.branch_id")
    '    End If
    '    Return DT
    'End Function
    <WebMethod()> _
    Public Function NCD_notice_30(ByVal fdt As Date, ByVal tdt As Date) As DataSet
        Dim dt As New DataSet
        dt = oh.ExecuteDataSet("select d.doc_id,d.cust_name, c.fat_hus, c.house_name,c.locality,c.street, p.post_office, a.district_name, s.state_name, f.country_name,p.pin_code ,c.phone1,a.emp_name from deposit_mst d, customer c,post_master p, district_master a, state_master s, country_dtl f, deposit_15g_notice b, employee_master a where d.doc_id=b.doc_id and d.tds_status=8 and to_date(b.letter_2)>=to_date('" & Format(fdt, "dd-MMM-yyyy") & "') and to_date(b.letter_2)<=to_date('" & Format(tdt, "dd-MMM-yyyy") & "')and c.pin_serial=p.sr_number and p.district_id=a.district_id and a.state_id= s.state_id and s.country_id= f.country_id and  d.cust_id=c.cust_id and d.branch_id=a.branch_id and a.post_id=10 and a.status_id=1 union select d.doc_id,d.cust_name, c.fat_hus, c.house_name,c.locality,c.street, p.post_office, a.district_name, s.state_name, f.country_name,p.pin_code ,c.phone1,a.emp_name from deposit_mst d, customer c,post_master p, district_master a, state_master s, country_dtl f, deposit_15g_notice b, employee_master a where d.doc_id=b.doc_id and d.tds_status=8 and to_date(b.letter_2)>=to_date('" & Format(fdt, "dd-MMM-yyyy") & "') and to_date(b.letter_2)<=to_date('" & Format(tdt, "dd-MMM-yyyy") & "')and c.pin_serial=p.sr_number and p.district_id=a.district_id and a.state_id= s.state_id and s.country_id= f.country_id and  d.cust_id=c.cust_id and d.branch_id=a.branch_id and a.post_id=198 and a.status_id=1 order by doc_id")
        Return dt
    End Function
    <WebMethod()> _
    Public Function NCD_notice_45(ByVal fdt As Date, ByVal tdt As Date) As DataSet
        Dim dt As New DataSet
        dt = oh.ExecuteDataSet("select d.doc_id,d.cust_name, c.fat_hus, c.house_name,c.locality,c.street, p.post_office, a.district_name, s.state_name, f.country_name,p.pin_code ,c.phone1,a.emp_name from deposit_mst d, customer c,post_master p, district_master a, state_master s, country_dtl f, deposit_15g_notice b, employee_master a where d.doc_id=b.doc_id and d.tds_status=8 and to_date(b.letter_3)>=to_date('" & Format(fdt, "dd-MMM-yyyy") & "') and to_date(b.letter_3)<=to_date('" & Format(tdt, "dd-MMM-yyyy") & "')and c.pin_serial=p.sr_number and p.district_id=a.district_id and a.state_id= s.state_id and s.country_id= f.country_id and  d.cust_id=c.cust_id and d.branch_id=a.branch_id and a.post_id=10 and a.status_id=1 union select d.doc_id,d.cust_name, c.fat_hus, c.house_name,c.locality,c.street, p.post_office, a.district_name, s.state_name, f.country_name,p.pin_code ,c.phone1,a.emp_name from deposit_mst d, customer c,post_master p, district_master a, state_master s, country_dtl f, deposit_15g_notice b, employee_master a where d.doc_id=b.doc_id and d.tds_status=8 and to_date(b.letter_3)>=to_date('" & Format(fdt, "dd-MMM-yyyy") & "') and to_date(b.letter_3)<=to_date('" & Format(tdt, "dd-MMM-yyyy") & "')and c.pin_serial=p.sr_number and p.district_id=a.district_id and a.state_id= s.state_id and s.country_id= f.country_id and  d.cust_id=c.cust_id and d.branch_id=a.branch_id and a.post_id=198 and a.status_id=1 order by doc_id")
        Return dt
    End Function
    <WebMethod()> _
    Public Function NCDbh(ByVal docid As String) As String
        Dim dt As New DataTable
        dt = oh.ExecuteDataSet("select a.emp_name from employee_master a, deposit_mst d where  d.branch_id=a.branch_id and a.post_id=10 and  a.status_id=1  and d.doc_id= '" & docid & "'").Tables(0)
        If dt.Rows.Count > 0 Then
            If Not IsDBNull(dt.Rows(0)(0)) Then
                Return dt.Rows(0)(0)
            Else
                Return ""
            End If
        Else
            Return ""
        End If
    End Function
    <WebMethod()> _
    Public Function NeftRecommbranchFill(ByVal Stattus As Integer, ByVal optionid As Integer) As DataSet
        Dim DT As New DataSet
        DT = oh.ExecuteDataSet("select -1 as branch_id, '-------Select-------' as branch_name from dual union select b.branch_id, b.branch_name from branch_master b, common_approvals n where b.branch_id = n.branch_id and n.status_id=" & Stattus & " and n.option_id=" & optionid & "")
        Return DT
    End Function
    <WebMethod()> _
    Public Function NeftRecommDataFill(ByVal brid As Integer, ByVal status As Integer, ByVal optionid As Integer) As DataSet
        Dim DT As New DataSet
        If optionid = 8 Then
            DT = oh.ExecuteDataSet("select '-------Select-------' as branch_id, '-1' as Neft_details from dual union select c.Doc_id||'  '||a.name || ' ' || a.fat_hus || ',' ||a.house_name || ',' || a.locality||'  '||n.user_id||'  '||n.tra_dt,c.Doc_id||'µ'||a.name || 'µ' || a.fat_hus || ',' ||a.house_name || ',' || a.locality||'µ'||n.user_id||'µ'||n.tra_dt from branch_master b, common_approvals n, customer a,deposit_mst c where n.option_id =" & optionid & " and n.status_id = " & status & " and n.branch_id =" & brid & " and n.doc_id = c.doc_id and a.cust_id=c.cust_id and b.branch_id = n.branch_id")
        ElseIf optionid = 11 Then
            DT = oh.ExecuteDataSet("select '-------Select-------' as branch_id, '-1' as Neft_details from dual union select n.doc_id||'  '||a.name || ' ' || a.fat_hus || ',' ||a.house_name || ',' || a.locality||'  '||n.user_id||'  '||n.tra_dt,n.doc_id||'µ'||a.name || 'µ' || a.fat_hus || ',' ||a.house_name || ',' || a.locality||'µ'||n.user_id||'µ'||n.tra_dt from branch_master b, common_approvals n, customer a where n.option_id = " & optionid & " and n.status_id = " & status & " and n.branch_id = " & brid & " and n.doc_id = a.cust_id and b.branch_id = n.branch_id")
        ElseIf optionid = 21 Then
            DT = oh.ExecuteDataSet("select '-------Select-------' as branch_id, '-1' as DEPOSIT_DETAILS  from dual union select b.branch_name || '  ' || n.doc_id || '  ' || a.cust_name || ' ' || a.dep_amt || ',' || a.dep_dt || ',' || a.due_dt || '  ' || n.user_id || '  ' || n.tra_dt, n.doc_id || 'µ' || a.cust_name || 'µ' || a.dep_amt || 'µ' || a.dep_dt || 'µ' || a.due_dt || 'µ' || n.user_id || 'µ' || n.tra_dt from branch_master b, common_approvals n, deposit_mst a where n.option_id = " & optionid & " and n.status_id = " & status & " and n.branch_id = " & brid & " and n.doc_id = a.doc_id and b.branch_id = n.branch_id")
        End If
        Return DT
    End Function
    <WebMethod()> _
    Public Function PANcardModification(ByVal confdata As String, ByVal panphoto() As Byte) As String
        '0 £ 02002600232883 £ AKFCN1452X £ 0 £ AKICP1234Z £ 23004
        '0        1               2        3        4         5       
        Dim Pan_data() As String = confdata.Split("£")
        Dim pr(4) As OracleParameter
        pr(0) = New OracleParameter("CustID", OracleType.VarChar, 16)
        pr(0).Value = Pan_data(1).ToString
        pr(1) = New OracleParameter("CustType", OracleType.Number, 3)
        pr(1).Value = Pan_data(3)
        pr(2) = New OracleParameter("NewPAN", OracleType.VarChar, 10)
        pr(2).Value = Pan_data(4)
        pr(3) = New OracleParameter("UserID", OracleType.VarChar, 30)
        pr(3).Value = Pan_data(5)
        pr(4) = New OracleParameter("ErrMsg", OracleType.VarChar, 1000)
        pr(4).Direction = ParameterDirection.InputOutput
        oh.ExecuteNonQuery("PanCardModification", pr)
        If Not (IsNothing(panphoto)) Then
            Dim sql1 As String = "update dms.DEPOSIT_PAN_DETAIL set PAN_COPY=:ph where cust_id = '" & Pan_data(1) & "'"
            Dim parm1(0) As OracleParameter
            parm1(0) = New OracleParameter
            parm1(0).ParameterName = "ph"
            parm1(0).OracleType = OracleType.Blob
            parm1(0).Direction = ParameterDirection.Input
            parm1(0).Value = panphoto
            oh.ExecuteNonQuery(sql1, parm1)
        End If
        Return pr(4).Value
    End Function
    <WebMethod()> _
    Public Function NCD_notice_1year(ByVal fdt As Date, ByVal tdt As Date, ByVal firmID As Integer) As DataSet
        Dim dt As New DataSet
        Dim fdt1 As String = Format(fdt, "dd-MMM-yyyy")
        Dim tdt1 As String = Format(tdt, "dd-MMM-yyyy")
        dt = oh.ExecuteDataSet("select t.doc_id, d.cust_name,c.fat_hus,c.house_name,c.locality,c.street,p.post_office,a.district_name,s.state_name,f.country_name,p.pin_code,c.phone1,d.due_dt,d.dep_amt  from deposit_maturity_notice t,deposit_mst d,customer c,post_master p,district_master a,state_master s,country_dtl f where t.due_dt >= to_date('" & fdt1 & "') and t.due_dt <= to_date('" & tdt1 & "') and not exists (select x.doc_id from pnl_security x where d.doc_id=x.doc_id) and d.module_id=6 and d.status_id=1 and d.firm_id=" & firmID & " and c.pin_serial = p.sr_number and p.district_id = a.district_id and a.state_id = s.state_id and s.country_id = f.country_id and t.doc_id=d.doc_id and d.cust_id = c.cust_id order by t.doc_id")
        Return dt
    End Function
    <WebMethod()> _
    Public Function NCD_notice_1year_Confirm(ByVal fdt As Date, ByVal tdt As Date, ByVal UserID As String, ByVal firmID As Integer) As String
        Try
            Dim dt As New DataTable
            Dim fdt1 As String = Format(fdt, "dd-MMM-yyyy")
            Dim tdt1 As String = Format(tdt, "dd-MMM-yyyy")

            dt = oh.ExecuteDataSet("select nvl(max(d.lett_status),0) from deposit_maturity_notice d where to_date(d.due_dt) >= to_date('" & fdt1 & "') and to_date(d.due_dt) <= to_date('" & tdt1 & "') and d.doc_id in (select a.doc_id from deposit_mst a where a.firm_id=" & firmID & ")").Tables(0)
            Dim status As Integer = dt.Rows(0)(0)
            status += 1

            oh.ExecuteNonQuery("update deposit_maturity_notice t set t.letter_dt=to_date(sysdate),t.lett_status=" & status & ",t.user_id='" & UserID & "' where t.doc_id in (select d.doc_id from deposit_mst d where d.firm_id=" & firmID & " and to_date(d.due_dt) >= to_date('" & fdt1 & "') and to_date(d.due_dt) <= to_date('" & tdt1 & "'))")
            Return "LETTER PRINTING UPDATED"
        Catch ex As Exception
            Return ex.Message.ToString
        End Try
    End Function
    <WebMethod()> _
    Public Function NCD_notice_1year_sum(ByVal fdt As Date, ByVal tdt As Date, ByVal firmID As Integer) As DataSet
        Dim dt As New DataSet
        Dim fdt1 As String = Format(fdt, "dd-MMM-yyyy")
        Dim tdt1 As String = Format(tdt, "dd-MMM-yyyy")
        dt = oh.ExecuteDataSet("select d.doc_id,nvl(d.lett_status,0),to_char(d.letter_dt,'dd-Mon-yyyy') from deposit_maturity_notice d where to_date(d.due_dt)>=to_date('" & fdt1 & "') and to_date(d.due_dt)<=to_date('" & tdt1 & "') and d.doc_id in (select a.doc_id from deposit_mst a where a.firm_id=" & firmID & ")")
        Return dt
    End Function

    <WebMethod()> _
    Public Function NCD_notice_1year_limit() As DataSet
        Dim dt As New DataSet
        dt = oh.ExecuteDataSet("select nvl(a.parmtr_value,0) from general_parameter a where a.firm_id=1 and a.module_id=1 and a.parmtr_id=0")
        Return dt
    End Function

    '*****************************req 7022 modification **********************************
    <WebMethod()> _
    Public Function add_customer(ByVal data As String, ByVal firmid As Integer, ByVal branchid As Integer, ByVal user_id As String, ByVal neftdata As String, ByVal rrn As String, ByVal uuid As String, ByVal servType As String) As String
        Dim datastr() As String = data.Split("¥")
        '-----------------------sebin----------------------------------------------------------------------------
        Dim parm_coll(65) As OracleParameter
        '---------------------------------------------------------------------------------------------------
        Dim message As String
        Dim oh1 As New Helper.Oracle.OracleHelper
        Try
            parm_coll(0) = New OracleParameter("fmno", OracleType.Number, 5)
            parm_coll(0).Value = CInt(firmid)
            parm_coll(0).Direction = ParameterDirection.Input

            parm_coll(1) = New OracleParameter("brno", OracleType.Number, 5)
            parm_coll(1).Value = CInt(branchid)
            parm_coll(1).Direction = ParameterDirection.Input

            parm_coll(2) = New OracleParameter("cusname", OracleType.VarChar, 40)
            parm_coll(2).Value = datastr(0)
            'ADADA/                             0
            parm_coll(2).Direction = ParameterDirection.Input

            parm_coll(3) = New OracleParameter("tele", OracleType.VarChar, 15)
            parm_coll(3).Value = datastr(12)
            '21323123123                        12
            parm_coll(3).Direction = ParameterDirection.Input

            parm_coll(4) = New OracleParameter("mob", OracleType.VarChar, 15)
            parm_coll(4).Value = datastr(13)
            '13123                              13
            parm_coll(4).Direction = ParameterDirection.Input

            parm_coll(5) = New OracleParameter("fathus", OracleType.VarChar, 40)
            parm_coll(5).Value = datastr(1)
            'ADASD                              1
            parm_coll(5).Direction = ParameterDirection.Input

            parm_coll(6) = New OracleParameter("housenm", OracleType.VarChar, 40)
            parm_coll(6).Value = datastr(2)
            'QWEQEWQW                           2
            parm_coll(6).Direction = ParameterDirection.Input

            parm_coll(7) = New OracleParameter("loca", OracleType.VarChar, 100)
            parm_coll(7).Value = datastr(4)
            'EQWSCAS                            4
            parm_coll(7).Direction = ParameterDirection.Input

            parm_coll(8) = New OracleParameter("pinsrl", OracleType.Number, 7)
            Dim pinsr() As String = datastr(8).Split("@")
            If Not IsNumeric(pinsr(1)) Then
                parm_coll(8).Value = DBNull.Value
            Else
                parm_coll(8).Value = CInt(pinsr(1))
            End If
            '680554@4016                        8
            parm_coll(8).Direction = ParameterDirection.Input

            parm_coll(9) = New OracleParameter("email_id", OracleType.VarChar, 35)
            parm_coll(9).Value = datastr(14)
            'zczcas                             14
            parm_coll(9).Direction = ParameterDirection.Input

            parm_coll(10) = New OracleParameter("occ_id", OracleType.Number, 2)
            If Not IsNumeric(datastr(11)) Then
                parm_coll(10).Value = DBNull.Value
            Else
                parm_coll(10).Value = CInt(datastr(11))
            End If

            '2                                  11
            parm_coll(10).Direction = ParameterDirection.Input

            parm_coll(11) = New OracleParameter("pass_id", OracleType.VarChar, 35)
            parm_coll(11).Value = ""
            parm_coll(11).Direction = ParameterDirection.Input

            parm_coll(12) = New OracleParameter("pan", OracleType.VarChar, 35)
            parm_coll(12).Value = datastr(29)
            parm_coll(12).Direction = ParameterDirection.Input

            Dim st As Date
            st = CDate(datastr(20))
            parm_coll(13) = New OracleParameter("dob", OracleType.DateTime, 11)
            parm_coll(13).Value = CDate(Format(st, "dd-MMM-yyyy"))
            '1                                  20
            parm_coll(13).Direction = ParameterDirection.Input

            parm_coll(14) = New OracleParameter("cust_type", OracleType.Number, 2)
            If Not IsNumeric(datastr(10)) Then
                parm_coll(14).Value = DBNull.Value
            Else
                parm_coll(14).Value = CInt(datastr(10))
            End If

            '8                                  10  
            parm_coll(14).Direction = ParameterDirection.Input

            parm_coll(15) = New OracleParameter("cntid", OracleType.Number, 4)
            If Not IsNumeric(datastr(5)) Then
                parm_coll(15).Value = DBNull.Value
            Else
                parm_coll(15).Value = CInt(datastr(5))
            End If

            '1                                  5
            parm_coll(15).Direction = ParameterDirection.Input

            parm_coll(16) = New OracleParameter("custid", OracleType.VarChar, 200)
            parm_coll(16).Direction = ParameterDirection.Output

            parm_coll(17) = New OracleParameter("id", OracleType.Number, 3)
            If Not IsNumeric(datastr(15)) Then
                parm_coll(17).Value = DBNull.Value
            Else
                parm_coll(17).Value = CInt(datastr(15))
            End If

            '5                                  15
            parm_coll(17).Direction = ParameterDirection.Input

            parm_coll(18) = New OracleParameter("id_no", OracleType.VarChar, 150)
            parm_coll(18).Value = datastr(16)
            'ASDAS12312                         16
            parm_coll(18).Direction = ParameterDirection.Input

            Dim isdt As Date
            If datastr(17) <> " " Then
                isdt = CDate(datastr(17))
            End If
            parm_coll(19) = New OracleParameter("is_dt", OracleType.DateTime, 11)
            If datastr(17) = " " Then
                parm_coll(19).Value = DBNull.Value
            Else
                parm_coll(19).Value = CDate(Format(isdt, "dd-MMM-yyyy"))
            End If
            'Friday, January 08, 2010           17
            parm_coll(19).Direction = ParameterDirection.Input

            Dim exdt As Date
            If datastr(18) <> " " Then
                exdt = CDate(datastr(18))
            End If
            parm_coll(20) = New OracleParameter("ex_dt", OracleType.DateTime, 11)
            If datastr(18) = " " Then
                parm_coll(20).Value = DBNull.Value
            Else
                parm_coll(20).Value = CDate(Format(exdt, "dd-MMM-yyyy"))
            End If
            'Thursday, December 08, 2011        18
            parm_coll(20).Direction = ParameterDirection.Input

            parm_coll(21) = New OracleParameter("is_plce", OracleType.VarChar, 40)
            parm_coll(21).Value = datastr(19)
            'ASDADAFriday, October 09, 2009     19
            parm_coll(21).Direction = ParameterDirection.Input

            parm_coll(22) = New OracleParameter("gen", OracleType.Number, 2)
            If Not IsNumeric(datastr(21)) Then
                parm_coll(22).Value = DBNull.Value
            Else
                parm_coll(22).Value = CInt(datastr(21))
            End If

            '1                                  21
            parm_coll(22).Direction = ParameterDirection.Input

            parm_coll(23) = New OracleParameter("p_street", OracleType.VarChar, 40)
            parm_coll(23).Value = datastr(3)
            'EQWSCAS                            3
            parm_coll(23).Direction = ParameterDirection.Input

            parm_coll(24) = New OracleParameter("p_media_id", OracleType.Number, 5)
            If Not IsNumeric(datastr(23)) Then
                parm_coll(24).Value = DBNull.Value
            Else
                parm_coll(24).Value = CInt(datastr(23))
            End If

            '1                                  22
            parm_coll(24).Direction = ParameterDirection.Input

            parm_coll(25) = New OracleParameter("p_module_id", OracleType.Number, 8)
            If Not IsNumeric(datastr(24)) Then
                parm_coll(25).Value = DBNull.Value
            Else
                parm_coll(25).Value = CInt(datastr(24))
            End If

            '1                                  24
            parm_coll(25).Direction = ParameterDirection.Input

            parm_coll(26) = New OracleParameter("userid", OracleType.VarChar, 40)
            parm_coll(26).Value = user_id
            parm_coll(26).Direction = ParameterDirection.Input
            '18                                 6
            '390                                7
            '680554                             9
            '1                                  25
            '                                   26
            '                                   28
            parm_coll(27) = New OracleParameter("typeid", OracleType.VarChar, 25)
            If IsNumeric(datastr(22)) Then
                parm_coll(27).Value = datastr(22)
            Else
                parm_coll(27).Value = DBNull.Value
            End If
            '0                                  23
            parm_coll(27).Direction = ParameterDirection.Input

            parm_coll(28) = New OracleParameter("cardNo", OracleType.VarChar, 40)
            parm_coll(28).Value = datastr(27)
            'F                                  27
            parm_coll(28).Direction = ParameterDirection.Input

            parm_coll(29) = New OracleParameter("shareflag", OracleType.VarChar, 2)
            parm_coll(29).Value = datastr(28)

            parm_coll(29).Direction = ParameterDirection.Input

            parm_coll(30) = New OracleParameter("err_msg", OracleType.VarChar, 300)
            parm_coll(30).Direction = ParameterDirection.Output

            parm_coll(31) = New OracleParameter("err_stat", OracleType.Number, 1)
            parm_coll(31).Direction = ParameterDirection.Output

            parm_coll(32) = New OracleParameter("neftdetails", OracleType.VarChar, 1000)
            parm_coll(32).Direction = ParameterDirection.Input
            parm_coll(32).Value = neftdata

            parm_coll(33) = New OracleParameter("LandHLD", OracleType.Number, 10)
            parm_coll(33).Direction = ParameterDirection.Input
            If Not IsNumeric(datastr(30)) Then
                parm_coll(33).Value = DBNull.Value
            Else
                parm_coll(33).Value = CInt(datastr(30))
            End If

            parm_coll(34) = New OracleParameter("EX_STATUS", OracleType.Number, 2)
            parm_coll(34).Direction = ParameterDirection.Input
            If Not IsNumeric(datastr(31)) Then
                parm_coll(34).Value = DBNull.Value
            Else
                parm_coll(34).Value = CInt(datastr(31))
            End If

            parm_coll(35) = New OracleParameter("EX_NO", OracleType.VarChar, 40)
            parm_coll(35).Direction = ParameterDirection.Input
            If Not IsNumeric(datastr(32)) Then
                parm_coll(35).Value = DBNull.Value
            Else
                parm_coll(35).Value = CStr(datastr(32))
            End If

            parm_coll(36) = New OracleParameter("relgn", OracleType.Number, 2)
            If Not IsNumeric(datastr(25)) Then
                parm_coll(36).Value = DBNull.Value
            Else
                parm_coll(36).Value = CInt(datastr(25))
            End If
            parm_coll(36).Direction = ParameterDirection.Input

            parm_coll(37) = New OracleParameter("cst", OracleType.Number, 2)
            If Not IsNumeric(datastr(26)) Then
                parm_coll(37).Value = DBNull.Value
            Else
                parm_coll(37).Value = CInt(datastr(26))
            End If
            parm_coll(37).Direction = ParameterDirection.Input

            parm_coll(38) = New OracleParameter("purofloan", OracleType.Number, 2)
            If Not IsNumeric(datastr(33)) Then
                parm_coll(38).Value = DBNull.Value
            Else
                parm_coll(38).Value = CInt(datastr(33))
            End If
            parm_coll(38).Direction = ParameterDirection.Input
            '------req 7022 req
            parm_coll(39) = New OracleParameter("alt_hname", OracleType.VarChar, 40)
            parm_coll(39).Direction = ParameterDirection.Input
            parm_coll(39).Value = datastr(34)

            parm_coll(40) = New OracleParameter("alt_loca", OracleType.VarChar, 40)
            parm_coll(40).Direction = ParameterDirection.Input
            parm_coll(40).Value = datastr(35)

            parm_coll(41) = New OracleParameter("alt_pin", OracleType.Number, 7)
            parm_coll(41).Direction = ParameterDirection.Input
            Dim altpinsr() As String = datastr(36).Split("@")
            If altpinsr.Length = 2 AndAlso altpinsr(1) <> "" Then
                parm_coll(41).Value = CInt(altpinsr(1))
            Else
                parm_coll(41).Value = DBNull.Value
            End If

            parm_coll(42) = New OracleParameter("kyc_of", OracleType.Number, 2)
            parm_coll(42).Direction = ParameterDirection.Input
            parm_coll(42).Value = datastr(37)

            parm_coll(43) = New OracleParameter("kyc_ml", OracleType.Number, 2)
            parm_coll(43).Direction = ParameterDirection.Input
            parm_coll(43).Value = datastr(38)

            parm_coll(44) = New OracleParameter("p_isactive", OracleType.Number, 2)
            parm_coll(44).Direction = ParameterDirection.Input
            parm_coll(44).Value = datastr(39)

            parm_coll(45) = New OracleParameter("cust_cat", OracleType.Number, 2)
            parm_coll(45).Direction = ParameterDirection.Input
            parm_coll(45).Value = datastr(40)

            parm_coll(46) = New OracleParameter("DSA_BA_USER", OracleType.Number, 2)
            parm_coll(46).Direction = ParameterDirection.Input
            parm_coll(46).Value = datastr(41)

            parm_coll(47) = New OracleParameter("Preflang", OracleType.Number, 2)
            parm_coll(47).Direction = ParameterDirection.Input
            parm_coll(47).Value = datastr(42)

            parm_coll(48) = New OracleParameter("KycRem", OracleType.VarChar, 500)
            parm_coll(48).Direction = ParameterDirection.Input
            parm_coll(48).Value = datastr(43)

            parm_coll(49) = New OracleParameter("PhotoRem", OracleType.VarChar, 500)
            parm_coll(49).Direction = ParameterDirection.Input
            parm_coll(49).Value = datastr(44)

            '-----------------------sebin----------------------------------------------------------------------------
            parm_coll(50) = New OracleParameter("Cust_PEP", OracleType.Number, 2)
            parm_coll(50).Direction = ParameterDirection.Input
            parm_coll(50).Value = datastr(45)
            '-----------------------------------------------------------------------------

            parm_coll(51) = New OracleParameter("CustMom", OracleType.VarChar, 38)
            parm_coll(51).Direction = ParameterDirection.Input
            parm_coll(51).Value = datastr(46)

            parm_coll(52) = New OracleParameter("Citizenship", OracleType.Number, 2)
            parm_coll(52).Direction = ParameterDirection.Input
            parm_coll(52).Value = datastr(47)

            parm_coll(53) = New OracleParameter("Nation", OracleType.Number, 2)
            parm_coll(53).Direction = ParameterDirection.Input
            parm_coll(53).Value = datastr(48)

            parm_coll(54) = New OracleParameter("Resident", OracleType.Number, 2)
            parm_coll(54).Direction = ParameterDirection.Input
            parm_coll(54).Value = datastr(49)

            parm_coll(55) = New OracleParameter("MaritalStat", OracleType.Number, 2)
            parm_coll(55).Direction = ParameterDirection.Input
            parm_coll(55).Value = datastr(50)

            parm_coll(56) = New OracleParameter("FatHusPre", OracleType.Number, 2)
            parm_coll(56).Direction = ParameterDirection.Input
            parm_coll(56).Value = datastr(51)

            parm_coll(57) = New OracleParameter("Prename", OracleType.Number, 2)
            parm_coll(57).Direction = ParameterDirection.Input
            parm_coll(57).Value = datastr(52)

            parm_coll(58) = New OracleParameter("AddrFlg", OracleType.Number, 2)
            parm_coll(58).Direction = ParameterDirection.Input
            parm_coll(58).Value = datastr(53)

            parm_coll(59) = New OracleParameter("EduQual", OracleType.Number, 2)
            parm_coll(59).Direction = ParameterDirection.Input
            parm_coll(59).Value = datastr(54)

            parm_coll(60) = New OracleParameter("NeedForLoan", OracleType.Number, 2)
            parm_coll(60).Direction = ParameterDirection.Input
            parm_coll(60).Value = datastr(55)

            parm_coll(61) = New OracleParameter("MonthIncome", OracleType.Number, 2)
            parm_coll(61).Direction = ParameterDirection.Input
            parm_coll(61).Value = datastr(56)

            parm_coll(62) = New OracleParameter("FirstGL", OracleType.Number, 2)
            parm_coll(62).Direction = ParameterDirection.Input
            parm_coll(62).Value = datastr(57)

            parm_coll(63) = New OracleParameter("Rrn", OracleType.VarChar, 50)
            parm_coll(63).Direction = ParameterDirection.Input
            parm_coll(63).Value = rrn

            parm_coll(64) = New OracleParameter("Uu_id", OracleType.VarChar, 150)
            parm_coll(64).Direction = ParameterDirection.Input
            parm_coll(64).Value = uuid

            parm_coll(65) = New OracleParameter("CustSource", OracleType.Number)
            parm_coll(65).Direction = ParameterDirection.Input
            parm_coll(65).Value = CInt(servType)

            Try
                oh1.ExecuteNonQuery("add_customer", parm_coll)
                message = Convert.ToString(parm_coll(30).Value) & "+" & Convert.ToString(parm_coll(31).Value) & "+" & Convert.ToString(parm_coll(16).Value)
                If Convert.ToString(parm_coll(30).Value) = "Customer ID recommended for SRM/RM Approval" Then
                    Srm_Customer_ApprovalMail(Convert.ToString(parm_coll(16).Value), branchid)
                End If
            Catch ex As Exception
                message = ex.Message
            End Try
        Catch ex As Exception
            message = ex.Message
        End Try
        Return message
    End Function

    <WebMethod()> _
    Public Function modify_customer_Displaydata(ByVal customer_id As String, ByVal branch_id As Integer, ByVal firm_id As Integer) As DataSet
        Dim odh As New Helper.Oracle.OracleHelper
        Dim ods As New DataSet
        Dim oddt As New DataTable
        Dim country_id, state_id, district_id As String
        Try
            oddt = odh.ExecuteDataSet("select * from identity id where identity_id not in (0,15,14) order by id.identity_name").Tables(0).Copy
            oddt.TableName = "identity"
            ods.Tables.Add(oddt)
            oddt = odh.ExecuteDataSet("select occupation_id,occupation_name from occupation_master om where om.status=1 and om.occupation_id <> 28 order by om.occupation_name").Tables(0).Copy
            oddt.TableName = "occupation_master"
            ods.Tables.Add(oddt)
            oddt = odh.ExecuteDataSet("select type_id,descr from customer_type").Tables(0).Copy
            oddt.TableName = "customer_type"
            ods.Tables.Add(oddt)
            oddt = odh.ExecuteDataSet("select country_id,country_name from country_dtl order by country_id ").Tables(0).Copy
            oddt.TableName = "country_dtl"
            country_id = oddt.Rows(0)(0).ToString
            ods.Tables.Add(oddt)
            oddt = odh.ExecuteDataSet("select state_id,state_name from state_master st order by st.state_name").Tables(0).Copy
            oddt.TableName = "state_master"
            state_id = oddt.Rows(0)(0).ToString
            ods.Tables.Add(oddt)
            oddt = odh.ExecuteDataSet("select district_id,district_name from district_master where state_id in (select bm.state_id from branch_master bm where bm.branch_id=" & branch_id & ")  order by district_name").Tables(0).Copy
            oddt.TableName = "district_master"
            district_id = oddt.Rows(0)(0).ToString
            ods.Tables.Add(oddt)
            oddt = odh.ExecuteDataSet("select pin_code ||'@'||sr_number as pincode,post_office from post_master where district_id in (select bm.district_id from branch_master bm where bm.branch_id=" & branch_id & ") order by post_office").Tables(0).Copy
            oddt.TableName = "post_master"
            ods.Tables.Add(oddt)
            oddt = odh.ExecuteDataSet(modigetcusid(customer_id, firm_id, branch_id)).Tables(0).Copy
            oddt.TableName = "customer_dt1"
            ods.Tables.Add(oddt)
            'oddt = odh.ExecuteDataSet("select ct.descr,om.occupation_name,cd.email_id,cd.date_of_birth,cd.pan from customer_detail cd,customer_type ct,occupation_master om where cust_id='" & customer_id & "' and cd.cust_type=ct.type_id and cd.occupation_id=om.occupation_id").Tables(0).Copy
            oddt = odh.ExecuteDataSet("select (select ct.descr from customer_type ct where ct.type_id = cd.cust_type) descr, (select om.occupation_name from occupation_master om where om.occupation_id = cd.occupation_id) occupation_name, cd.email_id, cd.date_of_birth, cd.pan, cd.emp_code, (select emp_name from employee_master where emp_code=cd.emp_code) emp_name from customer_detail cd where cust_id = '" & customer_id & "'").Tables(0).Copy
            oddt.TableName = "customer_dt2"
            ods.Tables.Add(oddt)
            oddt = odh.ExecuteDataSet("select iid.identity_name,idt.id_number,idt.issue_dt,idt.exp_dt,idt.issue_plce,idt.descr,iid.identity_id,idt.exservice_status,idt.pension_order from identity_dtl idt,identity iid where idt.cust_id='" & customer_id & "' and idt.identity_id=iid.identity_id").Tables(0).Copy
            oddt.TableName = "identity_values"
            ods.Tables.Add(oddt)
            oddt = odh.ExecuteDataSet("select t.type_id, t.type_name  from media_type_new t where t.type_id = 10 union all select type_id, type_name   from media_type_new m where m.type_id <> 10").Tables(0).Copy
            oddt.TableName = "media_type"
            ods.Tables.Add(oddt)
            oddt = odh.ExecuteDataSet("select media_id,media_name from media_master_new where type_id= " & ods.Tables("media_type").Rows(0)(0)).Tables(0).Copy
            oddt.TableName = "media_master"
            ods.Tables.Add(oddt)
            oddt = odh.ExecuteDataSet("select mem.media_id,mem.type_id from media_master_new mem,brand_awareness ba where ba.customer_id='" & customer_id & "' and ba.media_id=mem.media_id").Tables(0).Copy
            oddt.TableName = "media_master_list"
            ods.Tables.Add(oddt)
            oddt = odh.ExecuteDataSet("select card_no,issued_dt,issued_by,branch_id from customer_card_dtl where cust_id='" & customer_id & "' order by issued_dt desc").Tables(0).Copy
            oddt.TableName = "customer_card_dtl"
            ods.Tables.Add(oddt)
            '------req 7022
            oddt = odh.ExecuteDataSet("select post_master.post_office,post_master.pin_code ||'@'||post_master.sr_number as pincode,district_master.district_id,sm.state_id state,cd.country_name country,customer.alt_house_name,customer.alt_locality from  customer customer,post_master ,district_master,state_master sm,country_dtl cd where post_master.sr_number= customer.alt_pin_serial and district_master.district_id=post_master.district_id and customer.cust_id='" & customer_id & "' and sm.state_id=district_master.state_id  and cd.country_id=sm.country_id").Tables(0).Copy
            oddt.TableName = "customer_address_dtl"
            ods.Tables.Add(oddt)
        Catch ex As Exception

        End Try
        Return ods
    End Function

    <WebMethod()> _
    Public Function customer_modification(ByVal input_value() As String, ByVal input_dates() As Date, ByVal type As Integer, ByVal modi_key As String) As String
        'Public Function cust_modi() As String
        '
        Dim oh As New Helper.Oracle.OracleHelper
        Dim oh1 As New Helper.Oracle.OracleHelper
        '        Dim parm_coll(26) As OracleParameter '7022 req
        Dim parm_coll(49) As OracleParameter
        'Dim update_value() As String
        'Dim input_value(19) As String
        'Dim input_dates(6) As Date
        'Dim type As Integer = 2
        'Dim modi_key As String = "0!0!0!0!0!0!1!1!1!0!0!1!1!0!1!0!1!1!1!1!1!1!"
        'update_value = modi_key.Split("!")


        ' custid in varchar2,
        ' fathus in varchar2,
        ' housenm in varchar2,

        parm_coll(0) = New OracleParameter("custid", OracleType.VarChar, 16)
        parm_coll(0).Value = input_value(0)
        parm_coll(0).Direction = ParameterDirection.Input

        ' fathus in varchar2,
        parm_coll(1) = New OracleParameter("fathus", OracleType.VarChar, 100)
        parm_coll(1).Value = input_value(1)
        parm_coll(1).Direction = ParameterDirection.Input

        ' housenm in varchar2,
        parm_coll(2) = New OracleParameter("housenm", OracleType.VarChar, 100)
        parm_coll(2).Value = input_value(2)
        parm_coll(2).Direction = ParameterDirection.Input

        ' loca in varchar2,
        parm_coll(3) = New OracleParameter("loca", OracleType.VarChar, 40)
        parm_coll(3).Value = input_value(3)
        parm_coll(3).Direction = ParameterDirection.Input


        ' pinsrl in number,
        parm_coll(4) = New OracleParameter("pinsrl", OracleType.Number, 15)
        Dim pinsr() As String = input_value(4).Split("@")
        If IsNumeric(pinsr(1)) Then
            parm_coll(4).Value = CLng(pinsr(1))
        Else
            parm_coll(4).Value = DBNull.Value
        End If

        parm_coll(4).Direction = ParameterDirection.Input

        ' custtype in number,
        parm_coll(5) = New OracleParameter("custtype", OracleType.Number, 15)
        If IsNumeric(input_value(5)) Then
            parm_coll(5).Value = CInt(input_value(5))
        Else
            parm_coll(5).Value = DBNull.Value
        End If
        parm_coll(5).Direction = ParameterDirection.Input

        ' occ_id in number,
        parm_coll(6) = New OracleParameter("occ_id", OracleType.Number, 5)
        If IsNumeric(input_value(6)) Then
            parm_coll(6).Value = CInt(input_value(6))
        Else
            parm_coll(6).Value = DBNull.Value
        End If

        parm_coll(6).Direction = ParameterDirection.Input

        ' tele in varchar2,
        parm_coll(7) = New OracleParameter("tele", OracleType.VarChar, 40)
        parm_coll(7).Value = input_value(7)
        parm_coll(7).Direction = ParameterDirection.Input

        ' mob in varchar2,
        parm_coll(8) = New OracleParameter("mob", OracleType.VarChar, 40)
        parm_coll(8).Value = input_value(8)
        parm_coll(8).Direction = ParameterDirection.Input

        ' emailid in varchar2,
        parm_coll(9) = New OracleParameter("emailid", OracleType.VarChar, 40)
        If input_value(9) = "" Then
            parm_coll(9).Value = "NILL"
        Else
            parm_coll(9).Value = input_value(9)
        End If

        parm_coll(9).Direction = ParameterDirection.Input

        ' id_type in number,
        parm_coll(10) = New OracleParameter("id_type", OracleType.VarChar, 60)
        If IsNumeric(input_value(10)) Then
            parm_coll(10).Value = CInt(input_value(10))
        Else
            parm_coll(10).Value = DBNull.Value
        End If
        parm_coll(10).Direction = ParameterDirection.Input

        'id_no in varchar2,
        parm_coll(11) = New OracleParameter("id_no", OracleType.VarChar, 80)
        parm_coll(11).Value = input_value(11)
        parm_coll(11).Direction = ParameterDirection.Input

        ' date_of_issue in date,
        parm_coll(12) = New OracleParameter("date_of_issue", OracleType.DateTime, 22)
        If input_dates(0) <> "01-01-0001" Then
            parm_coll(12).Value = CDate(Format(input_dates(0), "dd-MMM-yyyy"))
        Else
            parm_coll(12).Value = DBNull.Value
        End If
        parm_coll(12).Direction = ParameterDirection.Input

        ' date_of_expiry in date,
        parm_coll(13) = New OracleParameter("date_of_expiry", OracleType.DateTime, 22)
        If input_dates(1) <> "01-01-0001" Then
            parm_coll(13).Value = CDate(Format(input_dates(1), "dd-MMM-yyyy"))
        Else
            parm_coll(13).Value = DBNull.Value
        End If
        parm_coll(13).Direction = ParameterDirection.Input

        ' place_of_issue in varchar2, 
        parm_coll(14) = New OracleParameter("place_of_issue", OracleType.VarChar, 40)
        parm_coll(14).Value = input_value(12)
        parm_coll(14).Direction = ParameterDirection.Input

        ' dob in date,
        Dim st As Date
        st = CDate(input_dates(2))
        parm_coll(15) = New OracleParameter("dob", OracleType.DateTime, 11)
        parm_coll(15).Value = CDate(Format(st, "dd-MMM-yyyy"))
        parm_coll(15).Direction = ParameterDirection.Input

        ' descr_modi in varchar2,
        parm_coll(16) = New OracleParameter("descr_modi", OracleType.VarChar, 60)
        parm_coll(16).Value = input_value(13)
        parm_coll(16).Direction = ParameterDirection.Input

        ' out_result out varchar2
        parm_coll(17) = New OracleParameter("out_result", OracleType.VarChar, 100)
        parm_coll(17).Direction = ParameterDirection.Output

        parm_coll(18) = New OracleParameter("update_modi", OracleType.VarChar, 60)
        parm_coll(18).Value = modi_key
        parm_coll(18).Direction = ParameterDirection.Input

        ' street in varchar2,
        parm_coll(19) = New OracleParameter("p_street", OracleType.VarChar, 40)
        parm_coll(19).Value = input_value(14)
        parm_coll(19).Direction = ParameterDirection.Input

        ' p_media_id in Number,
        parm_coll(20) = New OracleParameter("p_media_id", OracleType.Number, 8)
        If IsNumeric(input_value(15)) Then
            parm_coll(20).Value = CInt(input_value(15))
        Else
            parm_coll(20).Value = DBNull.Value
        End If
        parm_coll(20).Direction = ParameterDirection.Input

        ' p_module_id in Number,
        parm_coll(21) = New OracleParameter("p_module_id", OracleType.Number, 8)
        If IsNumeric(input_value(16)) Then
            parm_coll(21).Value = CInt(input_value(16))
        Else
            parm_coll(21).Value = DBNull.Value
        End If

        parm_coll(21).Direction = ParameterDirection.Input

        parm_coll(22) = New OracleParameter("ex_status", OracleType.Number, 2)
        If IsNumeric(input_value(17)) Then
            parm_coll(22).Value = CInt(input_value(17))
        Else
            parm_coll(22).Value = DBNull.Value
        End If

        parm_coll(22).Direction = ParameterDirection.Input

        parm_coll(23) = New OracleParameter("ex_no", OracleType.VarChar, 40)
        parm_coll(23).Value = CStr(input_value(18))
        parm_coll(23).Direction = ParameterDirection.Input

        parm_coll(24) = New OracleParameter("Empcode", OracleType.Number, 7)
        If IsNumeric(input_value(19)) Then
            parm_coll(24).Value = CInt(input_value(19))
        Else
            parm_coll(24).Value = DBNull.Value
        End If
        parm_coll(24).Direction = ParameterDirection.Input

        parm_coll(25) = New OracleParameter("Empname", OracleType.VarChar, 40)
        parm_coll(25).Value = CStr(input_value(20))
        parm_coll(25).Direction = ParameterDirection.Input

        parm_coll(26) = New OracleParameter("typeid", OracleType.VarChar)
        If IsNumeric(input_value(21)) Then
            parm_coll(26).Value = CInt(input_value(21))
        Else
            parm_coll(26).Value = DBNull.Value
        End If
        parm_coll(26).Direction = ParameterDirection.Input
        '----------------------7022 req
        parm_coll(27) = New OracleParameter("althouse", OracleType.VarChar, 40)
        parm_coll(27).Value = CStr(input_value(22))
        parm_coll(27).Direction = ParameterDirection.Input

        parm_coll(28) = New OracleParameter("altlocal", OracleType.VarChar, 40)
        parm_coll(28).Value = CStr(input_value(23))
        parm_coll(28).Direction = ParameterDirection.Input

        parm_coll(29) = New OracleParameter("altpin", OracleType.Number, 7)
        Dim altpinsr() As String = input_value(24).Split("@")
        If IsNumeric(altpinsr(1)) Then
            parm_coll(29).Value = CLng(altpinsr(1))
        Else
            parm_coll(29).Value = DBNull.Value
        End If
        'added for KYC chenges
        parm_coll(30) = New OracleParameter("userid", OracleType.VarChar, 10)
        parm_coll(30).Value = CStr(input_value(25))
        parm_coll(30).Direction = ParameterDirection.Input

        parm_coll(31) = New OracleParameter("kyc_of", OracleType.Number, 2)
        parm_coll(31).Value = CStr(input_value(26))
        parm_coll(31).Direction = ParameterDirection.Input

        parm_coll(32) = New OracleParameter("cust_namepar", OracleType.VarChar, 40)
        parm_coll(32).Value = input_value(27)
        parm_coll(32).Direction = ParameterDirection.Input

        parm_coll(33) = New OracleParameter("cust_gender", OracleType.Number, 2)
        parm_coll(33).Value = Convert.ToInt32(input_value(28))
        parm_coll(33).Direction = ParameterDirection.Input

        parm_coll(34) = New OracleParameter("preflang", OracleType.Number, 2)
        parm_coll(34).Value = Convert.ToInt32(input_value(29))
        parm_coll(34).Direction = ParameterDirection.Input

        parm_coll(35) = New OracleParameter("pep", OracleType.Number, 2)
        parm_coll(35).Value = Convert.ToInt32(input_value(30))
        parm_coll(35).Direction = ParameterDirection.Input

        parm_coll(36) = New OracleParameter("mom_name", OracleType.VarChar, 38)
        parm_coll(36).Value = input_value(31).ToString()
        parm_coll(36).Direction = ParameterDirection.Input

        parm_coll(37) = New OracleParameter("marital", OracleType.Number, 2)
        parm_coll(37).Value = Convert.ToInt32(input_value(32))
        parm_coll(37).Direction = ParameterDirection.Input

        parm_coll(38) = New OracleParameter("p_citizen", OracleType.Number, 2)
        parm_coll(38).Value = Convert.ToInt32(input_value(33))
        parm_coll(38).Direction = ParameterDirection.Input

        parm_coll(39) = New OracleParameter("nation", OracleType.Number, 2)
        parm_coll(39).Value = Convert.ToInt32(input_value(34))
        parm_coll(39).Direction = ParameterDirection.Input

        parm_coll(40) = New OracleParameter("resid", OracleType.Number, 2)
        parm_coll(40).Value = Convert.ToInt32(input_value(35))
        parm_coll(40).Direction = ParameterDirection.Input

        parm_coll(41) = New OracleParameter("prename", OracleType.Number, 2)
        parm_coll(41).Value = Convert.ToInt32(input_value(36).ToString().Split("~")(0))
        parm_coll(41).Direction = ParameterDirection.Input

        parm_coll(42) = New OracleParameter("FatHusPre", OracleType.Number, 2)
        parm_coll(42).Value = Convert.ToInt32(input_value(37))
        parm_coll(42).Direction = ParameterDirection.Input

        parm_coll(43) = New OracleParameter("MEmail", OracleType.VarChar, 40)
        parm_coll(43).Value = input_value(38).ToString()
        parm_coll(43).Direction = ParameterDirection.Input

        parm_coll(44) = New OracleParameter("Rel", OracleType.Number, 2)
        parm_coll(44).Value = Convert.ToInt32(input_value(39))
        parm_coll(44).Direction = ParameterDirection.Input

        parm_coll(45) = New OracleParameter("cas", OracleType.Number, 2)
        If input_value(40).ToString() = "" Then
            parm_coll(45).Value = 0
        Else
            parm_coll(45).Value = Convert.ToInt32(input_value(40))
        End If
        parm_coll(45).Direction = ParameterDirection.Input

        parm_coll(46) = New OracleParameter("EduQual", OracleType.Number, 2)
        parm_coll(46).Value = Convert.ToInt32(input_value(41))
        parm_coll(46).Direction = ParameterDirection.Input

        parm_coll(47) = New OracleParameter("NeedForLoan", OracleType.Number, 2)
        parm_coll(47).Value = Convert.ToInt32(input_value(42))
        parm_coll(47).Direction = ParameterDirection.Input

        parm_coll(48) = New OracleParameter("MIncome", OracleType.Number, 2)
        parm_coll(48).Value = Convert.ToInt32(input_value(43))
        parm_coll(48).Direction = ParameterDirection.Input

        parm_coll(49) = New OracleParameter("FirstGL", OracleType.Number, 2)
        parm_coll(49).Value = Convert.ToInt32(input_value(44))
        parm_coll(49).Direction = ParameterDirection.Input

        Try
            If type = 1 Then
                oh1.ExecuteNonQuery("other_customer_modi_new2", parm_coll)
                If parm_coll(17).Value = "0" Then
                    Return ("0")
                Else
                    Return parm_coll(17).Value
                End If
            ElseIf type = 2 Then
                oh1.ExecuteNonQuery("customer_modification", parm_coll)
                If parm_coll(17).Value = "0" Then
                    Return (0)
                Else
                    Return parm_coll(17).Value
                End If
            Else
                Return "You must Enter the type"
            End If
        Catch ex As Exception
            Return ex.Message.ToString
        End Try

    End Function
    '-----------------SD TRANSFER-----------17-Feb-2012
    <WebMethod()> _
    Public Function SDTransferMabenBranches() As DataSet
        Dim DT As New DataSet
        DT = oh.ExecuteDataSet("select -1 as branch_id, '-------Select-------' as branch_name from dual union select b.branch_id, b.branch_name from branch_master b where b.firm_id=2 union select b.branch_id, b.branch_name from branch_master b where b.branch_id in (19,7,16,12,58,57) order by branch_name")
        Return DT
    End Function
    <WebMethod()> _
    Public Function SDDetails(ByVal DocID As String) As String
        Dim dt As New DataTable
        dt = oh.ExecuteDataSet("select t.status_id,t.cust_name,a.branch_name,to_char(t.dep_dt, 'dd-Mon-yyyy'),nvl(t.dep_amt,0) from deposit_mst t,branch_master a where t.branch_id<>t.ir_branch and t.module_id=4 and t.status_id=1 and t.ir_branch=a.branch_id and a.firm_id<>2 and t.doc_id = '" & DocID & "' and t.branch_id=0").Tables(0)
        If dt.Rows.Count > 0 Then
            If Not IsDBNull(dt.Rows(0)(0)) Then
                Return dt.Rows(0)(0) & "!" & dt.Rows(0)(1) & "!" & dt.Rows(0)(2) & "!" & dt.Rows(0)(3) & "!" & dt.Rows(0)(4)
            Else
                Return ""
            End If
        Else
            Return ""
        End If
    End Function
    <WebMethod()> _
    Public Function SDTransferRequest(ByVal DocID As String, ByVal UserID As String, ByVal BranchId As Integer) As Integer
        Dim Params(5) As OracleParameter
        Dim DataStr As String = Nothing
        Params(0) = New OracleParameter("DocID", OracleType.VarChar, 20)
        Params(0).Value = DocID
        Params(1) = New OracleParameter("UserID", OracleType.VarChar, 50)
        Params(1).Value = UserID
        Params(2) = New OracleParameter("OptionID", OracleType.Number, 2)
        Params(2).Value = 1
        Params(3) = New OracleParameter("Branch", OracleType.Number, 10)
        Params(3).Value = BranchId
        Params(4) = New OracleParameter("OutMessage", OracleType.VarChar, 100)
        Params(4).Direction = ParameterDirection.Output
        Params(5) = New OracleParameter("MsgId", OracleType.Number, 2)
        Params(5).Direction = ParameterDirection.Output
        oh.ExecuteNonQuery("Stp_SDTransferToMaben", Params)
        DataStr = Params(5).Value
        Return DataStr

    End Function
    <WebMethod()> _
    Public Function SDTransferPending() As DataSet
        Dim DT As New DataSet
        DT = oh.ExecuteDataSet("select '-------Select-------' as doc_id, '-1' as Details from dual union select b.branch_name||'-'||t.doc_id||'-'||a.cust_name,b.branch_name||'µ'||t.doc_id||'µ'||a.cust_name||'µ'||a.dep_dt||'µ'||a.dep_amt||'µ'||c.branch_name||'µ'||t.amount as Details from common_approvals t,deposit_mst a,branch_master b,branch_master c where  t.option_id=22 and t.doc_id=a.doc_id and a.ir_branch=b.branch_id and t.amount=c.branch_id and t.status_id=0 and a.module_id=4 and a.branch_id=0 order by doc_id")
        Return DT
    End Function
    <WebMethod()> _
    Public Function SDTransferApprove(ByVal DocID As String, ByVal UserID As String, ByVal BranchId As Integer) As Integer
        Dim Params(4) As OracleParameter
        Dim DataStr As String = Nothing
        Params(0) = New OracleParameter("docid", OracleType.VarChar, 20)
        Params(0).Value = DocID
        Params(1) = New OracleParameter("tfrbranchid", OracleType.Number, 50)
        Params(1).Value = BranchId
        Params(2) = New OracleParameter("UserID", OracleType.VarChar, 50)
        Params(2).Value = UserID
        Params(3) = New OracleParameter("Err_msg", OracleType.VarChar, 100)
        Params(3).Direction = ParameterDirection.Output
        Params(4) = New OracleParameter("Err_stat", OracleType.Number, 5)
        Params(4).Direction = ParameterDirection.Output
        oh.ExecuteNonQuery("MafilSDtfrtoMabenbranch", Params)
        DataStr = Params(4).Value
        Return DataStr

    End Function
    <WebMethod()> _
    Public Function RDDetails(ByVal DocID As String) As String
        Dim dt As New DataTable
        dt = oh.ExecuteDataSet("select t.status_id,t.cust_name,a.branch_name,to_char(t.dep_dt, 'dd-Mon-yyyy'),nvl(t.dep_amt,0) from deposit_mst t,branch_master a where t.branch_id<>t.ir_branch and t.module_id=3 and t.status_id=1 and t.ir_branch=a.branch_id and a.firm_id<>2 and t.doc_id = '" & DocID & "' and t.branch_id=0").Tables(0)
        If dt.Rows.Count > 0 Then
            If Not IsDBNull(dt.Rows(0)(0)) Then
                Return dt.Rows(0)(0) & "!" & dt.Rows(0)(1) & "!" & dt.Rows(0)(2) & "!" & dt.Rows(0)(3) & "!" & dt.Rows(0)(4)
            Else
                Return ""
            End If
        Else
            Return ""
        End If
    End Function
    <WebMethod()> _
    Public Function RDTransferPending() As DataSet
        Dim DT As New DataSet
        DT = oh.ExecuteDataSet("select '-------Select-------' as doc_id, '-1' as Details  from dual union select b.branch_name || '-' || t.doc_id || '-' || a.cust_name,b.branch_name || 'µ' || t.doc_id || 'µ' || a.cust_name || 'µ' || a.dep_dt || 'µ' || sum(decode(d.type,'C',d.amount,d.amount*-1)) || 'µ' || c.branch_name || 'µ' || t.amount as Details from common_approvals t, deposit_mst a, branch_master b, branch_master c,deposit_tran d where t.option_id = 22 and t.doc_id = a.doc_id and a.ir_branch = b.branch_id and t.amount = c.branch_id and t.status_id = 0 and a.doc_id=d.doc_id and a.module_id=3 and a.branch_id=0 group by b.branch_name,t.doc_id,a.cust_name,b.branch_name,t.doc_id,a.cust_name,a.dep_dt,c.branch_name,t.amount")
        Return DT
    End Function
    <WebMethod()> _
    Public Function RDTransferApprove(ByVal DocID As String, ByVal UserID As String, ByVal BranchId As Integer) As Integer
        Dim Params(4) As OracleParameter
        Dim DataStr As String = Nothing
        Params(0) = New OracleParameter("docid", OracleType.VarChar, 20)
        Params(0).Value = DocID
        Params(1) = New OracleParameter("tfrbranchid", OracleType.Number, 50)
        Params(1).Value = BranchId
        Params(2) = New OracleParameter("UserID", OracleType.VarChar, 50)
        Params(2).Value = UserID
        Params(3) = New OracleParameter("Err_msg", OracleType.VarChar, 100)
        Params(3).Direction = ParameterDirection.Output
        Params(4) = New OracleParameter("Err_stat", OracleType.Number, 5)
        Params(4).Direction = ParameterDirection.Output
        oh.ExecuteNonQuery("MafilRDtfrtoMabenbranch", Params)
        DataStr = Params(4).Value
        Return DataStr

    End Function

    <WebMethod()> _
    Public Function GetAddressProof() As DataSet
        Dim dsOut As New DataSet
        dsOut = oh.ExecuteDataSet("select 0 as status_id, '--Select--' as description from dual union select status_id,description from status_master where module_id=1 and option_id=33")
        Return dsOut
    End Function

    <WebMethod()> _
    Public Function UpdateKYCInfo(ByVal as_flag As String, ByVal custID As String, ByVal addproofID As String, ByVal param As String, ByVal userid As String) As String
        Dim Params(5) As OracleParameter
        Dim DataStr As String = String.Empty
        Params(0) = New OracleParameter("as_flag", OracleType.VarChar, 100)
        Params(0).Value = as_flag
        Params(1) = New OracleParameter("custid", OracleType.VarChar, 200)
        Params(1).Value = custID
        Params(2) = New OracleParameter("addproofid", OracleType.VarChar, 100)
        Params(2).Value = addproofID
        Params(3) = New OracleParameter("param", OracleType.VarChar)
        Params(3).Value = param
        Params(4) = New OracleParameter("userid", OracleType.VarChar)
        Params(4).Value = userid
        Params(5) = New OracleParameter("out_msg", OracleType.VarChar, 100)
        Params(5).Direction = ParameterDirection.Output
        oh.ExecuteNonQuery("UpdateKYCInfo", Params)
        DataStr = Convert.ToString(Params(4).Value)
        Return DataStr
    End Function

    '<WebMethod()> _
    'Public Function AddAdditionalKycPhoto(ByVal custid As String, ByVal addKyc_photo() As Byte, ByVal idNumber As String) As String
    '    Dim ods As New DataSet
    '    Dim dt As New DataTable
    '    Try
    '        If Not IsNothing(addKyc_photo) Then
    '            Dim sql As String = "update dms.additional_kyc_photo set kyc_photo=:ph where cust_id='" & custid & "' and id_number='" & idNumber & "'"
    '            Dim parm(0) As OracleParameter
    '            parm(0) = New OracleParameter
    '            parm(0).ParameterName = "ph"
    '            parm(0).OracleType = OracleType.Blob
    '            parm(0).Direction = ParameterDirection.Input
    '            parm(0).Value = addKyc_photo
    '            oh.ExecuteNonQuery(sql, parm)
    '        End If
    '        Return "Complete"
    '    Catch ex As Exception
    '        Return ex.Message
    '    End Try
    'End Function

    <WebMethod()> _
    Public Function GetAdditionalKyc(ByVal custid As String) As DataSet
        Dim dsOut As New DataSet
        dsOut = oh.ExecuteDataSet("select t.identity_id,t.id_number,t.issue_dt,t.exp_dt,t.issue_plce,t.address_proof,im.identity_name from additional_kyc t,identity im where t.cust_id='" & custid & "' and im.identity_id(+)=t.identity_id")
        Return dsOut
    End Function


    <WebMethod()> _
    Public Function GetNonKycType() As DataSet
        Dim dsOut As New DataSet
        dsOut = oh.ExecuteDataSet("select status_id,description from status_master where module_id=1 and option_id=36 order by order_by")
        Return dsOut
    End Function

    <WebMethod()> _
    Public Function GetNonKycDetails(ByVal id As Integer) As DataSet
        Dim dsOut As New DataSet
        If id = 0 Then
            dsOut = oh.ExecuteDataSet("select * from identity id where 1=2")
        ElseIf id = 1 Then
            dsOut = oh.ExecuteDataSet("select * from identity id where identity_id between 100 and 199 order by id.identity_name")
        ElseIf id = 2 Then
            dsOut = oh.ExecuteDataSet("select * from identity id where identity_id between 200 and 299 order by id.identity_name")
        ElseIf id = 3 Then
            dsOut = oh.ExecuteDataSet("select * from identity id where identity_id between 300 and 399 order by id.identity_name")
        ElseIf id = 4 Then
            dsOut = oh.ExecuteDataSet("select * from identity id where identity_id between 400 and 499 order by id.identity_name")
        ElseIf id = 10 Then
            dsOut = oh.ExecuteDataSet("select * from identity id where identity_id in(1,14,3,2,4,16,5) order by id.identity_name")
        ElseIf id = 20 Then
            dsOut = oh.ExecuteDataSet("select * from identity id where identity_id not in(0,1,14,3,2,4,16,15,5)  and identity_id <100 order by id.identity_name")
            'ElseIf id = 16 Then
            '    dsOut = oh.ExecuteDataSet("select * from identity id where identity_id in(406) order by id.identity_name")
        ElseIf id = 5 Then
            dsOut = oh.ExecuteDataSet("select * from identity id where identity_id between 550 and 599 order by id.identity_name")
        ElseIf id = 16 Then
            dsOut = oh.ExecuteDataSet("select * from identity id where identity_id between 500 and 549 order by id.identity_name")
        Else
            dsOut = oh.ExecuteDataSet("select * from identity id where identity_id <>15 and identity_id<>0 and identity_id < 100 order by id.identity_name")
        End If
        Return dsOut
    End Function
    <WebMethod()> _
    Public Function Getverification(ByVal docid As String, ByVal statusid As Integer, ByVal userid As String) As Integer
        If statusid = 1 Then
            oh.ExecuteNonQuery("update deposit_applications t set t.verified_dt=sysdate,t.verified_id='" & userid & "',status_id=1 where t.doc_id='" & docid & "' and t.status_id=0")
        ElseIf statusid = 2 Then
            oh.ExecuteNonQuery("update deposit_applications t set t.verified_dt=sysdate,t.verified_id='" & userid & "',status_id=2 where t.doc_id='" & docid & "' and t.status_id=0")
        ElseIf statusid = 3 Then
            oh.ExecuteNonQuery("update common_approvals t set t.confirm_dt=sysdate,t.confirm_by='" & userid & "',status_id=2  where t.doc_id='" & docid & "' and status_id=1 and t.option_id=50")
        ElseIf statusid = 4 Then
            oh.ExecuteNonQuery("update common_approvals t set t.rejected_dt=sysdate,t.rejected_by='" & userid & "',status_id=5   where t.doc_id='" & docid & "' and status_id=1 and t.option_id=50")

        End If
        Return 1
    End Function
    <WebMethod()> _
    Public Function lock_approve(ByVal usid As String) As DataSet
        Dim QUERY As String
        Dim dset As New DataSet
        QUERY = "select count(*) from form_accessibility t where t.emp_id='" & usid & "' and t.form_id=616"
        dset = oh.ExecuteDataSet(QUERY)
        Return dset
    End Function
    <WebMethod()> _
    Public Function lock_release_aprove_pend(ByVal frid As Integer) As DataSet
        Dim QUERY As String
        Dim dset As New DataSet
        QUERY = "select t.doc_id,t.doc_id||' - '||tt.branch_name disp from common_approvals t,branch_master tt where t.branch_id=tt.branch_id and t.option_id=50 and t.status_id=1 order by tt.branch_name"
        dset = oh.ExecuteDataSet(QUERY)
        Return dset
    End Function
    <WebMethod()> _
    Public Function lock_release_doc_list(ByVal docid As String) As DataSet
        Dim QUERY As String
        Dim dset As New DataSet
        QUERY = "select tt.doc_id,to_char(tt.dep_dt,'dd/Mon/yyyy') NCDDate,to_char(tt.due_dt,'dd/Mon/yyyy') duedt,tt.dep_amt Amount,tt.cust_name,to_char(t.tra_dt,'dd/Mon/yyyy') request_dt,tt.int_rt interest_Rt from deposit_mst tt,common_approvals t where t.doc_id=tt.doc_id and  tt.doc_id='" & docid & "' "
        dset = oh.ExecuteDataSet(QUERY)
        Return dset
    End Function

    <WebMethod()> _
    Public Function lock_request(ByVal Type As Integer, ByVal ConfDtl As String) As String
        Dim result As String
        Dim parm_coll(2) As OracleParameter
        Try
            parm_coll(0) = New OracleParameter("TypeID", OracleType.Number, 1)
            parm_coll(0).Value = Type
            parm_coll(0).Direction = ParameterDirection.Input
            parm_coll(1) = New OracleParameter("ConfirmDtl", OracleType.VarChar, 32750)
            parm_coll(1).Value = ConfDtl
            parm_coll(1).Direction = ParameterDirection.Input
            parm_coll(2) = New OracleParameter("Msg", OracleType.VarChar, 300)
            parm_coll(2).Direction = ParameterDirection.Output
            oh.ExecuteNonQuery("deposit_lock_change", parm_coll)
            result = parm_coll(2).Value
        Catch ex As Exception
            result = ex.ToString
        End Try
        Return result
    End Function
    <WebMethod()> _
    Public Function attachfile(ByVal docid As String) As DataSet
        Dim QUERY As String
        Dim dset As New DataSet
        QUERY = "select t.application_form img from common_approvals t where t.doc_id='" & docid & "' and t.option_id=50"
        dset = oh.ExecuteDataSet(QUERY)
        Return dset
    End Function
    <WebMethod()> _
    Public Function lock_db_status(ByVal brid As Integer, ByVal frdt As String, ByVal todt As String) As DataSet
        Dim QUERY As String
        Dim dset As New DataSet
        If brid = 0 Then
            QUERY = "select b.branch_name,t.doc_id, tt.cust_name,decode(t.status_id, 2, 'Approve', 'Pending') status,t.tra_dt reqdt,t.status_id from common_approvals t, deposit_mst tt,branch_master b where t.doc_id = tt.doc_id and tt.branch_id=b.branch_id and t.option_id = 50 and t.tra_dt between '" & frdt & "' and '" & todt & "'"
        Else
            QUERY = "select b.branch_name,t.doc_id, tt.cust_name,decode(t.status_id, 2, 'Approve', 'Pending') status,t.tra_dt reqdt,t.status_id from common_approvals t, deposit_mst tt,branch_master b where t.doc_id = tt.doc_id and tt.branch_id=b.branch_id and t.option_id = 50 and t.branch_id =" & brid & " and t.tra_dt between '" & frdt & "' and '" & todt & "'"
        End If
        dset = oh.ExecuteDataSet(QUERY)
        Return dset
    End Function
    'Trustee Cheque settlement------31-Jul-2012
    <WebMethod()> _
    Public Function TRUSTEEDATABranch() As DataSet
        Dim DT As New DataSet
        DT = oh.ExecuteDataSet("select ' -------Select Branch-------' as branch_name, -1 as branch_id  from dual union select a.branch_name,t.branch_id from trusteeaccount_mst t,branch_master a where t.status_id=3 and t.pay_mode='CH' and t.branch_id=a.branch_id order by branch_name")
        Return DT
        '
    End Function
    <WebMethod()> _
    Public Function TRUSTEEDATAcheque(ByVal brid As Integer) As DataSet
        Dim ds As DataSet
        ds = oh.ExecuteDataSet("select t.trustee_id,t.doc_id,t.cust_id,t.cust_name,t.principal,t.balance,to_char(t.tra_dt,'dd-Mon-yyyy') Tra_dt,to_char(t.cls_dt,'dd-Mon-yyyy') cls_dt,to_char(t.hoverified_date,'dd-Mon-yyyy') HO_dt,to_char(t.brverified_date,'dd-Mon-yyyy') BR_dt from trusteeaccount_mst t where t.status_id=3 and t.pay_mode='CH' and t.branch_id=" & brid & " order by BR_dt")
        Return ds
    End Function

    <WebMethod()> _
    Public Function TRUSTEEchequeConfirm(ByVal ChequeNo As Integer, ByVal TrusteeID As String, ByVal UserID As String, ByVal ChequeDate As String) As String
        Dim Params(4) As OracleParameter
        Dim DataStr As String = Nothing
        Params(0) = New OracleParameter("TrusteeID", OracleType.VarChar, 20)
        Params(0).Value = TrusteeID
        Params(1) = New OracleParameter("ChequeNo", OracleType.Number, 10)
        Params(1).Value = ChequeNo
        Params(2) = New OracleParameter("ChequeDate", OracleType.DateTime)
        Params(2).Value = ChequeDate
        Params(3) = New OracleParameter("UserID", OracleType.VarChar, 50)
        Params(3).Value = UserID
        Params(4) = New OracleParameter("OutMessage", OracleType.VarChar, 100)
        Params(4).Direction = ParameterDirection.Output

        oh.ExecuteNonQuery("Stp_trustee_cheque_settle", Params)
        DataStr = Params(4).Value
        Return DataStr
    End Function
    <WebMethod()> _
    Public Function Submitneft_tdsreversal(ByVal CustId As String, ByVal BranchID As Integer, ByVal FirmId As Integer, ByVal UserID As String) As String
        ' CustId        in  varchar2,
        'FirmId        in  number,
        'UserID        in  varchar2,
        'OutMessage    out varchar2
        Dim pr(3) As OracleParameter
        pr(0) = New OracleParameter("CustId", OracleType.VarChar, 14)
        pr(0).Value = CustId
        pr(1) = New OracleParameter("FirmId", OracleType.Number, 5)
        pr(1).Value = FirmId
        pr(2) = New OracleParameter("UserID", OracleType.VarChar, 50)
        pr(2).Value = UserID
        pr(3) = New OracleParameter("OutMessage", OracleType.VarChar, 1000)
        pr(3).Direction = ParameterDirection.InputOutput
        oh.ExecuteNonQuery("Stp_NeftTDsReversal", pr)
        Return pr(3).Value
    End Function

    <WebMethod()> _
    Public Function AddCustomerPhotoForApproval(ByVal custid As String, ByVal cust_photo() As Byte, ByVal branch_id As Integer, ByVal userID As Integer, ByVal custBranchID As Integer) As String
        Dim ods As New DataSet
        Dim dt As New DataTable
        Dim message As String = Nothing
        Try
            'ods = oh.ExecuteDataSet("select count(*) from customer_photo where cust_id='" & custid & "'")
            'Modified by Tijo For Customer name and other related approval change
            ods = oh.ExecuteDataSet("select count(*) from dms.kyc_pre_auth p where p.change_type = 5 and p.status_id in (3,7) and p.cust_id = '" & custid & "'")
            If ods.Tables(0).Rows(0)(0) = 0 Then
                ods = oh.ExecuteDataSet("select count(*) from dms.kyc_pre_auth where cust_id='" & custid & "' and status_id=1 and change_type = 5")
                If ods.Tables(0).Rows(0)(0) > 0 Then
                    Dim sql1 As String = "update dms.kyc_pre_auth set CUST_PHOTO=:ph, TRA_DT = sysdate, UPLOADED_BRANCH = " & branch_id & ",UPLOADED_USER = " & userID & ", CUST_BRANCH = " & custBranchID & " where cust_id='" & custid & "' and status_id=1 and change_type = 5"

                    Dim parm1(0) As OracleParameter
                    parm1(0) = New OracleParameter
                    parm1(0).ParameterName = "ph"
                    parm1(0).OracleType = OracleType.Blob
                    parm1(0).Direction = ParameterDirection.Input
                    parm1(0).Value = cust_photo
                    oh.ExecuteNonQuery(sql1, parm1)
                    message = "Complete2"
                Else

                    'oh.ExecuteNonQuery("insert into customer_photo (cust_id) values ('" & custid & "')")
                    'Dim sql As String = "update customer_photo set pledge_photo=:ph where cust_id='" & custid & "'"
                    'Modified by Tijo For Customer name and other related approval change
                    Dim sql1 As String = "insert into dms.kyc_pre_auth (cust_id,requested_by,req_dt,STATUS_ID, TRA_DT, change_type,UPLOADED_BRANCH, UPLOADED_USER, CUST_BRANCH) values ('" & custid & "','" & userID & "',sysdate" & ",1,sysdate, 5," & branch_id & "," & userID & ", " & custBranchID & " )"
                    oh.ExecuteNonQuery(sql1)
                    Dim sql As String = "update dms.kyc_pre_auth set CUST_PHOTO=:ph where cust_id='" & custid & "' and status_id = 1 and change_type = 5"
                    Dim parm(0) As OracleParameter
                    parm(0) = New OracleParameter
                    parm(0).ParameterName = "ph"
                    parm(0).OracleType = OracleType.Blob
                    parm(0).Direction = ParameterDirection.Input
                    parm(0).Value = cust_photo
                    oh.ExecuteNonQuery(sql, parm)
                    message = "Complete"
                End If
            Else
                message = "Already another modification request pending for approval..!!"
            End If
        Catch ex As Exception
            message = ex.Message
        End Try
        Return message
    End Function

    <WebMethod()> _
    Public Function AddKycPhotoPreAuth(ByVal custid As String, ByVal kyc_photo() As Byte, ByVal branch_id As Integer, ByVal userID As Integer, ByVal kyc_type As String, ByVal kyc_id As String) As String
        Dim ods As New DataSet
        Dim dt As New DataTable
        Dim message As String = String.Empty
        Try
            'ods = oh.ExecuteDataSet("select count(*) from dms.kyc_pre_auth where cust_id='" & custid & "' and status_id=1")
            'Modified by Tijo For Customer name and other related approval change
            ods = oh.ExecuteDataSet("select count(*) from dms.kyc_pre_auth where cust_id='" & custid & "' and status_id=1 and change_type = 5")
            If ods.Tables(0).Rows(0)(0) > 0 Then
                If Not IsNothing(kyc_photo) Then
                    Dim sql As String = "update dms.kyc_pre_auth set kyc_photo=:ph, uploaded_branch=" & branch_id & ",uploaded_user=" & userID & ", TRA_DT = sysdate where cust_id='" & custid & "' and status_id = 1 and change_type = 5"
                    Dim parm(0) As OracleParameter
                    parm(0) = New OracleParameter
                    parm(0).ParameterName = "ph"
                    parm(0).OracleType = OracleType.Blob
                    parm(0).Direction = ParameterDirection.Input
                    parm(0).Value = kyc_photo

                    oh.ExecuteNonQuery(sql, parm)
                End If
                message = "Your request to modify the kyc is still pending for verification."
                Return message
            Else
                'oh.ExecuteNonQuery("insert into dms.kyc_pre_auth (cust_id,tra_dt,uploaded_branch,uploaded_user,status_id,kyc_type,kyc_id) values ('" & custid & "',sysdate," & branch_id & "," & userID & ",1," & IIf(String.IsNullOrEmpty(kyc_type), "null", kyc_type) & ",'" & kyc_id & "')")
                'Modified by Tijo For Customer name and other related approval change
                Dim sqlins As String = "insert into dms.kyc_pre_auth (cust_id,tra_dt,uploaded_branch,uploaded_user,kyc_type,kyc_id,requested_by,req_dt,STATUS_ID, change_type) values ('" & custid & "',sysdate," & branch_id & "," & userID & "," & IIf(String.IsNullOrEmpty(kyc_type), "null", kyc_type) & ",'" & kyc_id & "','" & userID & "',sysdate" & ",1, 5)"
                oh.ExecuteNonQuery(sqlins)
                If Not IsNothing(kyc_photo) Then
                    Dim sql As String = "update dms.kyc_pre_auth set kyc_photo=:ph where cust_id='" & custid & "' and STATUS_ID=1 and change_type = 5"
                    Dim parm(0) As OracleParameter
                    parm(0) = New OracleParameter
                    parm(0).ParameterName = "ph"
                    parm(0).OracleType = OracleType.Blob
                    parm(0).Direction = ParameterDirection.Input
                    parm(0).Value = kyc_photo
                    oh.ExecuteNonQuery(sql, parm)
                End If
                message = "Complete"
            End If
        Catch ex As Exception
            message = ex.Message
        End Try
        Return message
    End Function

    <WebMethod()> _
    Public Function AddKycPreAuth(ByVal custid As String, ByVal branch_id As Integer, ByVal userID As Integer, ByVal kyc_type As String, ByVal kyc_id As String) As String
        Dim ods As New DataSet
        Dim dt As New DataTable
        Dim message As String = String.Empty
        Try
            ods = oh.ExecuteDataSet("select count(*) from dms.kyc_pre_auth where cust_id='" & custid & "' and status_id=1")
            If ods.Tables(0).Rows(0)(0) > 0 Then
                oh.ExecuteNonQuery("update dms.kyc_pre_auth set kyc_type=" & kyc_type & ",kyc_id='" & kyc_id & "' where cust_id ='" & custid & "' and status_id=1")
            Else
                oh.ExecuteNonQuery("insert into dms.kyc_pre_auth (cust_id,tra_dt,uploaded_branch,uploaded_user,status_id,kyc_type,kyc_id) values ('" & custid & "',sysdate," & branch_id & "," & userID & ",1," & IIf(String.IsNullOrEmpty(kyc_type), "null", kyc_type) & ",'" & kyc_id & "')")
            End If
            message = "Complete"
        Catch ex As Exception
            message = ex.Message
        End Try
        Return message
    End Function
    <WebMethod()> _
    Public Function neft_modi_dtl(ByVal custid As String) As DataSet
        Dim result As DataSet
        result = oh.ExecuteDataSet("select n.cust_id|| '~' ||n.cust_name|| '~' ||n.beneficiary_branch|| '~' ||n.beneficiary_account|| '~' ||n.ifsc_code|| '~' ||c.account_name||'~'||a.bankname||'~'||a.dist_id||'~'||c.acc_type||'~'||n.mobile_number||'~'||a.state_id  from branch_master b,neft_customer_history n,neft_current_account c,neft_bank_mst a,customer d where a.ifsc_code=n.ifsc_code and b.branch_id=n.branch_id and n.verify_status='T' and c.acc_type=n.acc_type and n.cust_id=d.cust_id and n.cust_id='" & custid & "' and n.rowid in (select rowidtochar(max(t.rowid)) from neft_customer_history t where t.cust_id='" & custid & "' and t.verify_status='T')")
        Return result
    End Function
    '-------------NEFT CONFIRM EXCEL UPLOAD-------Added by JOHN on 07-Mar-2013
    <WebMethod()> _
    Public Function ExceltoTable(ByVal data As String, ByVal User As Integer) As String
        'Dim oh As New OracleHelper
        Dim arr(), sql As String
        Dim arr2() As String
        arr = data.Split("#")


        Dim dt1 As New DataTable
        dt1 = oh.ExecuteDataSet("select nvl(max(group_id),0)+1 from NEFT_EXCEL ").Tables(0)
        Dim gp As Integer
        gp = dt1.Rows(0)(0)
        Dim i As Integer
        For i = 0 To arr.Length - 1
            arr2 = arr(i).Split("$")
            sql = "insert into NEFT_EXCEL(group_id,CORPORATIONID,AMOUNT,TRA_DT,USERID ) values(" & gp & ",'" & arr2(0) & "','" & arr2(1) & "',sysdate,'" & User & "')"
            oh.ExecuteNonQuery(sql)
        Next
        Return gp
    End Function
    <WebMethod()> _
    Public Function RetrieveExcel(ByVal group As Integer, ByVal report As Integer) As DataSet
        Dim ds As New DataSet
        Dim sql As String
        If report = 0 Then
            sql = "select t.corporationid,a.amount,a.doc_id,a.cust_id,a.group_id from neft_excel t,neft_master a where t.corporationid=a.send_transid and a.module_id in(5,6) and t.group_id=" & group & " order by t.corporationid"
            'D() 'im oh As New Helper.Oracle.OracleHelper
            ds = oh.ExecuteDataSet(sql)
        Else
            'For Report Purpose
            sql = "select t.sno,t.account_no,a.account_name,t.type,t.sub_account,t.amount,decode(t.transid,null,'0',t.transid) transid,t.userid,decode(to_char(t.tradt),null,'NILL',to_char(t.tradt)) tradt,t.status from manual_excel t,account_profile a where t.account_no=a.account_no and  t.group_id=" & group & " order by t.sno"
            'Dim oh As New Helper.Oracle.OracleHelper
            ds = oh.ExecuteDataSet(sql)
        End If
        Return ds
    End Function
    <WebMethod()> _
    Public Function DeleteExcel(ByVal group As Integer) As String
        Dim msg As String
        'Dim oh As New Helper.Oracle.OracleHelper
        msg = oh.ExecuteNonQuery("delete from NEFT_EXCEL t where t.group_id=" & group & "")
        Return msg
    End Function

    <WebMethod()> _
    Public Function NeftExcelUpload() As String
        'Dim oh1 As New OracleHelper
        Dim EE(0) As OracleClient.OracleParameter
        EE(0) = New OracleParameter("err_stat", OracleType.Number, 1)
        EE(0).Direction = ParameterDirection.Output
        'NeftExcelUpload
        oh.ExecuteNonQuery("NeftExcelUpload", EE)
        Return EE(0).Value
    End Function
    <WebMethod()> _
    Public Function Neft_Excel_upload(ByVal grpid As Integer, ByVal File() As Byte, ByVal UserID As String) As Integer
        Dim dt, dt1, dt2 As New DataTable
        Dim a As Integer
        Dim QUERY As String = ""
        Dim srid As Integer
        'dt2 = oh.ExecuteDataSet("select t.group_id from neft_master t where t.corporate_id='" & copid & "'").Tables(0)
        'grpid = dt2.Rows(0)(0)

        dt1 = oh.ExecuteDataSet("select nvl(max(t.sr_no),0)+1 from dms.neft_excel_upload t").Tables(0)
        srid = dt1.Rows(0)(0)
        oh.ExecuteNonQuery("insert into dms.neft_excel_upload(sr_no,group_id,user_id,tra_dt) values (" & srid & "," & grpid & ",'" & UserID & "',sysdate)")


        QUERY = "update dms.neft_excel_upload p set p.excel=:BlobParameter where p.group_id=" & grpid & " and p.sr_no=" & srid & ""

        Dim parm_coll(0) As OracleParameter
        parm_coll(0) = New OracleParameter("BlobParameter", OracleType.Blob)
        parm_coll(0).Direction = ParameterDirection.Input
        parm_coll(0).Value = File
        a = oh.ExecuteNonQuery(QUERY, parm_coll)
        Return a

    End Function

    <WebMethod()> _
    Public Function GetEmpPostID(ByVal EmpID As String) As Integer
        Dim dt As New DataTable
        dt = oh.ExecuteDataSet("select m.post_id from employee_master m where m.status_id = 1 and m.emp_code = '" & EmpID & "'").Tables(0)
        Return dt.Rows(0)(0)
    End Function

    <WebMethod()> _
    Public Function GetModiStatus(ByVal CustID As String) As Integer
        Dim dt As New DataTable
        Dim str As String = "select count(*) from dms.kyc_pre_auth k where k.cust_id = '" & CustID & "'  and ((k.status_id = 3 and k.change_type <> 6) or (k.status_id = 2 and k.change_type = 6))"
        dt = oh.ExecuteDataSet(str).Tables(0)
        Return dt.Rows(0)(0)
    End Function

    <WebMethod()> _
    Public Function GetRiskMgmtPostID(ByVal EmpID As String) As Integer
        Dim dt As New DataTable
        Dim str As String = "select count(*) from employee_master em where em.emp_code = '" & EmpID & "' and em.status_id=1 and em.department_id =133 and em.post_id <> 69"
        dt = oh.ExecuteDataSet(str).Tables(0)
        Return dt.Rows(0)(0)
    End Function


    <WebMethod()> _
    Public Function neftdtlpaythruExcel(ByVal grpid As Integer, ByVal st As Integer) As DataSet

        'NEFT CONFIRMATION NEFTDTLPAYTHRU

        Dim ds As DataSet
        If st = 2 Then  'NT'
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,to_char(a.send_transid) as corporationid,a.amount,to_char(to_date(a.send_date),'dd/mm/yyyy') as valuedt,'UTIB0000046' as senderifsc,c.ifsc_code beneficiaryifsc,c.beneficiary_account as ben_account,ltrim(c.cust_name, 102) as ben_name,decode(a.typestatus,'NE','NEFT TRANSFER','RT','RTGS TRANSFER') as sender_information,'' as email,'' as emailbody,e.parmtr_value as debaccno,'11' as sen_acc_type,c.acc_type as rec_acc_type from neft_master a, neft_customer c, firm_master d, general_parameter e where a.cust_id = c.cust_id and a.firm_id = d.firm_id and e.parmtr_id = 56 and e.module_id=1 and e.firm_id = a.firm_id and c.verify_status = 'T' and a.send_transid is not null and a.status_id = 0 and a.typestatus <> 'DC' and a.group_id = " & grpid & " and a.upload_dt is null order by paymentidentifier,corporationid ")
        ElseIf st = 4 Then   'DC and a.module_id in (2,5,6,20,88)
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier,to_char(a.send_transid) as corporationid,a.amount,to_char(to_date(a.send_date),'dd/mm/yyyy') as valuedt,c.beneficiary_account as senderifsc,'MANAPPURAM FINANCE  LIMITED' as beneficiaryifsc,'' as ben_account,'' as ben_name,'912020067992955' as sender_information,c.beneficiary_account as email,'' as emailbody,'' as debaccno,'' as sen_acc_type,c.beneficiary_account as rec_acc_type from neft_master a, neft_customer c where a.cust_id = c.cust_id and c.verify_status = 'T' and a.send_transid is not null and to_date(a.send_date) is not null and a.status_id = 0 and a.typestatus = 'DC' and a.group_id = " & grpid & " and a.upload_dt is null /*and a.module_id  in (2,5,6,20,88) not in (1,39,44,90,91,33)*/ order by paymentidentifier,corporationid ")
            ' NEFT PAYTHRU EMPLOYEE SALARY
        End If
        Return ds
    End Function
    <WebMethod()> _
    Public Function CheckingModuleUser(ByVal user As Integer) As DataSet
        Dim oh1 As New Helper.Oracle.OracleHelper
        Dim ds As New DataSet
        ds = oh1.ExecuteDataSet("select count(*)  from module_mst_access t where t.module_id=2 and t.user_id=" & user & " and t.to_dt is null")
        Return ds
    End Function
    '-------------NEFT CONFIRM EXCEL UPLOAD-------Added by JOHN on 07-Mar-2013

    '-------------Restriction For Duplicate Customer ID -------Added by Prasanth on 03-OCT-2013
    <WebMethod()> _
    Public Function CustomerFilter(ByVal key As String, ByVal cname As String, ByVal cphone1 As String, ByVal crelat As String, ByVal chousename As String, ByVal clocality As String, ByVal cpincode As String, ByVal cstreet As String, ByVal cdob As Date, ByVal ckyc As String) As DataSet
        Dim Params(10) As OracleParameter
        Dim DataStr As DataSet
        Params(0) = New OracleParameter("key", OracleType.VarChar, 100)
        Params(0).Value = key
        Params(1) = New OracleParameter("c_name", OracleType.VarChar, 200)
        Params(1).Value = cname
        Params(2) = New OracleParameter("c_phone2", OracleType.VarChar, 100)
        Params(2).Value = cphone1
        Params(3) = New OracleParameter("c_fat_hus", OracleType.VarChar, 100)
        Params(3).Value = crelat
        Params(4) = New OracleParameter("c_house_name", OracleType.VarChar, 100)
        Params(4).Value = chousename
        Params(5) = New OracleParameter("c_locality", OracleType.VarChar, 100)
        Params(5).Value = clocality
        Params(6) = New OracleParameter("c_pin_serial", OracleType.VarChar, 100)
        Params(6).Value = cpincode
        Params(7) = New OracleParameter("c_street", OracleType.VarChar, 100)
        Params(7).Value = cstreet
        Params(8) = New OracleParameter("c_date_of_birth", OracleType.DateTime)
        Params(8).Value = cdob
        Params(9) = New OracleParameter("c_kycid", OracleType.VarChar, 100)
        Params(9).Value = ckyc
        Params(10) = New OracleParameter("as_outresult", OracleType.Cursor)
        Params(10).Direction = ParameterDirection.Output
        DataStr = oh.ExecuteDataSet("plp_customer_filter", Params)
        Return DataStr
    End Function

    Public Sub Srm_Customer_ApprovalMail(ByVal cust As String, ByVal br As String)
        Try
            Dim dt As New DataTable
            Dim srm_email As String
            Dim mbody As String
            Dim subj As String
            Dim cc, tow As String
            Dim str As String = "select rm_mailid,b.BRANCH_NAME,e.email_address from region_master m,branch_detail b,branch_email_address e where m.reg_id=b.reg_id and b.BRANCH_ID = e.branch_id and  b.BRANCH_ID =" & br
            dt = oh.ExecuteDataSet(str).Tables(0)
            srm_email = dt.Rows(0)(0).ToString()
            cc = dt.Rows(0)(2).ToString()
            Dim mh As New MailHelper.MailHelper("formkrisk", "outlookexpress")
            subj = "Customer ID " & cust & " pending for SRM Approval Request"
            mbody = " Hi Sir,<br/><br/> Customer ID " & cust & " from  " & dt.Rows(0)(1).ToString() & " branch is pending for SRM Approval. <br/><br/> This is a System Generated Mail Please do Not Reply."
            mh.SendMail("Formkrisk@in.manappuram.com", srm_email, "", cc, subj, mbody, "")
            mh.SendMail("Formkrisk@in.manappuram.com", "srmrisk@in.manappuram.com", "", "", subj, mbody, "")
        Catch ex As Exception

        End Try
    End Sub

    <WebMethod()> _
    Public Function NEFTDATApaythruGoldCount(ByVal st As Integer) As DataSet
        Dim ds As New DataSet
        If st = 41 Then
            ds = oh.ExecuteDataSet("select e.bank_name,e.bank_id,count(1) cnt from neft_master a, neft_customer c, neft_mana_banks e where a.cust_id = C.cust_id and e.bank_id = c.bank_id and e.isactive=1 and to_date(a.send_date) is null and a.status_id = 1 and a.typestatus = 'DC' and a.module_id = 113 group by e.bank_name,e.bank_id")
        End If
        Return ds
    End Function

    <WebMethod()> _
    Public Function NEFTDATApaythruGold(ByVal st As Integer, ByVal bankid As Integer) As DataSet
        Dim ds As New DataSet
        If st = 41 Then 'GoldLoan Pay thru
            ds = oh.ExecuteDataSet("select a.typestatus paymentidentifier, a.corporate_id as corporationid, a.amount, to_char(to_date(a.send_date), 'dd/mm/yyyy') as valuedt, e.ifsccode as senderifsc, c.ifsc_code beneficiaryifsc, c.beneficiary_account as ben_account, c.cust_name as ben_name,decode(a.typestatus, 'NE', 'NEFT TRANSFER', 'RT', 'RTGS TRANSFER') as sender_information, '' as email, '' as emailbody, e.account_no as debaccno, '11' as sen_acc_type, c.acc_type as rec_acc_type from neft_master a, neft_customer c, neft_mana_banks e where a.cust_id = c.cust_id and e.bank_id=c.bank_id and e.isactive=1 and to_date(a.send_date) is null and a.status_id = 1 and a.typestatus = 'DC' and a.module_id = 113 and e.bank_id=" & bankid & "  order by paymentidentifier")
        End If
        Return ds
    End Function

    <WebMethod()> _
    Public Function PawnBrokerInfo(ByVal as_flag As String, ByVal custID As String, ByVal licno As String, ByVal isudt As Date, ByVal expdt As Date, ByVal bkrltv As Int32, ByVal asset As String, ByVal liab As String, ByVal netw As Int32, ByVal userid As String) As String
        Dim Params(10) As OracleParameter
        Dim DataStr As String = String.Empty
        Params(0) = New OracleParameter("flag", OracleType.VarChar, 10)
        Params(0).Value = as_flag
        Params(1) = New OracleParameter("cust_id", OracleType.VarChar, 16)
        Params(1).Value = custID
        Params(2) = New OracleParameter("license_no", OracleType.VarChar, 50)
        Params(2).Value = licno
        Params(3) = New OracleParameter("is_date", OracleType.DateTime, 100)
        Params(3).Value = CDate(isudt)
        Params(4) = New OracleParameter("ex_date", OracleType.DateTime, 100)
        Params(4).Value = CDate(expdt)
        Params(5) = New OracleParameter("brk_ltv", OracleType.Int32, 8)
        Params(5).Value = bkrltv
        Params(6) = New OracleParameter("asst", OracleType.VarChar, 200)
        Params(6).Value = asset
        Params(7) = New OracleParameter("liab", OracleType.VarChar, 200)
        Params(7).Value = liab
        Params(8) = New OracleParameter("netwh", OracleType.Int32)
        Params(8).Value = netw
        Params(9) = New OracleParameter("userid", OracleType.Int32)
        Params(9).Value = userid
        Params(10) = New OracleParameter("mesg", OracleType.VarChar, 100)
        Params(10).Direction = ParameterDirection.Output
        oh.ExecuteNonQuery("plp_cust_pawnbroker", Params)
        DataStr = Convert.ToString(Params(10).Value)
        Return DataStr
    End Function

    <WebMethod()> _
    Public Function PawnBrokerInfoSelect(ByVal flag As String, ByVal custID As String, ByVal brid As String, ByVal prm1 As String, ByVal prm2 As String, ByVal prm3 As String) As DataTable
        Try
            Dim dt As DataTable
            Dim pr(6) As OracleParameter

            pr(0) = New OracleParameter("as_optflag", OracleType.VarChar)
            pr(0).Value = flag
            pr(0).Direction = ParameterDirection.Input

            pr(1) = New OracleParameter("as_customerid", OracleType.VarChar)
            pr(1).Value = custID
            pr(1).Direction = ParameterDirection.Input

            pr(2) = New OracleParameter("an_branchid", OracleType.VarChar)
            pr(2).Value = brid
            pr(2).Direction = ParameterDirection.Input

            pr(3) = New OracleParameter("param1", OracleType.VarChar)
            pr(3).Value = prm1
            pr(3).Direction = ParameterDirection.Input

            pr(4) = New OracleParameter("param2", OracleType.VarChar)
            pr(4).Value = prm2
            pr(4).Direction = ParameterDirection.Input

            pr(5) = New OracleParameter("param3", OracleType.VarChar)
            pr(5).Value = prm3
            pr(5).Direction = ParameterDirection.Input

            pr(6) = New OracleParameter("as_outresult", OracleType.Cursor)
            pr(6).Direction = ParameterDirection.Output

            dt = oh.ExecuteDataSet("plp_custpawnbroker_select", pr).Tables(0)
            Return dt
        Catch ex As Exception

        End Try
    End Function

    <WebMethod()> _
    Public Function AddAdditionalKycPhoto(ByVal custid As String, ByVal addKyc_photo() As Byte, ByVal idNumber As String) As String
        Dim ods As New DataSet
        Dim dt As New DataTable
        Try
            If Not IsNothing(addKyc_photo) Then
                Dim sql As String
                If idNumber = 1 Then
                    sql = "update dms.pawncustomer_documents set license_image=:ph where customer_id='" & custid & "'"
                Else
                    sql = "update dms.pawncustomer_documents set form_image=:ph where customer_id='" & custid & "'"
                End If
                Dim parm(0) As OracleParameter
                parm(0) = New OracleParameter
                parm(0).ParameterName = "ph"
                parm(0).OracleType = OracleType.Blob
                parm(0).Direction = ParameterDirection.Input
                parm(0).Value = addKyc_photo
                oh.ExecuteNonQuery(sql, parm)
            End If
            Return "Complete"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <WebMethod()> _
    Public Function isBackDtPldge(ByVal brid As String) As Boolean
        Dim dt As New DataTable
        Try
            Dim sql As String
            sql = "select count(*) from pledge_oldconf_mst a,pledge_oldconf_transaction b where a.branch_id= " & brid & " and a.request_id=b.request_id and to_date(a.request_dt)=to_date(sysdate) and b.status <> 0"
            dt = oh.ExecuteDataSet(sql).Tables(0)
            If dt.Rows.Count > 0 AndAlso dt.Rows(0)(0) > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
    <WebMethod()> _
    Public Function GetCustomerRegDetails(ByVal strCustRegId As String) As DataSet
        Try
            Dim oh As New Helper.Oracle.OracleHelper
            Dim DsCustmerInfo As New DataSet
            DsCustmerInfo = oh.ExecuteDataSet("select * from TBL_ONLINE_CUSTOMER_REGISTER where COL_REGID='" & strCustRegId & "'")
            Return DsCustmerInfo
        Catch ex As Exception
            Throw New ApplicationException("GetCustomerRegDetails Service Error:- " & ex.Message)
        End Try
    End Function
    <WebMethod()> _
    Public Function GetOnlineCustomerDetails(ByVal strCustId As String) As DataSet
        Try
            Dim oh As New Helper.Oracle.OracleHelper
            Dim DsCustmerInfo As New DataSet
            DsCustmerInfo = oh.ExecuteDataSet("select count(*) from TBL_ONLINE_GL_CUST_MST t where  t.status_id <>2 and CUST_ID='" & strCustId & "'")
            Return DsCustmerInfo
        Catch ex As Exception
            Throw New ApplicationException("GetOnlineCustomerDetails Service Error:- " & ex.Message)
        End Try
    End Function
    <WebMethod()> _
    Public Function CheckForDuplicateID(ByVal strKycNumber As String, ByVal intKycType As Integer, ByVal intPinSerial As Integer, ByVal intCustype As Integer, ByVal strcustID As String) As Boolean
        Try
            Dim DsKycInfo As New DataSet
            Dim arParms(5) As OracleParameter
            arParms(0) = New OracleParameter("CustID", OracleType.VarChar)
            arParms(0).Value = strcustID
            arParms(1) = New OracleParameter("KycNum", OracleType.VarChar)
            arParms(1).Value = strKycNumber
            arParms(2) = New OracleParameter("KycType", OracleType.Number)
            arParms(2).Value = intKycType
            arParms(3) = New OracleParameter("PinSer", OracleType.Number)
            arParms(3).Value = intPinSerial
            arParms(4) = New OracleParameter("Custtype", OracleType.Number)
            arParms(4).Value = intCustype
            arParms(5) = New OracleParameter("as_outresult", OracleType.Cursor)
            arParms(5).Direction = ParameterDirection.Output
            DsKycInfo = oh.ExecuteDataSet("sp_checkforduplicatekyc", arParms)
            If DsKycInfo.Tables.Count > 0 AndAlso DsKycInfo.Tables(0).Rows.Count > 0 Then
                Return DsKycInfo.Tables(0).Rows(0)("IsDuplicateID")
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
    <WebMethod()> _
    Public Function SendOTP(ByVal strOTP As String, ByVal strMobile As String, ByVal Type As Integer) As String
        Dim strResult As String = String.Empty
        Dim sms As New solution_infini_flag.mana.SMSTool
        Try
            If Type = 0 Then
                sms.Message = "Dear Client, Welcome to Manappuram Family!Your reg. code-" & strOTP & ".You can also visit www.manappuram.com or download our mobile app.for loan tracking& payments"
                sms.mobileNumber = strMobile
                sms.account_id = 3
                sms.ser_flag = 0
                strResult = sms.SendSms()
            Else
                sms.Message = "Dear Client, Welcome to Manappuram Family!Your reg. code-" & strOTP & ".You can also visit www.manappuram.com or download our mobile app.for loan tracking& payments"
                sms.mobileNumber = strMobile
                sms.account_id = 3
                sms.ser_flag = 1
                strResult = sms.SendSms()
            End If
        Catch ex As Exception
            strResult = ex.Message
        End Try
        Return strResult
    End Function
    <WebMethod()> _
    Public Function LoadReferenceDetails() As DataSet
        Dim oh As New Helper.Oracle.OracleHelper
        Dim DsRefDetails As New DataSet
        DsRefDetails = oh.ExecuteDataSet("select relation_id,relation_name From RELATION_DTL where relation_id<=20")
        Return DsRefDetails
    End Function
    <WebMethod()> _
    Public Function BindCustRefDetails(ByVal strCustId As String) As DataSet
        Dim oh As New Helper.Oracle.OracleHelper
        Dim DsRefDetails As New DataSet
        DsRefDetails = oh.ExecuteDataSet("select RELTYPE,REFNAME,MOB from TBL_CUST_REF_DTL where CUST_ID='" & strCustId & "'")
        Return DsRefDetails
    End Function
    <WebMethod()> _
    Public Function SaveRefDetails(ByVal StrFlag As String, ByVal custID As String, ByVal empcode As String, ByVal strMob As String, ByVal strOTP As String, ByVal RefDetails As String) As String
        Dim arParms(6) As OracleParameter
        arParms(0) = New OracleParameter("Flag", OracleType.VarChar)
        arParms(0).Value = StrFlag
        arParms(1) = New OracleParameter("Customer_Id", OracleType.VarChar)
        arParms(1).Value = custID
        arParms(1).Direction = ParameterDirection.Input
        arParms(2) = New OracleParameter("EmpCode", OracleType.VarChar)
        arParms(2).Value = empcode
        arParms(2).Direction = ParameterDirection.Input
        arParms(3) = New OracleParameter("Mobile", OracleType.VarChar)
        arParms(3).Value = strMob
        arParms(3).Direction = ParameterDirection.Input
        arParms(4) = New OracleParameter("OTPVAL", OracleType.VarChar)
        arParms(4).Value = strOTP
        arParms(4).Direction = ParameterDirection.Input
        arParms(5) = New OracleParameter("Ref_Details", OracleType.VarChar)
        arParms(5).Value = RefDetails
        arParms(5).Direction = ParameterDirection.Input
        arParms(6) = New OracleParameter("OutMessage", OracleType.VarChar, 1000)
        arParms(6).Direction = ParameterDirection.Output
        oh.ExecuteNonQuery("SP_SaveCustRefDetails", arParms)
        Return arParms(6).Value
    End Function
    <WebMethod()> _
    Public Function CheckForEmpReference(ByVal strMob As String, ByVal strcustid As String) As String
        Dim arParms(2) As OracleParameter
        Dim strResult As String = String.Empty
        arParms(0) = New OracleParameter("Mobile", OracleType.VarChar)
        arParms(0).Value = strMob
        arParms(0).Direction = ParameterDirection.Input
        arParms(1) = New OracleParameter("strCustomer_ID", OracleType.VarChar)
        arParms(1).Value = strcustid
        arParms(1).Direction = ParameterDirection.Input
        arParms(2) = New OracleParameter("OutMessage", OracleType.VarChar, 1000)
        arParms(2).Direction = ParameterDirection.Output
        oh.ExecuteNonQuery("SP_CheckCust_REF_Details", arParms)
        If Not arParms(2).Value Is DBNull.Value Then
            strResult = arParms(2).Value
        Else
            strResult = String.Empty
        End If
        Return strResult
    End Function

    <WebMethod()> _
    Public Function Vehicle_Finance_Survey(ByVal Customer_ID As String, ByVal Branch_ID As Integer, ByVal Comm_Veh_Ownership As Integer, ByVal Comm_Ownership_Veh_Type As String, ByVal Truck_Req As Integer, ByVal Req_Comm_Veh As Integer, ByVal Req_Comm_Vehicle_Type As String, ByVal Req_Other_Veh As Integer, ByVal Req_Other_Veh_Detail As String) As String
        Dim oh As New Helper.Oracle.OracleHelper
        Dim result As String = ""
        Try
            oh.ExecuteNonQuery("insert into TBL_Vehicle_Finance(CUSTOMER_ID,BRANCH_ID,COMM_VEH_OWNERSHIP,COMM_OWNERSHIP_VEH_TYPE,TRUCK_REQ,REQ_COMM_VEH,REQ_COMM_VEHICLE_TYPE,REQ_OTHER_VEH,REQ_OTHER_VEH_DETAIL)values(to_number(" & Customer_ID & "),to_number(" & Branch_ID & "),to_number(" & Comm_Veh_Ownership & "),'" & Comm_Ownership_Veh_Type & "',to_number(" & Truck_Req & "),to_number(" & Req_Comm_Veh & "),'" & Req_Comm_Vehicle_Type & "',to_number(" & Req_Other_Veh & "),'" & Req_Other_Veh_Detail & "')")

        Catch ex As Exception
            result = ex.Message()
        End Try

        Return result = "Success"

    End Function

    <WebMethod()> _
    Public Function Vehicle_Finance_Surv_Update(ByVal Customer_ID As String, ByVal Branch_ID As Integer, ByVal Comm_Veh_Ownership As Integer, ByVal Comm_Ownership_Veh_Type As String, ByVal Truck_Req As Integer, ByVal Req_Comm_Veh As Integer, ByVal Req_Comm_Vehicle_Type As String, ByVal Req_Other_Veh As Integer, ByVal Req_Other_Veh_Detail As String) As String
        Dim oh As New Helper.Oracle.OracleHelper
        Dim result As String = ""
        Try
            oh.ExecuteNonQuery("UPDATE TBL_Vehicle_Finance set COMM_VEH_OWNERSHIP=to_number(" & Comm_Veh_Ownership & "),COMM_OWNERSHIP_VEH_TYPE='" & Comm_Ownership_Veh_Type & "',TRUCK_REQ=to_number(" & Truck_Req & "),REQ_COMM_VEH=to_number(" & Req_Comm_Veh & "),REQ_COMM_VEHICLE_TYPE='" & Req_Comm_Vehicle_Type & "',REQ_OTHER_VEH=to_number(" & Req_Other_Veh & "),REQ_OTHER_VEH_DETAIL='" & Req_Other_Veh_Detail & "' where CUSTOMER_ID=to_number(" & Customer_ID & ") and BRANCH_ID=to_number(" & Branch_ID & ")")
        Catch ex As Exception
            result = ex.Message()
        End Try
        Return result = "Success"
    End Function
    <WebMethod()> _
    Public Function CustomerEkycVersion_Check(ByVal ipAddress As String, ByVal Systemversion As String) As Integer
        Dim oh As New Helper.Oracle.OracleHelper
        Dim dt As New DataTable
        Dim ret As String
        dt = oh.ExecuteDataSet("select count(*)  from CUST_EKYC_VERSIONCONTROL_NEW where ip_address='" & ipAddress & "' and version  ='" & Systemversion & "'").Tables(0)
        ret = dt.Rows(0)(0)
        Return ret
    End Function
    <WebMethod()> _
    Public Function ChkExistEKYC(ByVal CustID As String) As String
        Dim oh As New Helper.Oracle.OracleHelper
        Dim dt As New DataTable
        Dim ret As String
        dt = oh.ExecuteDataSet("select Idn.Id_Number from identity_dtl Idn where Idn.Identity_Id in (16,505,555)and Idn.Cust_Id = '" + CustID + "'").Tables(0)
        If dt.Rows.Count > 0 Then

            If Not IsDBNull(dt.Rows(0)(0)) Then
                ret = dt.Rows(0)(0).ToString()
            Else
                ret = "0"
            End If
        Else
            ret = "0"
        End If



        Return ret
    End Function

    <WebMethod()> _
    Public Function CustomerVersion_Check(ByVal ipAddress As String, ByVal Systemversion As String) As Integer
        Dim oh As New Helper.Oracle.OracleHelper
        Dim dt As New DataTable
        Dim ret As String
        dt = oh.ExecuteDataSet("select count(*)  from CUSTOMER_VERSIONCONTROL where ip_address='" & ipAddress & "' and version  ='" & Systemversion & "'").Tables(0)
        ret = dt.Rows(0)(0)
        Return ret
    End Function

    <WebMethod()> _
    Public Function GetCustomerBankDetails(ByVal IFSCCODE As String) As DataSet
        Try
            Dim oh As New Helper.Oracle.OracleHelper
            Dim DsCustmerInfo As New DataSet
            DsCustmerInfo = oh.ExecuteDataSet("select t.state_id,t.dist_id,t.IFSC_code from NEFT_BANK_MST t where t.ifsc_code='" & IFSCCODE & "'")
            Return DsCustmerInfo
        Catch ex As Exception
            Throw New ApplicationException("GetCustomerBankDetails Service Error:- " & ex.Message)
        End Try
    End Function

    <WebMethod()> _
    Public Function CheckForMobileDuplication(ByVal strMob As String) As Integer
        Dim oh As New Helper.Oracle.OracleHelper
        Dim dt As New DataTable
        Dim ret As Integer
        dt = oh.ExecuteDataSet("select count(*)  from CUSTOMER where phone2='" & strMob & "' and isactive <> 3").Tables(0)
        ret = dt.Rows(0)(0)
        Return ret
    End Function

    <WebMethod()> _
    Public Function FillReason(ByVal Flg As Integer) As DataSet
        Dim dt As New DataSet
        dt = oh.ExecuteDataSet("select -1, '---- Select ----' from dual union all select t.status_id, t.description from STATUS_MASTER t where t.module_id = 891 and t.option_id = " & Flg & "")
        Return dt
    End Function


    <WebMethod()> _
    Public Function CheckCoMob(ByVal mob As String) As DataSet
        Dim dt As New DataSet
        dt = oh.ExecuteDataSet("select count(*) from (select t.device_no mob from TBL_INST_ASSIGN t  where t.devicetype = 2 union all select m.mobile_no mob  from mobile_master m) a where a.mob = '" & mob & "'")
        Return dt
    End Function

    <WebMethod()> _
    Public Function EmpMobCheck(ByVal mob As String) As String
        Dim dt As New DataTable
        Dim flg As Integer
        dt = oh.ExecuteDataSet("select g.mobile_no, g.emp_code  from emp_greeting_master g, emp_master e  where g.emp_code = e.EMP_CODE and e.STATUS_ID = 1 and g.mobile_no = '" & mob & "'").Tables(0)
        If dt.Rows.Count > 0 Then
            flg = dt.Rows(0)(1).ToString()
        Else
            flg = "0"
        End If
        Return flg
    End Function

    <WebMethod()> _
    Public Function ChkExistOtpCust(ByVal mob As String) As String
        Dim dt, dt1 As New DataTable
        Dim flg As String
        dt = oh.ExecuteDataSet("select count(*)  from TBL_CUST_OTP_DETAILS t, customer c where t.cust_id = c.cust_id   and c.isactive in (1,0,null)  and t.otp is not null   and c.phone2 = t.mob   and c.phone2 = '" & mob & "'").Tables(0)
        dt1 = oh.ExecuteDataSet("select count(*) from customer c where c.isactive in (1,0,null) and c.phone2 = '" & mob & "'").Tables(0)
        If dt.Rows(0)(0) > 0 Or dt1.Rows(0)(0) >= 2 Then
            flg = "1"
        Else
            flg = "0"
        End If
        Return flg
    End Function

    <WebMethod()> _
    Public Function GetOnlineCustomerMobile(ByVal CustMob As String) As DataSet
        Try
            Dim oh As New Helper.Oracle.OracleHelper
            Dim DsCustmerInfo As New DataSet
            DsCustmerInfo = oh.ExecuteDataSet("select count(*)  from TBL_ONLINE_GL_CUST_MST t, tbl_online_customer_details c where t.status_id in (1, 0)   and t.cust_id = c.cust_id   and c.ph1 = '" & CustMob & "'")
            Return DsCustmerInfo
        Catch ex As Exception
            Throw New ApplicationException("GetOnlineCustomerMobile Service Error:- " & ex.Message)
        End Try
    End Function

    '<WebMethod()> _
    '           Public Function PanDupChk(ByVal PanNo As String) As DataSet
    '    Try
    '        Dim oh As New Helper.Oracle.OracleHelper
    '        Dim DsCustmerInfo As New DataSet
    '        DsCustmerInfo = oh.ExecuteDataSet("select a.aa + b.bb from (select count(*) aa  from DMS.DEPOSIT_PAN_DETAIL t, mana0809.customer c where c.cust_id = t.cust_id   and c.isactive in (1, null, 0, 5)   and t.pan = '" & PanNo & "') a, (select count(*) bb  from IDENTITY_DTL t, mana0809.customer c where identity_id = 14   and c.cust_id = t.cust_id   and c.isactive in (1, null, 0, 5)   and t.id_number = '" & PanNo & "') b ")
    '        Return DsCustmerInfo
    '    Catch ex As Exception
    '        Throw New ApplicationException("PanDupChk Service Error:- " & ex.Message)
    '    End Try
    'End Function

    <WebMethod()> _
    Public Function PanDupChkModi(ByVal PanNo As String, ByVal CustId As String) As DataSet
        Try
            Dim oh As New Helper.Oracle.OracleHelper
            Dim DsCustmerInfo As New DataSet
            DsCustmerInfo = oh.ExecuteDataSet("select count(*) aa from DMS.DEPOSIT_PAN_DETAIL t, mana0809.customer c  where c.cust_id = t.cust_id  and c.isactive in (1, null, 0, 5)   and t.pan = '" & PanNo & "'   and c.cust_id <> '" & CustId & "'")
            Return DsCustmerInfo
        Catch ex As Exception
            Throw New ApplicationException("PanDupChk Service Error:- " & ex.Message)
        End Try
    End Function
    <WebMethod()> _
    Public Function Matching_ExtCust_KycVerify(ByVal Customer_ID As String, ByVal uu_id As String, ByVal rrn As String) As String
        Dim oh As New Helper.Oracle.OracleHelper
        Dim result As String = ""
        Dim dt As New DataTable
        Dim ret As String
        Try

            dt = oh.ExecuteDataSet("select count(*) from tbl_ekyc_log TEL where TEL.uuid ='" + uu_id + "' and TEL.rrn_no = '" & rrn & "'  and TEL.status ='Y' ").Tables(0)
            ret = dt.Rows(0)(0)
            If ret = 0 Then
                result = "E-kyc is not correctly verified"
            Else
                dt = oh.ExecuteDataSet("select count(*) from tbl_ekyc_log t where t.uuid = '" & uu_id & "' and t.cust_id is not null").Tables(0)
                If dt.Rows(0)(0) = 0 Then
                    oh.ExecuteNonQuery("update tbl_ekyc_log t set t.cust_id ='" + Customer_ID + "', t.custid_status = 4 where t.uuid = '" + uu_id + "' and t.rrn_no = '" & rrn & "' and t.status ='Y'")
                    oh.ExecuteNonQuery("update tbl_customer_master t set t.uuid = '" + uu_id + "', t.rrn_no = '" & rrn & "', t.cust_source = 8 where t.cust_id = '" + Customer_ID + "'")
                    'dt = oh.ExecuteDataSet("select count(*) from identity_dtl i where i.cust_id = '" & Customer_ID & "' and i.id_number = '" & uu_id & "'").Tables(0)
                    'If dt.Rows(0)(0) > 0 Then
                    '    Try
                    '        Dim arParms(2) As OracleParameter
                    '        Dim strResult As String = String.Empty
                    '        arParms(0) = New OracleParameter("Customer_ID", OracleType.VarChar)
                    '        arParms(0).Value = Customer_ID
                    '        arParms(0).Direction = ParameterDirection.Input
                    '        arParms(1) = New OracleParameter("uu_id", OracleType.VarChar)
                    '        arParms(1).Value = uu_id
                    '        arParms(1).Direction = ParameterDirection.Input
                    '        arParms(2) = New OracleParameter("OutMessage", OracleType.VarChar, 1000)
                    '        arParms(2).Direction = ParameterDirection.Output
                    '        oh.ExecuteNonQuery("proc_ekyc_dtl_update", arParms)
                    '        If Not arParms(2).Value Is DBNull.Value Then
                    '            strResult = arParms(2).Value
                    '        Else
                    '            strResult = String.Empty
                    '        End If
                    '        Return strResult
                    '    Catch ex As Exception
                    '        Return ex.Message.ToString()
                    '    End Try
                    'End If
                    result = "E-kyc successfully verified"
                Else
                    result = "This EKYC already mapped with another customer ID."
                End If
            End If
        Catch ex As Exception
            result = ex.Message()
        End Try

        Return result

    End Function
    <WebMethod()> _
    Public Function ExtCust_KycVerifyDetails(ByVal Verify_ID As String, ByVal CustAadhStatus As String) As DataTable
        Dim oh As New Helper.Oracle.OracleHelper
        Dim result As String = ""
        Dim dt As New DataTable
        'Dim ret As String
        Try
            If CustAadhStatus = "CUSTOMERID" Then
                dt = oh.ExecuteDataSet("SELECT E.UUID, MAX(E.VERIFIED_DT)FROM TBL_EKYC_LOG E WHERE E.STATUS = 'Y' AND E.EKYC_MODE = 'KUA' AND E.Cust_Id = '" + Verify_ID + "' GROUP BY E.UUID").Tables(0)
            ElseIf CustAadhStatus = "AADHAARID" Then
                dt = oh.ExecuteDataSet("SELECT E.UUID, MAX(E.VERIFIED_DT),e.cust_id FROM TBL_EKYC_LOG E WHERE E.STATUS = 'Y' AND E.EKYC_MODE = 'KUA' AND E.UUID = '" + Verify_ID + "' GROUP BY E.UUID, e.cust_id").Tables(0)
            End If

            'ret = dt.Rows(0)(0)
            'If ret = 0 Then
            '    result = "This Kyc is not correctly verified"

            'Else

            '    oh.ExecuteNonQuery("update tbl_ekyc_log t set t.cust_id ='" + Customer_ID + "' where t.aadhaar_no = '" + Aadhaar_ID + "' and t.status ='Y'")
            '    result = "Success"

            'End If


        Catch ex As Exception
            'result = ex.Message()
        End Try

        Return dt

    End Function
    <WebMethod()> _
    Public Function MailDupChk(ByVal MailId As String) As DataSet
        Try
            Dim oh As New Helper.Oracle.OracleHelper
            Dim DsCustmerInfo As New DataSet
            DsCustmerInfo = oh.ExecuteDataSet("select count(*) from  mana0809.customer_detail c where c.email_id = '" & MailId & "'")
            Return DsCustmerInfo
        Catch ex As Exception
            Throw New ApplicationException("MailDupChk Service Error:- " & ex.Message)
        End Try
    End Function

    <WebMethod()> _
    Public Function UpdateAadhaar(ByVal branch As String, ByVal Aadhaar As String, ByVal V_Mode As String, ByVal e_Mode As String, ByVal e_Code As String, ByVal e_Txn As String, ByVal e_Ts As String, ByVal Retval As String, ByVal Photo As Byte(), ByVal e_Status As String, ByVal User As String, ByVal custDtl As String, ByVal rrn_n As String) As String
        Dim serRet As String
        Try
            Dim sql, SQL1, sql2, Auid As String
            Dim dt As DataTable = New DataTable()
            Dim dt1 As DataTable = New DataTable()
            Dim dt2 As DataTable = New DataTable()
            Dim parm_coll As OracleParameter() = New OracleParameter(12) {}
            parm_coll(0) = New OracleParameter("branch", OracleType.Number, 5)
            parm_coll(0).Value = branch
            parm_coll(0).Direction = ParameterDirection.Input
            parm_coll(1) = New OracleParameter("aadhar", OracleType.VarChar, 20)
            parm_coll(1).Value = Aadhaar
            parm_coll(1).Direction = ParameterDirection.Input
            parm_coll(2) = New OracleParameter("V_Mode", OracleType.VarChar, 50)
            parm_coll(2).Value = V_Mode
            parm_coll(2).Direction = ParameterDirection.Input
            parm_coll(3) = New OracleParameter("e_Mode", OracleType.VarChar, 50)
            parm_coll(3).Value = e_Mode
            parm_coll(3).Direction = ParameterDirection.Input
            parm_coll(4) = New OracleParameter("e_Code", OracleType.VarChar, 100)
            parm_coll(4).Value = e_Code
            parm_coll(4).Direction = ParameterDirection.Input
            parm_coll(5) = New OracleParameter("e_Txn", OracleType.VarChar, 50)
            parm_coll(5).Value = e_Txn
            parm_coll(5).Direction = ParameterDirection.Input
            parm_coll(6) = New OracleParameter("e_Ts", OracleType.VarChar, 50)
            parm_coll(6).Value = e_Ts
            parm_coll(6).Direction = ParameterDirection.Input
            parm_coll(7) = New OracleParameter("e_Status", OracleType.VarChar, 50)
            parm_coll(7).Value = e_Status
            parm_coll(7).Direction = ParameterDirection.Input
            parm_coll(8) = New OracleParameter("User_id", OracleType.VarChar, 50)
            parm_coll(8).Value = User
            parm_coll(8).Direction = ParameterDirection.Input
            parm_coll(9) = New OracleParameter("Auid", OracleType.VarChar, 100)
            parm_coll(9).Direction = ParameterDirection.Output
            parm_coll(10) = New OracleParameter("custDtl", OracleType.VarChar, 4000)
            parm_coll(10).Value = custDtl
            parm_coll(10).Direction = ParameterDirection.Input
            parm_coll(11) = New OracleParameter("ErrMessage", OracleType.VarChar, 1000)
            parm_coll(11).Direction = ParameterDirection.Output
            parm_coll(12) = New OracleParameter("rrn_n", OracleType.VarChar, 500)
            parm_coll(12).Value = rrn_n
            oh.ExecuteNonQuery("proc_ekyc_customer", parm_coll)
            Auid = Convert.ToString(parm_coll(9).Value)
            Dim separators As String() = {":"}
            Dim custId As String() = Convert.ToString(parm_coll(11).Value).Split(separators, StringSplitOptions.RemoveEmptyEntries)
            SQL1 = "UPDATE dms.TBL_EKYC_DTL SET tra_dt=sysdate, RESPONSE=:SBP WHERE ekyc_id='" & Auid & "'"
            Dim parm As OracleParameter() = New OracleParameter(0) {}
            parm(0) = New OracleParameter()
            parm(0).ParameterName = "SBP"
            parm(0).OracleType = OracleType.Clob
            parm(0).Direction = ParameterDirection.Input
            parm(0).Value = Retval
            oh.ExecuteNonQuery(SQL1, parm)
            If e_Mode = "KUA" AndAlso e_Status = "Y" Then
                sql2 = "UPDATE dms.TBL_EKYC_DTL SET PHOTO=:SBP WHERE ekyc_id='" & Auid & "'"
                Dim parm1 As OracleParameter() = New OracleParameter(0) {}
                parm1(0) = New OracleParameter()
                parm1(0).ParameterName = "SBP"
                parm1(0).OracleType = OracleType.Blob
                parm1(0).Direction = ParameterDirection.Input
                parm1(0).Value = Photo
                oh.ExecuteNonQuery(sql2, parm1)
                'If custId(1) <> "0" Then
                '    UpdatePhoto(custId(1).Trim(), Photo, User, branch)
                'End If
            End If

            If custId(0) = "Customer ID Recommended for SRM/RM Approval" Then
                Srm_Customer_ApprovalMail(custId(1).Trim(), branch)
            End If

            serRet = Convert.ToString(parm_coll(11).Value)
        Catch ex As Exception
            serRet = ex.Message
        End Try

        Return serRet
    End Function

    <WebMethod()> _
    Public Function Rrn_Gen() As String
        Try
            Dim oh As New Helper.Oracle.OracleHelper
            Dim DsCustmerInfo As New DataSet
            DsCustmerInfo = oh.ExecuteDataSet("select 'KUA' || SEQ_AADHAAR_ID.Nextval from dual")
            Return DsCustmerInfo.Tables(0).Rows(0)(0)
        Catch ex As Exception
            Throw New ApplicationException("Rrn_Gen Service Error:- " & ex.Message)
        End Try
    End Function

    <WebMethod()> _
    Public Function CheckExist(ByVal uuid As String) As DataSet
        Dim oh As New Helper.Oracle.OracleHelper
        Dim ods As New DataSet
        Dim oddt As New DataTable
        Try
            ods = oh.ExecuteDataSet("select t.cust_id from TBL_EKYC_LOG t where t.cust_id is not null and t.uuid = '" & uuid & "'")
        Catch ex As Exception

        End Try
        Return ods
    End Function

    <WebMethod()> _
    Public Function BaCheck(ByVal mob As String) As Integer
        Try
            Dim oh As New Helper.Oracle.OracleHelper
            Dim DsCustmerInfo As New DataSet
            DsCustmerInfo = oh.ExecuteDataSet("select count(*) from EMP_REFERENCE t where ref_phn1 = '" & mob & "' and t.ref_dt <= (sysdate - 12 / 24) and t.ba_code is not null")
            Return DsCustmerInfo.Tables(0).Rows(0)(0)
        Catch ex As Exception
            Throw New ApplicationException("BaCheck Service Error:- " & ex.Message)
        End Try
    End Function

    <WebMethod()> _
    Public Function custinsrt(ByVal cbid As String, ByVal kycphoto() As Byte) As String
        Dim sql As String
        Dim dt, dt1 As New DataTable
        dt = oh.ExecuteDataSet("select count(*) from dms.TBL_EKYC_consent where CUSTOMER_ID = '" & cbid & "'").Tables(0)
        dt1 = oh.ExecuteDataSet("select count(*) from tbl_ekyc_log where cust_id = '" & cbid & "' and cust_id is not null").Tables(0)
        If dt.Rows(0)(0) = 0 And dt1.Rows(0)(0) > 0 Then
            sql = "insert into dms.TBL_EKYC_consent (CUSTOMER_ID,CONSENT) values (:customerid1,:ph)"
            Dim conf_par(1) As OracleParameter
            conf_par(0) = New OracleParameter("customerid1", OracleType.VarChar, 50)
            conf_par(0).Direction = ParameterDirection.Input
            conf_par(0).Value = cbid

            conf_par(1) = New OracleParameter()
            conf_par(1).ParameterName = "ph"
            conf_par(1).OracleType = OracleType.Blob
            conf_par(1).Direction = ParameterDirection.Input
            conf_par(1).Value = kycphoto
            oh.ExecuteNonQuery(sql, conf_par)
            ' oh.ExecuteNonQuery(sql, conf_par)
            Return "success"
        ElseIf dt1.Rows(0)(0) = 0 Then
            Return "Not an EKYC authenticated customer."
        Else
            Return "Consent already uploaded."
        End If
    End Function

    <WebMethod()> _
            Public Function CKYC_FILL(ByVal custid As String) As DataSet
        Dim ds As New DataSet
        Dim dt As New DataTable
        Try
            dt = oh.ExecuteDataSet("select t.status_id, t.description from STATUS_MASTER t where module_id = 899 and t.option_id = 1 and t.order_by = 1").Tables(0).Copy
            dt.TableName = "NAMEPFX"
            ds.Tables.Add(dt)

            dt = oh.ExecuteDataSet("select t.status_id, t.description from STATUS_MASTER t where module_id = 899 and t.option_id = 2 and t.order_by = 1").Tables(0).Copy
            dt.TableName = "FATSPOUSE"
            ds.Tables.Add(dt)

            dt = oh.ExecuteDataSet("select t.status_id, t.description from STATUS_MASTER t where module_id = 899 and t.option_id = 1 and t.status_id in (1,4) and t.order_by = 1").Tables(0).Copy
            dt.TableName = "MALEPFX"
            ds.Tables.Add(dt)

            dt = oh.ExecuteDataSet("select t.status_id, t.description from STATUS_MASTER t where module_id = 899 and t.option_id = 1 and t.status_id in (2,3,4) and t.order_by = 1").Tables(0).Copy
            dt.TableName = "FEMALEPFX"
            ds.Tables.Add(dt)

            dt = oh.ExecuteDataSet("select t.status_id, t.description from STATUS_MASTER t where module_id = 899 and t.option_id = 1 and t.status_id in (2,3,4) and t.order_by = 1").Tables(0).Copy
            dt.TableName = "SPOUSEPFX"
            ds.Tables.Add(dt)

            dt = oh.ExecuteDataSet("select country_id,country_name from country_dtl  order by country_id ").Tables(0).Copy
            dt.TableName = "country_dtl"
            ds.Tables.Add(dt)

            dt = oh.ExecuteDataSet("select country_id,country_name from country_dtl  order by country_id ").Tables(0).Copy
            dt.TableName = "country_dtl1"
            ds.Tables.Add(dt)

            dt = oh.ExecuteDataSet("select t.status_id, t.description from STATUS_MASTER t where module_id = 899 and t.option_id = 3 and t.order_by = 1").Tables(0).Copy
            dt.TableName = "ADDRTYP"
            ds.Tables.Add(dt)

            dt = oh.ExecuteDataSet("select t.resident, t.fathus_pre, c.country_id from CUSTOMER_detail t, customer c where t.cust_id = '" & custid & "' and c.cust_id = t.cust_id").Tables(0).Copy
            dt.TableName = "CUSTDTL"
            ds.Tables.Add(dt)

            dt = oh.ExecuteDataSet("select CUST_ID, NAME_PFX, FIRST_NAME, MIDDLE_NAME, LAST_NAME, FAT_PFX, FAT_F_NAME, FAT_M_NAME, FAT_L_NAME, FAT_SPOUSE_FLG, MOTHER_PFX, MOTH_F_NAME, MOTH_M_NAME, MOTH_L_NAME, JURIS_RESID, TAX_ID_NUM, BIRTH_COUNTRY, ADDR_TYPE, USER_ID, TRA_DT, STATUS_ID from TBL_CUST_CKYC_DTL t where t.cust_id = '" & custid & "'").Tables(0).Copy
            dt.TableName = "CKYCDTL"
            ds.Tables.Add(dt)
        Catch ex As Exception
        End Try
        Return ds
    End Function

    <WebMethod()> _
            Public Function AddCkycDtl(ByVal CustId As String, ByVal CustPfx As Integer, ByVal CustFName As String, ByVal CustMName As String, ByVal CustLName As String, ByVal FatSpouse As Integer, ByVal FatPfx As Integer, ByVal FatFname As String, ByVal FatMname As String, ByVal FatLname As String, ByVal MotPfx As Integer, ByVal MotFname As String, ByVal MotMname As String, ByVal MotLname As String, ByVal JurisRes As Integer, ByVal TaxId As String, ByVal BirCtry As Integer, ByVal AddrTyp As Integer, ByVal UserId As String, ByVal UserFlg As Integer) As String
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim strResult As String = String.Empty
        Try
            Dim arParms(20) As OracleParameter

            arParms(0) = New OracleParameter("CustId", OracleType.VarChar)
            arParms(0).Value = CustId
            arParms(0).Direction = ParameterDirection.Input

            arParms(1) = New OracleParameter("CustPfx", OracleType.Number)
            arParms(1).Value = CustPfx
            arParms(1).Direction = ParameterDirection.Input

            arParms(2) = New OracleParameter("CustFName", OracleType.VarChar)
            arParms(2).Value = CustFName
            arParms(2).Direction = ParameterDirection.Input

            arParms(3) = New OracleParameter("CustMName", OracleType.VarChar)
            arParms(3).Value = CustMName
            arParms(3).Direction = ParameterDirection.Input

            arParms(4) = New OracleParameter("CustLName", OracleType.VarChar)
            arParms(4).Value = CustLName
            arParms(4).Direction = ParameterDirection.Input

            arParms(5) = New OracleParameter("FatSpouse", OracleType.Number)
            arParms(5).Value = FatSpouse
            arParms(5).Direction = ParameterDirection.Input

            arParms(6) = New OracleParameter("FatPfx", OracleType.Number)
            arParms(6).Value = FatPfx
            arParms(6).Direction = ParameterDirection.Input

            arParms(7) = New OracleParameter("FatFname", OracleType.VarChar)
            arParms(7).Value = FatFname
            arParms(7).Direction = ParameterDirection.Input

            arParms(8) = New OracleParameter("FatMname", OracleType.VarChar)
            arParms(8).Value = FatMname
            arParms(8).Direction = ParameterDirection.Input

            arParms(9) = New OracleParameter("FatLname", OracleType.VarChar)
            arParms(9).Value = FatLname
            arParms(9).Direction = ParameterDirection.Input

            arParms(10) = New OracleParameter("MotPfx", OracleType.Number)
            arParms(10).Value = MotPfx
            arParms(10).Direction = ParameterDirection.Input

            arParms(11) = New OracleParameter("MotFname", OracleType.VarChar)
            arParms(11).Value = MotFname
            arParms(11).Direction = ParameterDirection.Input

            arParms(12) = New OracleParameter("MotMname", OracleType.VarChar)
            arParms(12).Value = MotMname
            arParms(12).Direction = ParameterDirection.Input

            arParms(13) = New OracleParameter("MotLname", OracleType.VarChar)
            arParms(13).Value = MotLname
            arParms(13).Direction = ParameterDirection.Input

            arParms(14) = New OracleParameter("JurisRes", OracleType.Number)
            arParms(14).Value = JurisRes
            arParms(14).Direction = ParameterDirection.Input

            arParms(15) = New OracleParameter("TaxId", OracleType.VarChar)
            arParms(15).Value = TaxId
            arParms(15).Direction = ParameterDirection.Input

            arParms(16) = New OracleParameter("BirCtry", OracleType.Number)
            arParms(16).Value = BirCtry
            arParms(16).Direction = ParameterDirection.Input

            arParms(17) = New OracleParameter("AddrTyp", OracleType.VarChar)
            arParms(17).Value = AddrTyp
            arParms(17).Direction = ParameterDirection.Input

            arParms(18) = New OracleParameter("UserId", OracleType.Number)
            arParms(18).Value = UserId
            arParms(18).Direction = ParameterDirection.Input

            arParms(19) = New OracleParameter("UserFlg", OracleType.Number)
            arParms(19).Value = UserFlg
            arParms(19).Direction = ParameterDirection.Input

            arParms(20) = New OracleParameter("OutMessage", OracleType.VarChar, 1000)
            arParms(20).Direction = ParameterDirection.Output

            oh.ExecuteNonQuery("proc_add_ckyc_dtl", arParms)
            If Not arParms(20).Value Is DBNull.Value Then
                strResult = arParms(20).Value
            Else
                strResult = String.Empty
            End If
            Return strResult
        Catch ex As Exception
        End Try
        Return strResult
    End Function

    <WebMethod()> _
    Public Function ChkBranchExist(ByVal branchID As String) As String
        Dim oh As New Helper.Oracle.OracleHelper
        Dim dt As New DataTable
        Dim ret As String
        dt = oh.ExecuteDataSet("select count(*) from TBL_EKYC_BRANCHES t where branch_id = " + branchID + " or t.branch_id = 0").Tables(0)
        If dt.Rows(0)(0) > 0 Then
            ret = "1"
        Else
            ret = "0"
        End If
        Return ret
    End Function

    <WebMethod()> _
    Public Function ChkUuidExist(ByVal uu_id As String) As DataSet
        Dim oh As New Helper.Oracle.OracleHelper
        Dim dt As New DataSet
        dt = oh.ExecuteDataSet("SELECT  LISTAGG(i.cust_id, ', ') WITHIN GROUP (ORDER BY i.cust_id) AS cust_id FROM   (select t.cust_id  from identity_dtl t  where t.id_number = '" & uu_id & "' union select u.cust_id   from tbl_cust_uuid u  where u.uuid = '" & uu_id & "') i")
        Return dt
    End Function

    <WebMethod()> _
    Public Function CustSearchLog(ByVal brid As String, ByVal userid As Integer, ByVal Typ As Integer, ByVal SerVal As String) As String
        Try
            Dim p(3) As OracleParameter

            p(0) = New OracleParameter("BrId", OracleType.Number)
            p(0).Value = brid

            p(1) = New OracleParameter("UserId", OracleType.Number)
            p(1).Value = userid

            p(2) = New OracleParameter("Typ", OracleType.Number)
            p(2).Value = Typ

            p(3) = New OracleParameter("SerVal", OracleType.VarChar, 150)
            p(3).Value = SerVal

            Dim res = oh.ExecuteNonQuery("proc_cust_search_log", p)

            Return res
        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
        End Try
    End Function

End Class


