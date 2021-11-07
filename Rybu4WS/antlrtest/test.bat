CALL antlr4 ../Rybu4WS.g4
CALL javac Rybu4WS*.java
CALL grun Rybu4WS file -tree -gui %1