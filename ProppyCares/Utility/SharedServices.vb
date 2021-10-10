
Namespace ProppyAPI.Authorizations
    Module SharedServices
        Public Const TABLE_MEMBER As String = "AA"
        Public Const TABLE_PROPERTYAGENT As String = "AE"
        Public Const TABLE_MERCHANTS As String = "AB"
        Public Const TABLE_PRELOVED As String = "AD"
        Public Const TABLE_MOP As String = "AL"

        Function GenerateUserId(table As String) As String
            Dim dateAndTime As Date
            dateAndTime = Now
            Dim sPrefix As String = ""
            Dim rdm As New Random()
            For i As Integer = 1 To 2 ' 3 Letters enough ?
                sPrefix &= ChrW(rdm.Next(65, 90))
            Next

            Return table & Format(dateAndTime, "yyMMddHHmmssfff").ToString & sPrefix
        End Function

    End Module

End Namespace