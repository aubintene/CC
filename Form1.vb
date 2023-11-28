
Public Class Form1
    'Bouton Ajouter des éléments a la ListView
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'Ajouter des éléments a la ListView
        ListView1.Items.Add(New ListViewItem(New String() {TextBox1.Text, TextBox2.Text, TextBox3.Text}))
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
    End Sub

    'Bouton Enregistrer la ListView
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'Choisir l'emplacement d'enregistrement des données.
        Using sauve As New SaveFileDialog()
            sauve.Filter = "Fichier texte|*.txt"
            If sauve.ShowDialog() = DialogResult.OK Then
                SauvegarderLesDonnées(ListView1, sauve.FileName)
            End If

        End Using

    End Sub

    'Bouton Recharger la ListView
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        'Choisir le fichier a recharger dans la ListView
        Using Ouvrir As New OpenFileDialog
            Ouvrir.Filter = "Texte|*.txt" 'Pour plusieurs extensions on fait: Texte|*.txt|Application|*.exe|Tous|*.*
            If Ouvrir.ShowDialog() = DialogResult.OK Then
                RechargerLesDonnées(ListView1, Ouvrir.FileName)
            End If

        End Using

    End Sub


    'Méthode pour sauvegarder les donénes dans un fichier.
    Sub SauvegarderLesDonnées(ByVal Liste As ListView, ByVal Fichier As String)

        Dim ItemSplit As String = "(*E*)" 'Pour séparer entre les éléments -> élément1(*E*)élémen2(*E*)élément3
        Dim ColonneSplit As String = "(*C*)" 'Pour séparer entre les valeurs -> Nom(*C*)Age(*C*)(*Couleur*)
        Dim Build As New Text.StringBuilder() 'Un outil très utile pour construire du string
        Dim nElement As Integer = Liste.Items.Count

        For Each element As ListViewItem In Liste.Items 'Pour chaque élément dans la ListView

            For i As Integer = 0 To Liste.Columns.Count - 1 'Pour chaque colonne dans un élément
                Build.Append(element.SubItems(i).Text) 'Ajouter la valeur du colonne dans le Build
                If (i < Liste.Columns.Count - 1) Then 'Si ce n'est pas la dernière colonne.
                    Build.Append(ColonneSplit) 'Ajouter le séparateur (*C*)
                End If
            Next

            Build.Append(ItemSplit) 'Séparer entre chaque élément -> David(*C*)25(*C*)Rouge(*E*)
        Next



        Try

            IO.File.WriteAllText(Fichier, Build.ToString()) 'écrir le build dans le fichier 
            MessageBox.Show("Sauvegardé")
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erreur")
        End Try


    End Sub

    'Méthode pour rechager les donénes depuis un fichier.
    Sub RechargerLesDonnées(ByVal Liste As ListView, ByVal Fichier As String)
        Liste.Items.Clear()
        Dim ItemSplit As String = "(*E*)"
        Dim ColonneSplit As String = "(*C*)"


        Try 'Au cas d'erreur
            If IO.File.Exists(Fichier) Then 'Si le fichier existe

                Dim LesDonnées As String = IO.File.ReadAllText(Fichier) 'Lire toute les données
                Dim LesElements() As String = LesDonnées.Split(New String() {ItemSplit}, StringSplitOptions.RemoveEmptyEntries) 'Séparer avec (*E*)

                For Each element As String In LesElements 'Pour chaque éléments dans LesElements()
                    If (element.Contains(ColonneSplit)) Then 'Si ça contient le séparateur (*C*)

                        Dim LesValeurs() As String = element.Split(New String() {ColonneSplit}, StringSplitOptions.None) 'Séparer avec (*C*)
                        Dim NouveauElement As New ListViewItem 'Nouveau élément a ajouter dans la ListView

                        For i As Integer = 0 To LesValeurs.Length - 1 'Pour chaque valeur dans  LesValeurs()
                            If i = 0 Then 'Si c'est la première valeur
                                NouveauElement.Text = LesValeurs(i) 'Ajouter la valeur dans la première colonne.
                            Else 'Si non l'ajouter dans les autres colonnes
                                NouveauElement.SubItems.Add(LesValeurs(i)) 'Si i = 1 donc c'est la deuxième colonne etc
                            End If
                        Next
                        Liste.Items.Add(NouveauElement) 'Ajouter l'élément a la ListView

                    End If
                Next


            End If



        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erreur")
        End Try


    End Sub

End Class

