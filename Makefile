SJASM=`which sjasm` -j
EMULATOR=openmsx -cart

all: test.rom test2.rom

%.rom: %.fs
	python samc.py < $? > $(@:.rom=.asm)
	$(SJASM) $(@:.rom=.asm) $@

test: test.rom
	$(EMULATOR) test.rom

test2: test2.rom
	$(EMULATOR) test2.rom

clear:
	rm -f test.asm test.lst test.rom test2.asm test2.lst test2.rom
