Imports System.Data.SqlClient
Imports System.Net
Imports System.Web.Http

Public Class ValuesController
    Inherits ApiController

    Private connStr As String = System.Configuration.ConfigurationManager.ConnectionStrings(1).ConnectionString

    <HttpGet>
    <Route("api/Category")>
    Public Function GetValues() As IHttpActionResult
        Return Ok(FetchNeedsFromDb("Select * from ProppyCaresCategory Where Status = 1"))
    End Function

    ' GET api/values/5
    <HttpGet>
    <Route("api/Category/{term}")>
    Public Function GetValue(ByVal term As String) As IHttpActionResult
        Return Ok(FetchNeedsFromDb("Select * from ProppyCaresCategory Where Status = 1 and Description like '%" + term + "%'"))
    End Function

    ' POST api/values
    Public Sub PostValue(<FromBody()> ByVal value As String)

    End Sub

    ' PUT api/values/5
    Public Sub PutValue(ByVal id As Integer, <FromBody()> ByVal value As String)

    End Sub

    ' DELETE api/values/5
    Public Sub DeleteValue(ByVal id As Integer)

    End Sub

    Private Function FetchNeedsFromDb(ByVal query As String) As List(Of CategoryModel)
        Dim needs As List(Of CategoryModel) = New List(Of CategoryModel)()

        Using Con As SqlConnection = New SqlConnection(connStr)
            Con.Open()

            Using cmd As SqlCommand = New SqlCommand(query, Con)
                Dim sdr As SqlDataReader = cmd.ExecuteReader()

                While sdr.Read()
                    needs.Add(New CategoryModel() With {
                        .ID = Long.Parse(sdr.GetValue(0).ToString()),
                        .Code = sdr.GetValue(CInt(1)).ToString(),
                        .Description = sdr.GetValue(CInt(2)).ToString(),
                        .Description_ZH = sdr.GetValue(CInt(3)).ToString(),
                        .SeqNo = sdr.GetValue(CInt(4)).ToString(),
                        .ImageURL = sdr.GetValue(CInt(5)).ToString(),
                        .Status = Integer.Parse(sdr.GetValue(CInt(6)).ToString()),
                        .ParentID = Integer.Parse(sdr.GetValue(CInt(7)).ToString())
                    })
                End While
            End Using
        End Using

        Return needs
    End Function

End Class
