# barcode-scanning

barcode-scanning

A project developed on a Intermect CN70 device running the windows mobile 6.5 operating system. The project is a barcode scanning software that scans product barcodes (EAN13) and then generates an EAN128 barcode for boxes and pallets. The software makes use of object oriented programming on C# and an Oracle Database.

There are 5 main forms used in the software which are listed below.

FormUserLogin This form takes in the user's username and password. The username and password get inputted into a PL/SQL function that verifies the user's credentials. The user would also be requested to choose which printer the device should make a connection with in order to print the EAN128 barcodes. Intermec printers are being used and a IP address is assigned to each printer. The code behind will associate certain printers with the corresponding IP address. The username, and IP address of the printer is then saved under a static class called "InnodisLogin" under the GlobalClass.cs file.

FormChooseMethod This form is accessed when the user's credential matches what is available on the database. On this form, the user can choose to access three forms or decide to Logout (which will take the user to FormUserLogin). The three forms mentioned are the forms discussed in 3, 4, and 5.

FormScanEAN13 This form allows the user to choose the client and delivery date for the products that are to be sent, through the use of comboboxes. Once the client and the corresponding delivery date is chosen, the user can scan individual products. The barcode number, client name, client code, production location code, product netweight, scanneditemId (primary key), product code, product name, delivery date, and scanned date appear on the gridview on the form. This information will keep appearing for each product scanned. Once scanning is done, the user can click the "Print EAN128 button" and all the information on the gridview will be inserted into a database table. At the same time, the barcode is printed on the printer that was chosen on FormUserLogin. A pop message will appear either showing that the "EAN128 has been printed successfully" or "EAN128 not printed" along with its error message.

FormScabEAB128 This form allows a user to scan the EAN 128 barcodes that were printed on FormScanEAN13. Once scanned, the gridview will display the products information under the EAN128 scanned. The form keeps doing so until the user clicks on the "Print Pallet Barcode". Again, the pallet id gets saved into the database and a new barcode gets printed for the pallet. The form will be transferred back to FormChooseMethod.

FormViewEAN128information This form allows the user to scan already printed EAN128 from either FormScanEAN13 or FormScabEAB128 and check the information stored within it. The information would be viewed on the two gridviews available on the form. Once the information has been loaded, the user can decide to reprint the scanned barcode or return the FormChooseMethod.

There are additional functions in each form that I would like to go more in depth into. This will be updated over time as the final coding is not complete as of yet.
