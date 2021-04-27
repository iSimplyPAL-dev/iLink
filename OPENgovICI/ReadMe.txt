
Cambiamenti effettuati:

- nella tabella TIPO_RENDITA i tipi dei dati sono stati convertiti a varchar
- nella tabella TblDettaglioTestata i tipi dei dati dei seguenti campi sono stati convertiti a int: MesiPossesso, MesiEsclusioneEsenzione, MesiRiduzione
- nella tabella Versamenti le date sono state convertite al tipo DateTime
- nella tabella Versamenti è stata aggiunta la colonna Annulltao per la cancellazione logica dei dati.
- nella tabella Oggetti il tipo del campo "ValoreImmobile" è stato cambiato per float.

Cambiamenti da fare

- Classe deve essere drop down (nella ricera immobili)
- Idem per categoria

Importazione Versamenti:

- Controllo sull' "importo per determinazione del dovuto non valorizzati" non effettuato
- Controllo sull' "importo detrazione non corrispondente" non effettuato
- nell'importazione POSTEL non è specificato l'ente, da dove prenderla?
- nell'importazione POSTEL non è specificato il numero bollettino, da dove prenderlo?
- nell'importazione POSTEL non è specificato il comune ubicazione immobile, da dove prenderlo?
- nell'importazione RISCONET non è specificato il numero bollettino ( o sarebbe il numero provvedimento?)
- nell'importazione UVI non è specificato il numero bollettino
- non si trovano i dati del contribuente nei tracciati ministeriali per i varesamenti, se c'è qualcosa
massimo il cognome e il nome
- nei tracciati POSTEL e RISCONET il cognome e il nome del contribuente non sono separate

TO DO SUL PROGETTO (20/08/2004)
--------------------------------

- Completamento dell'allineamento framework per pagina comandi e tutto Utilità ICI (work in progress)

- Interfacciamento a catasto e territorio

- Completamento delle importazioni:
	1) Passaggio da utilità a definitivo
	2) importazione xml di StartUp 
	3) Importazioni file di testo Versamenti (work in progress Orsi)
	4) Importazioni file di testo Dichiarazioni
	
- Popup di ricerca/aggiunta in anagrafica preso da Ribes


TO DO SUL PROGETTO (28/09/2004)
--------------------------------

1) L'immobie non scrive MAI l'idTestata, il legame è SEMPRE la tblDettaglioTestata

2) Aggiunta dei campi specificati nella mail nella pagina immobile

3) Gestione dei campi (2) nella dettagliotestata

4) La tabella Oggetti nonè univoca, gli oggetti vengono riinseriri se trovati (anche uguali) 
   in una dichiarazione
   
5) Aggiungere i validator per tutte le date 

6) Risolvere il problema della paginazione della datagrid nella pagina BonificaDichiarazioni.aspx
   che non si replichino i record.
   

CAMBIAMENTI SUI DB (16/06/2005)
-------------------------------
E' stata aggiunta a tutti e due i db la tabella tblProvenienze

Il campo IDProvenineza (di tipo integer, not allow null, default value = 0) è stato aggiunto
alle seguenti tabelle:
1) nel db ICI (Utilità): tblTestata, tblAnagrafica, tblVersamenti
2) nel db Effettivo: tblTestata, tblVersamenti

E' stato aggiunto il campo Effettivo nelle seguenti viste nel db ICI (Utilità):
	viewContribuentiImmobileUtil, viewContribuentiUtil
	
E' stato aggiunto il campo Bonificato nelle seguenti viste nel db Effettivo:
	viewContribuentiImmobile, viewContribuentiTutti


CAMBIAMENTI SUI DB (20/06/2005)
-------------------------------

E' stato aggiuto il campo CodImmobile (di tipo integer, allow null) alla tabella tblOggetti nel db ICI (utilità).
Deve essere tolta la relazione tra IdDenunciante e ID della tblAnagrafica nel db ICI (utilità) nella vista
viewContribuentiImmobileUtil.
Nella tblDettaglioTestata nel db EffettivoICI sono stati aggiunti i seguenti campi (di tipo bit, not allow null, default value = 0):
	Riduzione, Possesso, EsclusioneEsenzione


CAMBIAMENTI SUI DB (05/07/2005)
-------------------------------

Nel db EffetivoICI:
E' stato aggiunto il campo Annullato alla vista viewContrinbuentiTutti come non output, criteria <>1
E' stato aggiunto il campo Annullato alla vista viewContrinbuentiImmobile come non output, criteria <>1
E' stato aggiunto il campo Annullato alla vista viewVersamenti
Nella vista viewContrinbuentiTutti controllare che ci sia il campo Bonificato
Nella vista viewContrinbuentiImmobile controllare che ci sia il campo Bonificato


CAMBIAMENTI SUI DB (14/07/2005)
-------------------------------

Nel db EffetivoICI:
E' stato aggiunto il campo Ente alla vista viewContrinbuentiTutti
E' stato aggiunto il campo Ente alla vista viewContrinbuentiImmobile

Nel db ICI (Utilità):
E' stato aggiunto il campo Ente alla vista viewContrinbuentiUtil
E' stato aggiunto il campo Ente alla vista viewContrinbuentiImmobileUtil
Sono stati aggiunti i seguenti campi alla tabella tblImportazioni: Operatore (varchar, 50, NOT ALLOW NULL), FileName (varchar, 100, NOT ALLOW NULL),
	Importato (bit, default value 0, NOT ALLOW NULL)