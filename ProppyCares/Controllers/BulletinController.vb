Imports System.Data.SqlClient
Imports System.Net
Imports System.Web.Http
Imports ProppyCares.ProppyAPI.Authorizations

Namespace Controllers
    Public Class BulletinController
        Inherits ApiController

        Private connStr As String = System.Configuration.ConfigurationManager.ConnectionStrings(1).ConnectionString

        ' GET: api/Bulletin
        Public Function GetValues() As IEnumerable(Of String)
            Return New String() {"value1", "value2"}
        End Function

        ' GET: api/Bulletin/5
        Public Function GetValue(ByVal id As Integer) As String
            Return "value"
        End Function

        <HttpPost>
        <Route("api/Bulletin")>
        Public Sub PostValue(<FromBody()> ByVal bulletinBoard As BulletinBoardPostModel)
            GrabBulletin(bulletinBoard)
        End Sub

        ' PUT: api/Bulletin/5
        Public Sub PutValue(ByVal id As Integer, <FromBody()> ByVal bulletinBoard As BulletinBoardPostModel)
            bulletinBoard.BulletinID = id
            UpdateBulletin(bulletinBoard)
        End Sub

        ' DELETE: api/Bulletin/5
        Public Sub DeleteValue(ByVal id As Integer)

        End Sub

        Private Function GrabBulletin(ByVal bulletinModel As BulletinBoardPostModel) As Integer
            Dim result As Integer = 0

            Using sqlConnection As SqlConnection = New SqlConnection(connStr)
                sqlConnection.Open()

                Using cmd As SqlCommand = New SqlCommand("INSERT INTO [dbo].[BulletinBoard]
                                                                           ([BizUnit]
                                                                           ,[BulletinID]
                                                                           ,[ListingID]
                                                                           ,[AssignedTo]
                                                                           ,[CreatedBy]
                                                                           ,[CreatedAt]
                                                                           ,[UpdatedBy]
                                                                           ,[LastUpdated]
                                                                           ,[Status]
                                                                           ,[GrabbedDateTime])
                                                                     VALUES
                                                                           (@BizUnit,
                                                                            @BulletinID,
                                                                            @ListingID,
                                                                            @AssignedTo,
                                                                            @CreatedBy,
                                                                            @CreatedAt,
                                                                            @UpdatedBy,
                                                                            @LastUpdated,
                                                                            @Status,
                                                                            @GrabbedDateTime)", sqlConnection)
                    cmd.Parameters.AddWithValue("@BizUnit", bulletinModel.BizUnit)
                    cmd.Parameters.AddWithValue("@BulletinID", SharedServices.GenerateUserId(TableConstants.BulletinBoard))
                    cmd.Parameters.AddWithValue("@ListingID", bulletinModel.ListingID)
                    cmd.Parameters.AddWithValue("@AssignedTo", bulletinModel.AssignedTo)
                    cmd.Parameters.AddWithValue("@CreatedBy", bulletinModel.CreatedBy)
                    cmd.Parameters.AddWithValue("@CreatedAt", bulletinModel.CreatedAt)
                    cmd.Parameters.AddWithValue("@UpdatedBy", bulletinModel.UpdatedBy)
                    cmd.Parameters.AddWithValue("@LastUpdated", bulletinModel.LastUpdated)
                    cmd.Parameters.AddWithValue("@Status", bulletinModel.Status)
                    cmd.Parameters.AddWithValue("@GrabbedDateTime", bulletinModel.GrabbedDateTime)
                    result = cmd.ExecuteNonQuery()
                End Using
            End Using

            Return result
        End Function

        Private Function UpdateBulletin(ByVal bulletinModel As BulletinBoardPostModel) As Integer
            Dim result As Integer = 0

            Using sqlConnection As SqlConnection = New SqlConnection(connStr)
                sqlConnection.Open()

                Using cmd As SqlCommand = New SqlCommand("UPDATE [dbo].[BulletinBoard]
                                                            SET    [UpdatedBy] = @UpdatedBy,
                                                                   [Status] = @Status
                                                            WHERE  BulletinID = @BulletinID;", sqlConnection)
                    cmd.Parameters.AddWithValue("@BulletinID", bulletinModel.BulletinID)
                    cmd.Parameters.AddWithValue("@UpdatedBy", bulletinModel.UpdatedBy)
                    cmd.Parameters.AddWithValue("@LastUpdated", Date.Now)
                    cmd.Parameters.AddWithValue("@Status", bulletinModel.Status)
                    result = cmd.ExecuteNonQuery()
                End Using
            End Using

            Return result
        End Function

    End Class
End Namespace