hex

\ colors
0 const COLOR-TRANSPARENT         
1 const COLOR-BLACK               
2 const COLOR-MEDIUM-GREEN        
3 const COLOR-LIGHT-GREEN         
4 const COLOR-DARK-BLUE           
5 const COLOR-LIGHT-BLUE          
6 const COLOR-DARK-RED            
7 const COLOR-CYAN                
8 const COLOR-MEDIUM-RED
9 const COLOR-LIGHT-RED
a const COLOR-DARK-YELLOW
b const COLOR-LIGHT-YELLOW
c const COLOR-DARK-GREEN
d const COLOR-MAGENTA
e const COLOR-GRAY
f const COLOR-WHITE

i: color ( fg bg -- color )
    swap 16 * + ;i

\ system vars ===================================================
F380 const SYS-RDPRIM	\ Routine that reads from a primary slot
F385 const SYS-WRPRIM	\ Routine that writes to a primary slot
F38C const SYS-CLPRIM	\ Routine that calls a routine in a primary slot
F39A const SYS-USRTAB	\ Address to call with Basic USR0
F3AE const SYS-LINL40	\ Width for SCREEN 0 (default 37)
F3AF const SYS-LINL32	\ Width for SCREEN 1 (default 29)
F3B0 const SYS-LINLEN	\ Width for the current text mode
F3B1 const SYS-CRTCNT	\ Number of lines on screen
F3B2 const SYS-CLMLST	\ Column space. Itâ€™s uncertain what this address actually stores
F3B3 const SYS-TXTNAM	\ BASE(0) - SCREEN 0 name table
F3B5 const SYS-TXTCOL	\ BASE(1) - SCREEN 0 color table
F3B7 const SYS-TXTCGP	\ BASE(2) - SCREEN 0 character pattern table
F3B9 const SYS-TXTATR	\ BASE(3) - SCREEN 0 Sprite Attribute Table
F3BB const SYS-TXTPAT	\ BASE(4) - SCREEN 0 Sprite Pattern Table
F3B3 const SYS-T32NAM	\ BASE(5) - SCREEN 1 name table
F3B5 const SYS-T32COL	\ BASE(6) - SCREEN 1 color table
F3B7 const SYS-T32CGP	\ BASE(7) - SCREEN 1 character pattern table
F3B9 const SYS-T32ATR	\ BASE(8) - SCREEN 1 sprite attribute table
F3BB const SYS-T32PAT	\ BASE(9) - SCREEN 1 sprite pattern table
F3B3 const SYS-GRPNAM	\ BASE(10) - SCREEN 2 name table
F3B5 const SYS-GRPCOL	\ BASE(11) - SCREEN 2 color table
F3B7 const SYS-GRPCGP	\ BASE(12) - SCREEN 2 character pattern table
F3B9 const SYS-GRPATR	\ BASE(13) - SCREEN 2 sprite attribute table
F3BB const SYS-GRPPAT	\ BASE(14) - SCREEN 2 sprite pattern table
F3B3 const SYS-MLTNAM	\ BASE(15) - SCREEN 3 name table
F3B5 const SYS-MLTCOL	\ BASE(16) - SCREEN 3 color table
F3B7 const SYS-MLTCGP	\ BASE(17) - SCREEN 3 character pattern table
F3B9 const SYS-MLTATR	\ BASE(18) - SCREEN 3 sprite attribute table
F3BB const SYS-MLTPAT	\ BASE(19) - SCREEN 3 sprite pattern table
F3DB const SYS-CLIKSW	\ =0 when key press click disabled =1 when key press click enabled
F3DC const SYS-CSRY	\ Current row-position of the cursor
F3DD const SYS-CSRX	\ Current column-position of the cursor
F3DE const SYS-CNSDFG	\ =0 when function keys are not displayed =1 when function keys are displayed
F3DF const SYS-RG0SAV	\ Content of VDP(0) register (R#0)
F3E0 const SYS-RG1SAV	\ Content of VDP(1) register (R#1)
F3E1 const SYS-RG2SAV	\ Content of VDP(2) register (R#2)
F3E2 const SYS-RG3SAV	\ Content of VDP(3) register (R#3)
F3E3 const SYS-RG4SAV	\ Content of VDP(4) register (R#4)
F3E4 const SYS-RG5SAV	\ Content of VDP(5) register (R#5)
F3E5 const SYS-RG6SAV	\ Content of VDP(6) register (R#6)
F3E6 const SYS-RG7SAV	\ Content of VDP(7) register (R#7)
F3E7 const SYS-STATFL	\ Content of VDP(8) status register (S#0)
F3E8 const SYS-TRGFLG	\ Information about trigger buttons and space bar state
F3E9 const SYS-FORCLR	\ Foreground color
F3EA const SYS-BAKCLR	\ Background color
F3EB const SYS-BDRCLR	\ Border color
F3EC const SYS-MAXUPD	\ Jump instruction used by Basic LINE command. The routines used are: RIGHTC, LEFTC, UPC and DOWNC
F3EF const SYS-MINUPD	\ Jump instruction used by Basic LINE command. The routines used are: RIGHTC, LEFTC, UPC and DOWNC
F3F2 const SYS-ATRBYT	\ Attribute byte (for graphical routines itâ€™s used to read the color)
F3F3 const SYS-QUEUES	\ Address of the queue table
F3F5 const SYS-FRCNEW	\ CLOAD flag =0 when CLOAD =255 when CLOAD?
F3F6 const SYS-SCNCNT	\ Key scan timing
F3F7 const SYS-REPCNT	\ This is the key repeat delay counter
F3F8 const SYS-PUTPNT	\ Address in the keyboard buffer where a character will be written
F3FA const SYS-GETPNT	\ Address in the keyboard buffer where the next character is read
F3FC const SYS-CS120	\ Cassette I/O parameters to use for 1200 baud
F401 const SYS-CS240	\ Cassette I/O parameters to use for 2400 baud
F406 const SYS-LOW	\ Signal delay when writing a 0 to tape
F408 const SYS-HIGH	\ Signal delay when writing a 1 to tape
F40A const SYS-HEADER	\ Delay of tape header (sync.) block
F40B const SYS-ASPCT1	\ Horizontal / Vertical aspect for CIRCLE command
F40D const SYS-ASPCT2	\ Horizontal / Vertical aspect for CIRCLE command
F40F const SYS-ENDPRG	\ Pointer for the RESUME NEXT command
F414 const SYS-ERRFLG	\ Basic Error code
F415 const SYS-LPTPOS	\ Position of the printer head
F416 const SYS-PRTFLG	\ Printer output flag is read by OUTDO
F417 const SYS-NTMSXP	\ Printer type is read by OUTDO. SCREEN ,,,n writes to this address
F418 const SYS-RAWPRT	\ Raw printer output is read by OUTDO
F419 const SYS-VLZADR	\ Address of data that is temporarilly replaced by â€˜Oâ€™ when Basic function VAL("") is running
F41B const SYS-VLZDAT	\ Original value that was in the address pointed to with VLZADR
F41C const SYS-CURLIN	\ Line number the Basic interpreter is working on, in direct mode it will be filled with #FFFF
FAF8 const SYS-EXBRSA	\ Slot address of the SUBROM (EXtended Bios-Rom Slot Address)
FB21 const SYS-DRVINF	\ Nr. of drives connected to disk interface 1
FCC1 const SYS-EXPTBL	\ Slot 0: #80 = expanded, 0 = not expanded. Also slot address of the main BIOS-ROM.
FCC5 const SYS-SLTTBL	\ Mirror of slot 0 secondary slot selection register.

\ bios calls ====================================================

\ F               Check RAM and sets slot for command area.
\ E               ÎÅÔ
\ R               ÎÅÔ
\ M               ×ÓÅ
\ N               When done,a jump to INIT must be made for
\                 further initializator.
: CHKRAM \ TODO
    0 0 0 0 0000 SYSCALL 4drop ;


\ F               Checks if then current character pointed by
\                 HL is one desired.If not,generates
\                 'Syntax error',otherwise falls into CHRGTB.
\ E               HL,character to be checked be placed at the
\                 next location to this RST.
\ R               HL points to next character,a has the
\                 character.
\                 Carry flag set if number, Z flag set if end
\                 of statesment.
\ M               AF,HL.
: SYNCHR \ TODO
    0 0 0 0 0008 SYSCALL 4drop ;


\ F               Select the appropriate slot according to the
\                 value given through registers, and reads the
\                  contents of memory from the slots.
\ E               A:   FxxxSSPP
\                      |   ||||
\                      |   ||++--Primary slot #(0-3)
\                      |   ++----Secondary slots #(0-3)
\                      +---------If secondary slot # specified.
\ R               A: Contents of memory.
\ M               AF,BC,DE
\ N               Interrupts are disabled automatically but
\                 are never enabled by this routine.
: RDSLT \ TODO
    0 0 0 0 000C SYSCALL 4drop ;


\ F               Gets next character (on token) from BASIC text.
\ E               HL
\ R               HL points to next characted, A has the
\                 character. Carry flag set if number,Z flag
\                 set if end of statesment encountered.
\ M               AF,HL.
: CHRGTB \ TODO
    0 0 0 0 0010 SYSCALL 4drop ;


\ F               Select the appropriate slot according to the
\                 value given through registers, and writes to
\                 memory.
\ E               A: FxxxSSPP.(see RDSLT).
\                 HL: Address of target memory.
\                 E: Data to be written.
\ R               None.
\ M               AF,BC,D
\ N               (see RDSLT)
: WRTSLT \ TODO
    0 0 0 0 0014 SYSCALL 4drop ;


\ F               Output to the current device.
\ E               A,RTFIL,PRTPGL.
\ R               None.
\ M               None.
: OUTDO \ TODO
    0 0 0 0 0018 SYSCALL 4drop ;


\ F               Performs inter-slot call to specified address.
\ E               IY -FxxxSSPP (high) (see RDSLT)
\                 IX: Adress to call
\ R               None
\ M               None
\ N               Interrupt are disabled automatically but
\                 never enabled to this routine. Argument can
\                 never be passed via the alternate registers of
\                 Z80 or IX and IY.
: CALSLT \ TODO
    0 0 0 0 001C SYSCALL 4drop ;


\ F               Compare HL with DE.
\ E               HL,DE.
\ R               Flag.
\ M               AF.
: DCOMPR \ TODO
    0 0 0 0 0020 SYSCALL 4drop ;


\ F               Select the appropriate slot according to the
\                 value given through registers,and permanently
\                 enables the slot
\ E               A: FxxxSSPP.
\                 HL:Address of target memory.
\ R               None.
\ M               All.
\ N               (see RDSLT)
: ENASLT \ TODO
    0 0 0 0 0024 SYSCALL 4drop ;


\ F               Return the type FAC.
\ E               FAC.
\ R               Flags.
\ M               AF.
: GETYPR \ TODO
    0 0 0 0 0028 SYSCALL 4drop ;


\ F               Performs far_call (i.e. inter-slots call)
\ E               None
\ R               Flags.
\ N               The calling sequence is as follows.
\ 
\ 
\                 RST 6
\                 DB              Destination slot.
\                 DW              Destination address.
\ 
\                 (see CALSLT)
: CALLF \ TODO
    0 0 0 0 0030 SYSCALL 4drop ;


\ F               Performs hardware interrupt procedures.
\ E               None.
\ R               None.
\ M               None.
: KEYINT
    0 0 0 0 0038 SYSCALL 4drop ;


\ F               Performed devise initialization.
\ E               None
\ R               None
\ M               All
: INITIO
    0 0 0 0 003B SYSCALL 4drop ;


\ F               Initializes function key strings.
\ E               None
\ R               None
\ M               All
: INIFNK
    0 0 0 0 003E SYSCALL 4drop ;


\ F               Disables screen display.
\ E               None
\ R               None
\ M               AF,BC
: DISSCR ( -- )
    0 0 0 0 0041 SYSCALL 4drop ;


\ F               Enables screen display.
\ E               None
\ R               None
\ M               AF,BC
: ENASCR ( -- )
    0 0 0 0 0044 SYSCALL 4drop ;


\ F               Writes to the VDP register.
\ E               Register in C,data in B.
\ M               AF,BC.
: WRTVDP ( data register -- )
    8 lshift + >r
    0 r> 0 0 0047 SYSCALL 4drop ;


\ F               Reads the VRAM address by [HL].
\ E               HL.
\ R               A
\ M               AF
: RDVRM ( address -- data )
    >r 0 0 0 r> 004a SYSCALL 3drop 8 rshift ;


\ F               Write to the VRAM address by [HL].
\ E               HL,A.
\ R               None
\ M               AF
: WRTVRM ( data address -- )
    >r 8 lshift 0 0 r> 004d SYSCALL 4drop ;


\ F               Sets up the VDP for read.
\ E               HL
\ R               None
\ M               AF
: SETRD \ TODO
    0 0 0 0 0050 SYSCALL 4drop ;


\ F               Sets up the VDP for write.
\ E               HL
\ R               None
\ M               AF
: SETWRT \ TODO
    0 0 0 0 0053 SYSCALL 4drop ;


\ F               fill the vram with specified data
\ E               addres [HL] , length [BC], data [a]
\ R               none
\ M               All
: FILVRM ( data length address -- )
    >r >r 8 lshift r> r> 0 swap 0056 SYSCALL 4drop ;


\ F               Moves a VRAM memory block to memory.
\ E               Address of sourse [HL], destanation [DE]
\                 lenght [BC] .
\ R               none
\ M               All
: LDIRMV ( length destination source -- )
    >r >r >r 0 r> r> r> 0059 SYSCALL 4drop ;


\ F               Move blok of memery from memory to Vram
\ E               sourse [HL], destanation [DE], length [BC]
\ R               None
\ M               All
: LDIRVM ( length destination source -- )
    >r >r >r 0 r> r> r> 005c SYSCALL 4drop ;


\ F               Sets the VDP mode according to SCRMOD.
\ E               SCRMOD (0...3)
\ R               None
\ M               All
: CHGMOD \ TODO
    0 0 0 0 005F SYSCALL 4drop ;


\ F               Changes the color of the screen.
\ E               Foreground color in FOBCLR.
\                 Background color in BAKCLR.
\                 Border color in BDRCLR
\ R               None
\ M               All
: CHGCLR
    0 0 0 0 0062 SYSCALL 4drop ;


\ F               Performs non-maskable interrupt procedures.
\ E               None
\ R               None
\ M               None
: NMI ( -- )
    0 0 0 0 0066 SYSCALL 4drop ;


\ F               Initializes all sprites.
\                 Patterns are set to nulls,sprite names are
\                 set to sprite plane number,sprite colors are
\                 set to foregroup color,vertical positions are
\                 set to 209
\ E               SCRMOD
\ R               None
\ M               All
: CLRSPR ( -- )
    0 0 0 0 0069 SYSCALL 4drop ;


\ F               Initializes screen for text mode (40*24) and
\                 sets the VDP.
\ E               TXTNAM,TXTCGP.
\ R               None.
\ M               All
: INITXT ( -- )
    0 0 0 0 006C SYSCALL 4drop ;


\ F               Initializes screen for text mode (32*24) and
\                 sets the VDP.
\ E               T32NAM,T32GRP,T32COL,T32ATR,T32PAT.
\ R               None
\ M               All
: INIT32 ( -- )
    0 0 0 0 006F SYSCALL 4drop ;


\ F               Initializes screen for high-resolution mode
\                 and sets the VDP.
\ E               GRPNAM,GRPCGP,GRPCOL,GRATR,GRPPAT.
\ R               None
\ M               All
: INIGRP ( -- )
    0 0 0 0 0072 SYSCALL 4drop ;


\ F               Initializes screen for multi-color mode and
\                 sets the VDP.
\ E               MLTNAM,MLTCGP,MLTCOL,MLTATR,MLTPAT.
\ R               None
\ M               All
: INIMLT ( -- )
    0 0 0 0 0075 SYSCALL 4drop ;


\ F               Sets the VDP for text (40*24) mode.
\ E               TXTNAM,TXTCGT.
\ R               None
\ M               All
: SETTXT ( -- )
    0 0 0 0 0078 SYSCALL 4drop ;


\ F               Sets the VDP for text (32*24) mode.
\ E               T32NAM,T32CGT,T32COL,T32ATR,T32PAT.
\ R               None
\ M               All
: SETT32 ( -- )
    0 0 0 0 007B SYSCALL 4drop ;


\ F               Sets the VDP for high-resolution mode.
\ E               GRPNAM,GRPCGP,GRPCOL,GRPATR,GRPPAT.
\ R               None
\ M               All
: SETGRP ( -- )
    0 0 0 0 007E SYSCALL 4drop ;


\ F               Sets the VDP for multicolor mode.
\ E               MLTNAM,MLTCGRP,MLTCOL,MLTATR,MLTPAT.
\ R               None
\ M               All
: SETMLT ( -- )
    0 0 0 0 0081 SYSCALL 4drop ;


\ F               Returns address of sprite pattern table.
\ E               Sprite ID in [Acc].
\ R               Address in [HL]
\ M               AF,DE,HL.
: CALPAT ( spriteid -- address )
    8 lshift 0 0 0 0084 SYSCALL nip nip nip ;


\ F               Returns address of sprite atribute table.
\ E               Sprite ID in [Acc].
\ R               Address in [HL].
\ M               AF,DE,HL.
: CALATR ( spriteid -- address )
    8 lshift 0 0 0 0087 SYSCALL nip nip nip ;


\ F               Returns the current sprite size.
\ E               None.
\ R               Sprite size(# of bytes) in [Acc].
\                 Carry set if 16*16 sprite is use,otherwise
\                 reset the otherwise.
\ M               AF
: GSPSIZ \ TODO
    0 0 0 0 008A SYSCALL 4drop ;


\ F               Prints a character on the graphic screen.
\ E               Code to output in [Acc].
\ R               None
\ M               None
: GRPPRT \ TODO
    0 0 0 0 008D SYSCALL 4drop ;


\ F               Initializes PSG,and static data for PLAY
\                 statement.
\ E               None.
\ R               None.
\ M               All
: GICINI ( -- )
    0 0 0 0 0090 SYSCALL 4drop ;


\ F               Writes data to the PSG register.
\ E               Register number in [Acc],data in [E].
\ R               None
\ M               None
: WRTPSG \ TODO
    0 0 0 0 0093 SYSCALL 4drop ;


\ F               Reads data from PSG register.
\ E               Register number in [Acc].
\ R               Data in [Acc].
\ M               None.
: RDPSG \ TODO
    0 0 0 0 0096 SYSCALL 4drop ;


\ F               Checks/starts background tasks for PLAY.
\ E               None
\ R               None
\ M               All
: STRTMS ( -- )
    0 0 0 0 0099 SYSCALL 4drop ;


\ F               Check the status of keyboard buffer.
\ E               None
\ R               Z flag reset if any character in buffer.
\ M               AF
: CHSNS \ TODO
    0 0 0 0 009C SYSCALL 4drop ;


\ F               Waits for character being input and returns
\                 the character codes.
\ E               None.
\ R               Character code in [Acc].
\ M               AF
: CHGET
    0 0 0 0 009F SYSCALL 3drop 8 rshift ;


\ F               Outputs a character to the console.
\ E               Character code to be output in [Acc].
\ R               None
\ M               None
: CHPUT \ TODO
    0 0 0 0 00A2 SYSCALL 4drop ;


\ F               Output a character to the line printer.
\ E               Character codes to the output in [Acc].
\ R               Carry flag set if aborted.
\ M               F
: LPTOUT \ TODO
    0 0 0 0 00A5 SYSCALL 4drop ;


\ F               Check the line priter status.
\ E               None.
\ R               FF in [Acc] and Z flag reset if priter ready.
\                 0 and Z flag set if not.
\ M               AF.
: LPTSTT \ TODO
    0 0 0 0 00A8 SYSCALL 4drop ;


\ F               Check graphic header byte and converts codes.
\ E               Character code in [Acc].
\ R               Cy flag reset: graphic header byte
\                 Cy and Z flags set converted graphic code.
\                 Cy flag set,Z flag reset,non-converted code.
\ M               AF.
: SNVCHR \ TODO
    0 0 0 0 00AB SYSCALL 4drop ;


\ F               Accepts a line from console until a CR or STOP
\                 is typed,and stores the line in a buffer.
\ E               None
\ R               Address of buffer top-1 in [HL],carry flag
\                 sets if STOP is input.
\ M               All.
: PINLIN \ TODO
    0 0 0 0 00AE SYSCALL 4drop ;


\ F               Same as PINLIN,exept if AUTFLO if set.
\ E               None
\ R               Address of buffer top-1 in [HL],carry flag
\                 set if STOP is input.
\ M               All
: INLIN \ TODO
    0 0 0 0 00B1 SYSCALL 4drop ;


\ F               Output a '?' mark and a space then falls into
\                 the INLIN routine.
\ E               None
\ R               Address of buffer top-1 in [HL],carry flag
\                 set if STOP is input.
\ M               All.
: QINLIN \ TODO
    0 0 0 0 00B4 SYSCALL 4drop ;


\ F               Check the status of the Control-STOP key.
\ E               None
\ R               Carry flag set if being pressed.
\ M               AF
\ N               This routine is used to check Control-STOP
\                 when interrupt are disabled.
: BREAKX \ TODO
    0 0 0 0 00B7 SYSCALL 4drop ;


\ F               Check the status of the SHIFT-STOP key.
\ E               None
\ R               None
\ M               None
: ISCNTC ( -- )
    0 0 0 0 00BA SYSCALL 4drop ;


\ F               Same as ISCNTC,used by BASIC
\ E               None
\ R               None
\ M               None
: CKCNTC ( -- )
    0 0 0 0 00BD SYSCALL 4drop ;


\ F               Sounds the buffer
\ E               None
\ R               None
\ M               All
: BEEP ( -- )
    0 0 0 0 00C0 SYSCALL 4drop ;


\ F               Clear the screen.
\ E               None.
\ R               None.
\ M               AF,BC,DE.
: CLS ( -- )
    0 0 0 0 00C3 SYSCALL 4drop ;


\ F               Locate cursor at the specified position.
\ E               Column in [H],row in [L]
\ R               None
\ M               AF
: POSIT \ TODO
    0 0 0 0 00C6 SYSCALL 4drop ;


\ F               Check if function key display is active. If
\                 it is,it display it,otherwise does nothing.
\ E               FNKFLG
\ R               None
\ M               All
: FNKSB ( -- )
    0 0 0 0 00C9 SYSCALL 4drop ;


\ F               Erased the function key diplay.
\ E               None
\ R               None
\ M               All
: ERAFNK ( -- )
    0 0 0 0 00CC SYSCALL 4drop ;


\ F               Display the function key display.
\ E               None
\ R               None
\ M               All
: DSPFNK ( -- )
    0 0 0 0 00CF SYSCALL 4drop ;


\ F               Forcidly places the screen in text mode.
\ E               None
\ R               None
\ M               All
: TOTEXT ( -- )
    0 0 0 0 00D2 SYSCALL 4drop ;


\ F               Return the current joystick status.
\ E               Joystick ID in [Acc]
\ R               Direction in [Acc]
\ M               All
: GTSTCK \ TODO
    0 0 0 0 00D5 SYSCALL 4drop ;


\ F               Return the current trigger button status.
\ E               Trigger button in ID in [Acc].
\ R               Return 0 in [Acc] if not pressed,255
\                 otherwise.
\ M               AF
: GTTRIG \ TODO
    0 0 0 0 00D8 SYSCALL 4drop ;


\ F               Check the current touch PAD status.
\ E               ID in [Acc]
\ R               Value in [Acc]
\ M               All
: GTPAD \ TODO
    0 0 0 0 00DB SYSCALL 4drop ;


\ F               Return the value of the paddle.
\ E               Padle ID in [Acc].
\ R               Value in [Acc]
\ M               All
: GTPDL \ TODO
    0 0 0 0 00DE SYSCALL 4drop ;


\ F
\ E
\ R
\ M
\ N               Used only to play music in the background.
: GETVCP ( -- )
    0 0 0 0 0150 SYSCALL 4drop ;


\ F
\ E
\ R
\ M
\ N               Used only to play music in barcground.
: GETVC2 ( -- )
    0 0 0 0 0153 SYSCALL 4drop ;


\ F               Clears the keyboard buffer.
\ E               None
\ R               None
\ M               HL
: KILBUF
    0 0 0 0 0156 SYSCALL 4drop ;


\ F               Performs far_call (i.e.inter-slot call) into
\                 the BASIC interpreter.
\ E               Address in [IX]
\ R
\ M
: CALBAS \ TODO
    0 0 0 0 0159 SYSCALL 4drop ;

decimal