SJASM=`which sjasm` -j
EMULATOR=openmsx -cart
SAMC=../samc.py --header=../samforth.begin --footer=../samforth.end --include=.. 

all: robots.rom

%.rom: %.fs
	python $(SAMC) $? $(@:.rom=.asm)
	$(SJASM) $(@:.rom=.asm) $@

robots: robots.rom
	$(EMULATOR) $?

clear:
	rm -f robots.asm robots.lst robots.rom
