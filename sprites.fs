: clear-sprites ( -- )
    CLRSPR ;

: set-sprites-16x16 ( -- )
    SYS-RG1SAV c@ 0b10 or 1 WRTVDP ;

: sprite-name! ( name spriteid -- )
    CALATR 2 + WRTVRM ;

: sprite-color! ( color spriteid -- )
    CALATR 3 + WRTVRM ;

: sprite-pos! ( horizontal vertical spriteid -- )
    CALATR dup 1+ -rot WRTVRM WRTVRM ;
