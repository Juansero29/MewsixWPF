# MewsixWPF
*A simple music player born from a master/detail project.*


<h2>Short Description:</h2>
<h3>
First stable version of Mewsix. The first music player that uses your song files metadata to bring you song lyrics, artist information, album cover images and a nice and simple user interface. You can also modify your file's information, press enter and it will update the song's information and metadata accordingly.
</h3>


<h2> Current version: v0.1.0.0-alpha.1*</h2>
<h3> 
- Added .mp3 or .flac files support.
- Drag n' drop function available.
- Search bar to find your songs quicker.
- Modify any information from your song files' metadata.
</h3>


<h2>Class Diagram: </h2>

![MewsixClassDiagram](https://github.com/Juansero29/MewsixWPF/blob/master/Diagrams/Class%20Diagram/MewsixClassDiagram.png "Mewsix Complete Class Diagram")


<h2>Activities Diagrams: </h2>

![AddTrackActivity](https://github.com/Juansero29/MewsixWPF/blob/master/Diagrams/Activities%20Diagrams/AddTrackActivity.PNG "Add track activity... ")

![EditTrackActivity](https://github.com/Juansero29/MewsixWPF/blob/master/Diagrams/Activities%20Diagrams/EditTrackActivity.PNG "Edit track activity diagram... ")

![RemoveTrackActivity](https://github.com/Juansero29/MewsixWPF/blob/master/Diagrams/Activities%20Diagrams/RemoveTrackActivity.PNG "Remove track activity diagram...")


<h2>Use-case Diagram: </h2>
![MewsixUseCaseDiagram](https://github.com/Juansero29/MewsixWPF/blob/master/Diagrams/UseCase%20Diagram/MewsixUseCaseDiagram.PNG "Use-case diagram... ")


<h2>Functional Specifications: </h2>

<h3>
Les sp�cifications fonctionnelles
Les sp�cifications fonctionnelles permettent de d�finir comme le produit doit se comporter.

L'utilisateur appuie sur bouton NextTrack ou PreviousTrack ou Pause
- Cas normal : on joue le morceau suivant/pr�c�dent, ou on le met en pause.
- Cas d'erreur : S/O
- Cas limite : S/O

Glisser - d�poser
- Cas normal : le fichier d�pos� est de type .mp3 ou .flac, il est donc ajout� � la liste des Tracks
- Cas d'erreur : le fichier d�pos� n'est pas compatible avec le logiciel
- Cas limite : L'utilisateur ne peut pas d�poser des dossiers contenant des dossiers

Modification d'informations
- Cas normal : L'utilisateur modifie les informations sur un morceau. Une bo�te de dialogue indique que les changements ont �t� sauvegard�s
- Cas limite : Certaines informations ne sont pas modifiables
- Cas d'erreur : Le morceau est en train d'�tre jou�

Barre de recherche
- Cas normal : On trouve ce qu'on recherche
- Cas limite : On ne peut pas trouver ce qu'on n'a pas sur son ordinateur
- Cas d'erruer : S/O

Requ�te API (paroles, image d'album, informations Artiste)
- Cas normal : Les informations voulues sont trouv�es sur internet
- Cas limite : L'API ne renvoie pas d'informations suite � la requ�te, on affiche des informations par d�faut.
- Cas d'erreur : L'utilisateur ne dispose pas d'une connexion internet ou l'API est indisponible.
</h3>


<h2> Sketchs </h2>

