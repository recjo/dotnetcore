# PDF Scraper

This DotNet Core 2.1 console app loads all pdf source files in a directory.<br />
The pdf files contain a grid table of crime statistics.<br />
The program loops through all pdf files and, using the iTextSharp pdf library, reads the text data.<br />
It determines the column the text is under by comparing the x and y location points of the text to the known x and y points of the column edges. It then builds a csv file of all the columnar data.

View a sample of the PDF source file [jan-2017-rd.pdf](pdf/jan-2017-rd.pdf)
