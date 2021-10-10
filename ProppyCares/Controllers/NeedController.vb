Imports System.Data.SqlClient
Imports System.Net
Imports System.Web.Http
Imports ProppyCares.ProppyAPI.Authorizations

Namespace Controllers
    Public Class NeedController
        Inherits ApiController

        Private connStr As String = System.Configuration.ConfigurationManager.ConnectionStrings(1).ConnectionString

        <HttpGet>
        <Route("api/Need/GetActiveNeeds")>
        Public Function GetActiveNeeds() As IHttpActionResult
            Return Ok(GetAllActiveNeeds())
        End Function

        <HttpGet>
        <Route("api/Need/GetUserNeeds/{UserId}")>
        Public Function GetUserNeeds(ByVal UserId As String) As IHttpActionResult
            Return Ok(GetNeedsByUser(UserId))
        End Function

        <HttpGet>
        <Route("api/Need/GetUserDeals/{UserId}")>
        Public Function GetUserDeals(ByVal UserId As String) As IHttpActionResult
            Return Ok(GetGrabbedUserDeals(UserId))
        End Function

        <HttpPost>
        <Route("api/Need")>
        Public Function PostValue(<FromBody()> ByVal needModel As NeedPostModel) As IHttpActionResult
            Dim Res As Integer = CreateNeed(needModel)
            If Res > 0 Then
                Return Ok()
            Else
                Return BadRequest()
            End If
        End Function

        ' PUT: api/Need/5
        Public Sub PutValue(ByVal id As Integer, <FromBody()> ByVal value As String)

        End Sub

        ' DELETE: api/Need/5
        Public Sub DeleteValue(ByVal id As Integer)

        End Sub

        Public Function CreateNeed(ByVal needModel As NeedPostModel) As Integer
            Dim result As Integer = 0

            Using sqlConnection As SqlConnection = New SqlConnection(connStr)
                sqlConnection.Open()

                Using cmd As SqlCommand = New SqlCommand("INSERT INTO [dbo].[ProppyCaresNeeds]
                                                           ([BizUnit]
                                                           ,[NeedsID]
                                                           ,[CategoryCode]
                                                           ,[Country]
                                                           ,[State]
                                                           ,[District]
                                                           ,[Area]
                                                           ,[SubArea]
                                                           ,[Street]
                                                           ,[Postcode]
                                                           ,[Building]
                                                           ,[Type]
                                                           ,[LevelNo]
                                                           ,[UnitNo]
                                                           ,[Notes]
                                                           ,[PreferredDay]
                                                           ,[PreferredTimeFrom]
                                                           ,[PreferredTimeTo]
                                                           ,[CreatedBy]
                                                           ,[CreatedAt]
                                                           ,[UpdatedBy]
                                                           ,[LastUpdated]
                                                           ,[Status]
                                                           ,[ImagePathOne]
                                                           ,[ImagePathTwo])
                                                     VALUES
                                                           (@BizUnit,
                                                            @NeedsID,
                                                            @CategoryCode,
                                                            @Country,
                                                            @State,
                                                            @District,
                                                            @Area,
                                                            @SubArea,
                                                            @Street,
                                                            @Postcode,
                                                            @Building,
                                                            @Type, 
                                                            @LevelNo,
                                                            @UnitNo, 
                                                            @Notes, 
                                                            @PreferredDay, 
                                                            @PreferredTimeFrom,
                                                            @PreferredTimeTo,
                                                            @CreatedBy,
                                                            @CreatedAt,
                                                            @UpdatedBy,
                                                            @LastUpdated,
                                                            @Status,
                                                            @ImagePathOne,
                                                            @ImagePathTwo )", sqlConnection)
                    cmd.Parameters.AddWithValue("@BizUnit", needModel.BizUnit)
                    cmd.Parameters.AddWithValue("@NeedsID", SharedServices.GenerateUserId(TableConstants.ProppyCaresNeeds))
                    cmd.Parameters.AddWithValue("@CategoryCode", needModel.CategoryCode)
                    cmd.Parameters.AddWithValue("@Country", needModel.Country)
                    cmd.Parameters.AddWithValue("@State", needModel.State)
                    cmd.Parameters.AddWithValue("@District", needModel.District)
                    cmd.Parameters.AddWithValue("@Area", needModel.Area)
                    cmd.Parameters.AddWithValue("@SubArea", needModel.SubArea)
                    cmd.Parameters.AddWithValue("@Street", needModel.Street)
                    cmd.Parameters.AddWithValue("@Postcode", needModel.Postcode)
                    cmd.Parameters.AddWithValue("@Building", needModel.Building)
                    cmd.Parameters.AddWithValue("@Type", needModel.Type)
                    cmd.Parameters.AddWithValue("@LevelNo", needModel.LevelNo)
                    cmd.Parameters.AddWithValue("@UnitNo", needModel.UnitNo)
                    cmd.Parameters.AddWithValue("@Notes", needModel.Notes)
                    cmd.Parameters.AddWithValue("@PreferredDay", needModel.PreferredDay)
                    cmd.Parameters.AddWithValue("@PreferredTimeFrom", needModel.PreferredTimeFrom)
                    cmd.Parameters.AddWithValue("@PreferredTimeTo", needModel.PreferredTimeTo)
                    cmd.Parameters.AddWithValue("@CreatedBy", needModel.CreatedBy)
                    cmd.Parameters.AddWithValue("@CreatedAt", needModel.CreatedAt)
                    cmd.Parameters.AddWithValue("@UpdatedBy", needModel.UpdatedBy)
                    cmd.Parameters.AddWithValue("@LastUpdated", needModel.LastUpdated)
                    cmd.Parameters.AddWithValue("@Status", needModel.LastUpdated)
                    cmd.Parameters.AddWithValue("@ImagePathOne", needModel.ImagePathOne)
                    cmd.Parameters.AddWithValue("@ImagePathTwo", needModel.ImagePathTwo)
                    result = cmd.ExecuteNonQuery()
                End Using
            End Using

            Return result
        End Function

        Public Function GetAllActiveNeeds() As List(Of NeedModel)
            Return FetchNeedsFromDb("SELECT   A.*,
                                               B.description,
                                               C.NAME AS MemberName
                                        FROM   proppycaresneeds A
                                               LEFT JOIN proppycarescategory B
                                                      ON A.categorycode = B.code
                                               LEFT JOIN member C
                                                      ON A.createdby = C.userid ")
        End Function

        Public Function GetNeedsByUser(ByVal UserId As String) As List(Of NeedModel)
            Return FetchNeedsFromDb("SELECT A.*,
                                                B.description,
                                                '' AS MemberName
                                        FROM   proppycaresneeds A
                                                JOIN proppycarescategory B
                                                    ON A.categorycode = B.code
                                        WHERE  A.createdby = '" & UserId & "'
                                        ORDER  BY A.createdat DESC ")
        End Function

        Public Function GetGrabbedUserDeals(ByVal UserId As String) As List(Of NeedModel)
            Return FetchNeedsFromDb("SELECT A.*,
                                               B.description,
                                               D.company_name AS MerchantName
                                        FROM   proppycaresneeds A
                                               LEFT JOIN proppycarescategory B
                                                      ON A.categorycode = B.code
                                               JOIN bulletinboard C
                                                 ON A.needsid = C.listingid
                                               LEFT JOIN merchants D
                                                      ON D.merchantid = C.assignedto
                                        WHERE  A.createdby = '" & UserId & "' And C.Status = 'Pending' 
                                        ORDER  BY A.createdat DESC ")
        End Function

        Private Function FetchNeedsFromDb(ByVal query As String) As List(Of NeedModel)
            Dim needs As List(Of NeedModel) = New List(Of NeedModel)()

            Using Con As SqlConnection = New SqlConnection(connStr)
                Con.Open()

                Using cmd As SqlCommand = New SqlCommand(query, Con)
                    Dim sdr As SqlDataReader = cmd.ExecuteReader()

                    While sdr.Read()
                        needs.Add(New NeedModel() With {
                            .ID = Long.Parse(sdr.GetValue(0).ToString()),
                            .BizUnit = sdr.GetValue(CInt(1)).ToString(),
                            .NeedsID = sdr.GetValue(CInt(2)).ToString(),
                            .Country = sdr.GetValue(CInt(3)).ToString(),
                            .CategoryCode = sdr.GetValue(CInt(4)).ToString(),
                            .State = sdr.GetValue(CInt(5)).ToString(),
                            .District = sdr.GetValue(CInt(6)).ToString(),
                            .Area = sdr.GetValue(CInt(7)).ToString(),
                            .SubArea = sdr.GetValue(CInt(8)).ToString(),
                            .Street = sdr.GetValue(CInt(9)).ToString(),
                            .Postcode = sdr.GetValue(CInt(10)).ToString(),
                            .Building = sdr.GetValue(CInt(11)).ToString(),
                            .Type = sdr.GetValue(CInt(12)).ToString(),
                            .LevelNo = sdr.GetValue(CInt(13)).ToString(),
                            .UnitNo = sdr.GetValue(CInt(14)).ToString(),
                            .Notes = sdr.GetValue(CInt(15)).ToString(),
                            .PreferredDay = sdr.GetValue(CInt(16)).ToString(),
                            .PreferredTimeFrom = TimeSpan.Parse(sdr.GetValue(17).ToString()),
                            .PreferredTimeTo = TimeSpan.Parse(sdr.GetValue(18).ToString()),
                            .CreatedBy = sdr.GetValue(CInt(19)).ToString(),
                            .CreatedAt = Date.Parse(sdr.GetValue(20).ToString()),
                            .UpdatedBy = sdr.GetValue(CInt(21)).ToString(),
                            .LastUpdated = Date.Parse(sdr.GetValue(22).ToString()),
                            .Status = sdr.GetValue(CInt(23)).ToString(),
                            .ImagePathOne = sdr.GetValue(CInt(24)).ToString(),
                            .ImagePathTwo = sdr.GetValue(CInt(25)).ToString(),
                            .CategoryDescription = sdr.GetValue(CInt(26)).ToString(),
                            .MemberMerchantName = sdr.GetValue(CInt(27)).ToString()
                        })
                    End While
                End Using
            End Using

            Return needs
        End Function

    End Class
End Namespace