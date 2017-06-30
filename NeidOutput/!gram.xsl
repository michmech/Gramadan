<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:param name="metalang">ga</xsl:param> <!--acceptable values: 'ga' or 'en'-->
	<xsl:output indent="yes" encoding="utf-8" method="html" doctype-public="-//W3C//DTD XHTML 1.0 Transitional//EN" doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"/>
	<!--<xsl:output indent="yes" encoding="utf-8" method="xml"/>-->
	
	<xsl:template match="/">
		<html>
			<head>
				<title><xsl:value-of select="/*/@lemma"/></title>
				<link type="text/css" rel="stylesheet" href="!gram.css" />
			</head>
			<body>
				<div class="envelope">
					<xsl:apply-templates/>
				</div>
			</body>
		</html>
	</xsl:template>

	<xsl:template match="Lemma">
		<xsl:apply-templates/>
	</xsl:template>

	<xsl:template match="noun">
		<div class="header">
			<h1><xsl:value-of select="parent::Lemma/@lemma"/></h1>
			<div class="property">
				<div class="bullet">▪</div>
				<div class="value">
					<xsl:choose><xsl:when test="$metalang='ga'">AINMFHOCAL</xsl:when><xsl:when test="$metalang='en'">NOUN</xsl:when></xsl:choose>
				</div>
			</div>
			<xsl:if test="@gender">
				<div class="property">
					<div class="bullet">▪</div>
					<div class="value">
						<xsl:choose>
							<xsl:when test="@gender='masc'"><xsl:choose><xsl:when test="$metalang='ga'">FIRINSCNEACH</xsl:when><xsl:when test="$metalang='en'">MASCULINE</xsl:when></xsl:choose></xsl:when>
							<xsl:when test="@gender='fem'"><xsl:choose><xsl:when test="$metalang='ga'">BAININSCNEACH</xsl:when><xsl:when test="$metalang='en'">FEMININE</xsl:when></xsl:choose></xsl:when>
						</xsl:choose>
					</div>
				</div>
			</xsl:if>
			<xsl:if test="@declension>0">
				<div class="property">
					<div class="bullet">▪</div>
					<div class="value">
						<xsl:choose>
							<xsl:when test="@declension='1'"><xsl:choose><xsl:when test="$metalang='ga'">1ú DÍOCHLAONADH</xsl:when><xsl:when test="$metalang='en'">1st DECLENSION</xsl:when></xsl:choose></xsl:when>
							<xsl:when test="@declension='2'"><xsl:choose><xsl:when test="$metalang='ga'">2ú DÍOCHLAONADH</xsl:when><xsl:when test="$metalang='en'">2nd DECLENSION</xsl:when></xsl:choose></xsl:when>
							<xsl:when test="@declension='3'"><xsl:choose><xsl:when test="$metalang='ga'">3ú DÍOCHLAONADH</xsl:when><xsl:when test="$metalang='en'">3rd DECLENSION</xsl:when></xsl:choose></xsl:when>
							<xsl:when test="@declension='4'"><xsl:choose><xsl:when test="$metalang='ga'">4ú DÍOCHLAONADH</xsl:when><xsl:when test="$metalang='en'">4th DECLENSION</xsl:when></xsl:choose></xsl:when>
							<xsl:when test="@declension='5'"><xsl:choose><xsl:when test="$metalang='ga'">5ú DÍOCHLAONADH</xsl:when><xsl:when test="$metalang='en'">5th DECLENSION</xsl:when></xsl:choose></xsl:when>
						</xsl:choose>
					</div>
				</div>
			</xsl:if>
		</div>
		<xsl:if test="sgNom or sgGen">
			<div class="section">
				<h2><xsl:choose><xsl:when test="$metalang='ga'">Uatha</xsl:when><xsl:when test="$metalang='en'">Singular</xsl:when></xsl:choose></h2>
				<xsl:if test="sgNom">
					<div class="subsection">
						<h3><xsl:choose><xsl:when test="$metalang='ga'">AINMNEACH</xsl:when><xsl:when test="$metalang='en'">NOMINATIVE</xsl:when></xsl:choose></h3>
						<xsl:apply-templates select="sgNom"/>
					</div>
				</xsl:if>
				<xsl:if test="sgGen">
					<div class="subsection">
						<h3><xsl:choose><xsl:when test="$metalang='ga'">GINIDEACH</xsl:when><xsl:when test="$metalang='en'">GENITIVE</xsl:when></xsl:choose></h3>
						<xsl:apply-templates select="sgGen"/>
					</div>
				</xsl:if>
			</div>
		</xsl:if>
		<xsl:if test="plNom or plGen">
			<div class="section">
				<h2><xsl:choose><xsl:when test="$metalang='ga'">Iolra</xsl:when><xsl:when test="$metalang='en'">Plural</xsl:when></xsl:choose></h2>
				<xsl:if test="plNom">
					<div class="subsection">
						<h3><xsl:choose><xsl:when test="$metalang='ga'">AINMNEACH</xsl:when><xsl:when test="$metalang='en'">NOMINATIVE</xsl:when></xsl:choose></h3>
						<xsl:apply-templates select="plNom"/>
					</div>
				</xsl:if>
				<xsl:if test="plGen">
					<div class="subsection">
						<h3><xsl:choose><xsl:when test="$metalang='ga'">GINIDEACH</xsl:when><xsl:when test="$metalang='en'">GENITIVE</xsl:when></xsl:choose></h3>
						<xsl:apply-templates select="plGen"/>
					</div>
				</xsl:if>
			</div>
		</xsl:if>
	</xsl:template>
	<xsl:template match="nounPhrase">
		<div class="header">
			<h1><xsl:value-of select="parent::Lemma/@lemma"/></h1>
			<div class="property">
				<div class="bullet">▪</div>
				<div class="value">
					<xsl:choose><xsl:when test="$metalang='ga'">FRÁSA AINMFHOCLACH</xsl:when><xsl:when test="$metalang='en'">NOUN PHRASE</xsl:when></xsl:choose>
				</div>
			</div>
			<xsl:if test="@gender">
				<div class="property">
					<div class="bullet">▪</div>
					<div class="value">
						<xsl:choose>
							<xsl:when test="@gender='masc'"><xsl:choose><xsl:when test="$metalang='ga'">FIRINSCNEACH</xsl:when><xsl:when test="$metalang='en'">MASCULINE</xsl:when></xsl:choose></xsl:when>
							<xsl:when test="@gender='fem'"><xsl:choose><xsl:when test="$metalang='ga'">BAININSCNEACH</xsl:when><xsl:when test="$metalang='en'">FEMININE</xsl:when></xsl:choose></xsl:when>
						</xsl:choose>
					</div>
				</div>
			</xsl:if>
		</div>
		<xsl:if test="sgNom or sgGen">
			<div class="section">
				<h2><xsl:choose><xsl:when test="$metalang='ga'">Uatha</xsl:when><xsl:when test="$metalang='en'">Singular</xsl:when></xsl:choose></h2>
				<xsl:if test="sgNom">
					<div class="subsection">
						<h3><xsl:choose><xsl:when test="$metalang='ga'">AINMNEACH</xsl:when><xsl:when test="$metalang='en'">NOMINATIVE</xsl:when></xsl:choose></h3>
						<xsl:apply-templates select="sgNom"/>
					</div>
				</xsl:if>
				<xsl:if test="sgGen">
					<div class="subsection">
						<h3><xsl:choose><xsl:when test="$metalang='ga'">GINIDEACH</xsl:when><xsl:when test="$metalang='en'">GENITIVE</xsl:when></xsl:choose></h3>
						<xsl:apply-templates select="sgGen"/>
						<xsl:if test="@forceNominative='1' and sgGen/articleNo/text()!=sgNom/articleNo/text()">
							<div class="note">
								<xsl:choose>
									<xsl:when test="$metalang='ga'">
										<b>NÓTA</b> Nuair nach mbíonn an t-alt 
										<xsl:choose>
											<xsl:when test="@gender='masc'">(<i>an</i>)</xsl:when>
											<xsl:when test="@gender='fem'">(<i>na</i>)</xsl:when>
										</xsl:choose>
										i láthair, is iondúil gurb é an t-ainmneach a úsáidtear in áit an ghinidigh:
										<i>ag lorg <xsl:value-of select="sgGen/articleYes/text()"/>, le haghaidh <xsl:value-of select="sgGen/articleYes/text()"/></i>
										ach <i>ag lorg <xsl:value-of select="sgNom/articleNo/text()"/>, le haghaidh <xsl:value-of select="sgNom/articleNo/text()"/></i>.
									</xsl:when>
									<xsl:when test="$metalang='en'">
										<b>NOTE</b> When the definite article 
										<xsl:choose>
											<xsl:when test="@gender='masc'">(<i>an</i>)</xsl:when>
											<xsl:when test="@gender='fem'">(<i>na</i>)</xsl:when>
										</xsl:choose>
										is not present, generally the nominative is used instead of the genitive:
										<i>ag lorg <xsl:value-of select="sgGen/articleYes/text()"/>, le haghaidh <xsl:value-of select="sgGen/articleYes/text()"/></i>
										but <i>ag lorg <xsl:value-of select="sgNom/articleNo/text()"/>, le haghaidh <xsl:value-of select="sgNom/articleNo/text()"/></i>.
									</xsl:when>
								</xsl:choose>
							</div>
						</xsl:if>
					</div>
				</xsl:if>
			</div>
		</xsl:if>
		<xsl:if test="plNom or plGen">
			<div class="section">
				<h2><xsl:choose><xsl:when test="$metalang='ga'">Iolra</xsl:when><xsl:when test="$metalang='en'">Plural</xsl:when></xsl:choose></h2>
				<xsl:if test="plNom">
					<div class="subsection">
						<h3><xsl:choose><xsl:when test="$metalang='ga'">AINMNEACH</xsl:when><xsl:when test="$metalang='en'">NOMINATIVE</xsl:when></xsl:choose></h3>
						<xsl:apply-templates select="plNom"/>
					</div>
				</xsl:if>
				<xsl:if test="plGen">
					<div class="subsection">
						<h3><xsl:choose><xsl:when test="$metalang='ga'">GINIDEACH</xsl:when><xsl:when test="$metalang='en'">GENITIVE</xsl:when></xsl:choose></h3>
						<xsl:apply-templates select="plGen"/>
						<xsl:if test="@forceNominative='1' and plGen/articleNo/text()!=plNom/articleNo/text()">
							<div class="note">
								<xsl:choose>
									<xsl:when test="$metalang='ga'">
										<b>NÓTA</b> Nuair nach mbíonn an t-alt (<i>na</i>) i láthair, is iondúil gurb é an t-ainmneach a úsáidtear in áit an ghinidigh:
										<i>ag lorg <xsl:value-of select="plGen/articleYes/text()"/>, le haghaidh <xsl:value-of select="plGen/articleYes/text()"/></i>
										ach <i>ag lorg <xsl:value-of select="plNom/articleNo/text()"/>, le haghaidh <xsl:value-of select="plNom/articleNo/text()"/></i>.
									</xsl:when>
									<xsl:when test="$metalang='en'">
										<b>NOTE</b> When the definite article (<i>na</i>) is not present, generally the nominative is used instead of the genitive:
										<i>ag lorg <xsl:value-of select="plGen/articleYes/text()"/>, le haghaidh <xsl:value-of select="plGen/articleYes/text()"/></i>
										but <i>ag lorg <xsl:value-of select="plNom/articleNo/text()"/>, le haghaidh <xsl:value-of select="plNom/articleNo/text()"/></i>.
									</xsl:when>
								</xsl:choose>
							</div>
						</xsl:if>
					</div>
				</xsl:if>
			</div>
		</xsl:if>
	</xsl:template>
	<xsl:template match="prepositionalPhrase">
		<div class="header">
			<h1>
				<xsl:value-of select="parent::Lemma/@lemma"/>
			</h1>
			<div class="property">
				<div class="bullet">▪</div>
				<div class="value">
					<xsl:choose>
						<xsl:when test="$metalang='ga'">FRÁSA RÉAMHFHOCLACH</xsl:when>
						<xsl:when test="$metalang='en'">PREPOSITIONAL PHRASE</xsl:when>
					</xsl:choose>
				</div>
			</div>
		</div>
		<xsl:if test="sg">
			<div class="section">
				<h2>
					<xsl:choose>
						<xsl:when test="$metalang='ga'">Uatha</xsl:when>
						<xsl:when test="$metalang='en'">Singular</xsl:when>
					</xsl:choose>
				</h2>
				<xsl:apply-templates select="sg"/>
			</div>
		</xsl:if>
		<xsl:if test="pl">
			<div class="section">
				<h2>
					<xsl:choose>
						<xsl:when test="$metalang='ga'">Iolra</xsl:when>
						<xsl:when test="$metalang='en'">Plural</xsl:when>
					</xsl:choose>
				</h2>
				<xsl:apply-templates select="pl"/>
			</div>
		</xsl:if>
	</xsl:template>
	<xsl:template match="noun/*|nounPhrase/*|prepositionalPhrase/*">
		<div class="block">
			<div class="line">
				<span class="value primary"><xsl:value-of select="articleNo/text()"/></span>
			</div>
			<xsl:for-each select="articleYes">
				<div class="line bulletted">
					<span class="bullet">▪</span>
					<span class="value"><xsl:value-of select="text()"/></span>
					<xsl:if test="@var">
						<xsl:text xml:space="preserve"> </xsl:text>
						<xsl:choose>
							<xsl:when test="@var='south'">
								<span class="label"><xsl:choose><xsl:when test="$metalang='ga'">(CÓRAS LÁRNACH)</xsl:when><xsl:when test="$metalang='en'">(CENTRAL SYSTEM)</xsl:when></xsl:choose></span>
							</xsl:when>
							<xsl:when test="@var='north'">
								<span class="label"><xsl:choose><xsl:when test="$metalang='ga'">(CÓRAS AN tSÉIMHITHE)</xsl:when><xsl:when test="$metalang='en'">(LENITION SYSTEM)</xsl:when></xsl:choose></span>
							</xsl:when>
						</xsl:choose>
					</xsl:if>
				</div>
			</xsl:for-each>
		</div>
	</xsl:template>
	
	<xsl:template match="adjective">
		<div class="header">
			<h1><xsl:value-of select="parent::Lemma/@lemma"/></h1>
			<div class="property">
				<div class="bullet">▪</div>
				<div class="value">
					<xsl:choose><xsl:when test="$metalang='ga'">AIDIACHT</xsl:when><xsl:when test="$metalang='en'">ADJECTIVE</xsl:when></xsl:choose>
				</div>
			</div>
			<xsl:if test="@declension>0">
				<div class="property">
					<div class="bullet">▪</div>
					<div class="value">
						<xsl:choose>
							<xsl:when test="@declension='1'"><xsl:choose><xsl:when test="$metalang='ga'">1ú DÍOCHLAONADH</xsl:when><xsl:when test="$metalang='en'">1st DECLENSION</xsl:when></xsl:choose></xsl:when>
							<xsl:when test="@declension='2'"><xsl:choose><xsl:when test="$metalang='ga'">2ú DÍOCHLAONADH</xsl:when><xsl:when test="$metalang='en'">2nd DECLENSION</xsl:when></xsl:choose></xsl:when>
							<xsl:when test="@declension='3'"><xsl:choose><xsl:when test="$metalang='ga'">3ú DÍOCHLAONADH</xsl:when><xsl:when test="$metalang='en'">3rd DECLENSION</xsl:when></xsl:choose></xsl:when>
							<xsl:when test="@declension='4'"><xsl:choose><xsl:when test="$metalang='ga'">4ú DÍOCHLAONADH</xsl:when><xsl:when test="$metalang='en'">4th DECLENSION</xsl:when></xsl:choose></xsl:when>
							<xsl:when test="@declension='5'"><xsl:choose><xsl:when test="$metalang='ga'">5ú DÍOCHLAONADH</xsl:when><xsl:when test="$metalang='en'">5th DECLENSION</xsl:when></xsl:choose></xsl:when>
						</xsl:choose>
					</div>
				</div>
			</xsl:if>
		</div>
		<xsl:if test="sgNomMasc or sgNomFem or sgGenMasc or sgGenFem">
			<div class="section">
				<h2><xsl:choose><xsl:when test="$metalang='ga'">Uatha</xsl:when><xsl:when test="$metalang='en'">Singular</xsl:when></xsl:choose></h2>
				<xsl:if test="sgNomMasc or sgNomFem">
					<div class="subsection">
						<h3><xsl:choose><xsl:when test="$metalang='ga'">AINMNEACH</xsl:when><xsl:when test="$metalang='en'">NOMINATIVE</xsl:when></xsl:choose></h3>
						<div class="block">
							<xsl:apply-templates select="sgNomMasc"/>
							<xsl:apply-templates select="sgNomFem"/>
						</div>
					</div>
				</xsl:if>
				<xsl:if test="sgGenMasc or sgGenFem">
					<div class="subsection">
						<h3><xsl:choose><xsl:when test="$metalang='ga'">GINIDEACH</xsl:when><xsl:when test="$metalang='en'">GENITIVE</xsl:when></xsl:choose></h3>
						<div class="block">
							<xsl:apply-templates select="sgGenMasc"/>
							<xsl:apply-templates select="sgGenFem"/>
						</div>
					</div>
				</xsl:if>
			</div>
		</xsl:if>
		<xsl:if test="plNom or plNomSlen or plGenStrong or plGenWeak">
			<div class="section">
				<h2><xsl:choose><xsl:when test="$metalang='ga'">Iolra</xsl:when><xsl:when test="$metalang='en'">Plural</xsl:when></xsl:choose></h2>
				<xsl:if test="plNom">
					<div class="subsection">
						<h3><xsl:choose><xsl:when test="$metalang='ga'">AINMNEACH</xsl:when><xsl:when test="$metalang='en'">NOMINATIVE</xsl:when></xsl:choose></h3>
						<xsl:apply-templates select="plNom"/>
					</div>
				</xsl:if>
				<xsl:if test="plGenStrong or plGenWeak">
					<div class="subsection">
						<h3><xsl:choose><xsl:when test="$metalang='ga'">GINIDEACH</xsl:when><xsl:when test="$metalang='en'">GENITIVE</xsl:when></xsl:choose></h3>
						<div class="block">
							<xsl:apply-templates select="plGenStrong"/>
							<xsl:apply-templates select="plGenWeak"/>
						</div>
					</div>
				</xsl:if>
			</div>
		</xsl:if>
		<xsl:if test="comparPres or superPres or abstractNoun">
			<div class="section">
				<h2><xsl:choose><xsl:when test="$metalang='ga'">Foirmeacha breise</xsl:when><xsl:when test="$metalang='en'">Other forms</xsl:when></xsl:choose></h2>
				<xsl:if test="comparPres">
					<div class="subsection">
						<h3><xsl:choose><xsl:when test="$metalang='ga'">BREISCHÉIM</xsl:when><xsl:when test="$metalang='en'">COMPARATIVE</xsl:when></xsl:choose></h3>
						<xsl:apply-templates select="comparPres"/>
					</div>
				</xsl:if>
				<xsl:if test="superPres">
					<div class="subsection">
						<h3><xsl:choose><xsl:when test="$metalang='ga'">SÁRCHÉIM</xsl:when><xsl:when test="$metalang='en'">SUPERLATIVE</xsl:when></xsl:choose></h3>
						<xsl:apply-templates select="superPres"/>
					</div>
				</xsl:if>
				<xsl:if test="abstractNoun">
					<div class="subsection">
						<h3><xsl:choose><xsl:when test="$metalang='ga'">AINM TEIBÍ</xsl:when><xsl:when test="$metalang='en'">ABSTRACT NOUN</xsl:when></xsl:choose></h3>
						<xsl:apply-templates select="abstractNoun"/>
					</div>
				</xsl:if>
			</div>
		</xsl:if>
	</xsl:template>
	<xsl:template match="adjective/sgNomMasc|adjective/sgGenMasc">
		<div class="line">
			<span class="value primary"><xsl:value-of select="text()"/></span>
			<xsl:text xml:space="preserve"> </xsl:text>
			<span class="label"><xsl:choose><xsl:when test="$metalang='ga'">(FIR.)</xsl:when><xsl:when test="$metalang='en'">(MASC.)</xsl:when></xsl:choose></span>
		</div>
	</xsl:template>
	<xsl:template match="adjective/sgNomFem|adjective/sgGenFem">
		<div class="line">
			<span class="value primary"><xsl:value-of select="text()"/></span>
			<xsl:text xml:space="preserve"> </xsl:text>
			<span class="label"><xsl:choose><xsl:when test="$metalang='ga'">(BAIN.)</xsl:when><xsl:when test="$metalang='en'">(FEM.)</xsl:when></xsl:choose></span>
		</div>
	</xsl:template>
	<xsl:template match="adjective/plNom">
		<xsl:variable name="pos" select="position()"/>
		<div class="block">
			<div class="line">
				<span class="value primary"><xsl:value-of select="text()"/></span>
				<xsl:text xml:space="preserve"> </xsl:text>
			</div>
			<xsl:variable name="slen" select="parent::adjective/plNomSlen[$pos]"/>
			<xsl:if test="$slen and $slen!=.">
			<div class="line">
				<span class="value primary"><xsl:value-of select="$slen/text()"/></span>
				<xsl:text xml:space="preserve"> </xsl:text>
				<span class="label"><xsl:choose><xsl:when test="$metalang='ga'">(CONSAIN CHAOLAITHE)</xsl:when><xsl:when test="$metalang='en'">(SLENDER CONSONANTS)</xsl:when></xsl:choose></span>
			</div>
			</xsl:if>
		</div>
	</xsl:template>
	<xsl:template match="adjective/plGenStrong">
		<div class="line">
			<span class="value primary"><xsl:value-of select="text()"/></span>
			<xsl:text xml:space="preserve"> </xsl:text>
			<span class="label"><xsl:choose><xsl:when test="$metalang='ga'">(TRÉANIOLRAÍ)</xsl:when><xsl:when test="$metalang='en'">(STRONG PLURALS)</xsl:when></xsl:choose></span>
		</div>
	</xsl:template>
	<xsl:template match="adjective/plGenWeak">
		<div class="line">
			<span class="value primary"><xsl:value-of select="text()"/></span>
			<xsl:text xml:space="preserve"> </xsl:text>
			<span class="label"><xsl:choose><xsl:when test="$metalang='ga'">(LAGIOLRAÍ)</xsl:when><xsl:when test="$metalang='en'">(WEAK PLURALS)</xsl:when></xsl:choose></span>
		</div>
	</xsl:template>
	<xsl:template match="adjective/comparPres">
		<xsl:variable name="pos" select="position()"/>
		<div class="block">
			<div class="line">
				<span class="value primary"><xsl:value-of select="text()"/></span>
			</div>
			<div class="line bulletted">
				<span class="bullet">▪</span>
				<span class="value"><xsl:value-of select="parent::adjective/comparPast[$pos]/text()"/></span>
			</div>
		</div>
	</xsl:template>
	<xsl:template match="adjective/superPres">
		<xsl:variable name="pos" select="position()"/>
		<div class="block">
			<div class="line">
				<span class="value primary"><xsl:value-of select="text()"/></span>
			</div>
			<div class="line bulletted">
				<span class="bullet">▪</span>
				<span class="value"><xsl:value-of select="parent::adjective/superPast[$pos]/text()"/></span>
			</div>
		</div>
	</xsl:template>
	<xsl:template match="adjective/abstractNoun">
		<xsl:variable name="pos" select="position()"/>
		<div class="block">
			<div class="line">
				<span class="value primary"><xsl:value-of select="text()"/></span>
			</div>
			<xsl:apply-templates select="parent::adjective/abstractNounExamples[$pos]/example"/>
		</div>
	</xsl:template>
	<xsl:template match="adjective/abstractNounExamples/example">
			<div class="line bulletted">
				<span class="bullet">▪</span>
				<span class="value"><xsl:value-of select="text()"/></span>
			</div>
	</xsl:template>

	<xsl:template match="preposition">
		<div class="header">
			<h1><xsl:value-of select="parent::Lemma/@lemma"/></h1>
			<div class="property">
				<div class="bullet">▪</div>
				<div class="value">
					<xsl:choose><xsl:when test="$metalang='ga'">RÉAMHFHOCAL</xsl:when><xsl:when test="$metalang='en'">PREPOSITION</xsl:when></xsl:choose>
				</div>
			</div>
		</div>
		<xsl:if test="persSg1 or persSg2 or persSg3Masc or persSg3Fem">
			<div class="section">
				<h2><xsl:choose><xsl:when test="$metalang='ga'">Uatha</xsl:when><xsl:when test="$metalang='en'">Singular</xsl:when></xsl:choose></h2>
				<xsl:if test="persSg1">
					<div class="block introd"><div class="intro">1</div><xsl:apply-templates select="persSg1"/></div>
				</xsl:if>
				<xsl:if test="persSg2">
					<div class="block introd"><div class="intro">2</div><xsl:apply-templates select="persSg2"/></div>
				</xsl:if>
				<xsl:if test="persSg3Masc|persSg3Fem">
					<div class="block introd"><div class="intro">3</div><xsl:apply-templates select="persSg3Masc|persSg3Fem"/></div>
				</xsl:if>
			</div>
		</xsl:if>
		<xsl:if test="persPl1 or persPl2 or persPl3">
			<div class="section">
				<h2><xsl:choose><xsl:when test="$metalang='ga'">Iolra</xsl:when><xsl:when test="$metalang='en'">Plural</xsl:when></xsl:choose></h2>
				<xsl:if test="persPl1">
					<div class="block introd"><div class="intro">1</div><xsl:apply-templates select="persPl1"/></div>
				</xsl:if>
				<xsl:if test="persPl2">
					<div class="block introd"><div class="intro">2</div><xsl:apply-templates select="persPl2"/></div>
				</xsl:if>
				<xsl:if test="persPl3">
					<div class="block introd"><div class="intro">3</div><xsl:apply-templates select="persPl3"/></div>
				</xsl:if>
			</div>
		</xsl:if>
	</xsl:template>
	<xsl:template match="preposition/persSg1|preposition/persSg2|preposition/persPl1|preposition/persPl2|preposition/persPl3">
		<div class="line">
			<span class="value primary"><xsl:value-of select="text()"/></span>
		</div>
	</xsl:template>
	<xsl:template match="preposition/persSg3Masc">
		<div class="line">
			<span class="value primary"><xsl:value-of select="text()"/></span>
			<xsl:text xml:space="preserve"> </xsl:text>
			<span class="label"><xsl:choose><xsl:when test="$metalang='ga'">(FIR.)</xsl:when><xsl:when test="$metalang='en'">(MASC.)</xsl:when></xsl:choose></span>
		</div>
	</xsl:template>
	<xsl:template match="preposition/persSg3Fem">
		<div class="line">
			<span class="value primary"><xsl:value-of select="text()"/></span>
			<xsl:text xml:space="preserve"> </xsl:text>
			<span class="label"><xsl:choose><xsl:when test="$metalang='ga'">(BAIN.)</xsl:when><xsl:when test="$metalang='en'">(FEM.)</xsl:when></xsl:choose></span>
		</div>
	</xsl:template>
	
	<xsl:template match="verb">
		<div class="header">
			<h1><xsl:value-of select="parent::Lemma/@lemma"/></h1>
			<div class="property">
				<div class="bullet">▪</div>
				<div class="value">
					<xsl:choose><xsl:when test="$metalang='ga'">BRIATHAR</xsl:when><xsl:when test="$metalang='en'">VERB</xsl:when></xsl:choose>
				</div>
			</div>
		</div>
		<xsl:if test="vn">
			<div class="subsection">
				<h3><xsl:choose><xsl:when test="$metalang='ga'">AINM BRIATHARTHA</xsl:when><xsl:when test="$metalang='en'">VERBAL NOUN</xsl:when></xsl:choose></h3>
				<div class="block">
					<xsl:apply-templates select="vn"/>
				</div>
			</div>
		</xsl:if>
		<xsl:if test="va">
			<div class="subsection">
				<h3><xsl:choose><xsl:when test="$metalang='ga'">AIDIACHT BHRIATHARTHA</xsl:when><xsl:when test="$metalang='en'">VERBAL ADJECTIVE</xsl:when></xsl:choose></h3>
				<div class="block">
					<xsl:apply-templates select="va"/>
				</div>
			</div>
		</xsl:if>
		<div class="selector">
			<select id="selector" onchange="pick(this.value)">
				<xsl:apply-templates select="past|present|presentConti|future|condi|pastConti|imper|subj" mode="list"/>
			</select>
		</div>
		<script>
			<![CDATA[
				function pick(val) {
					if(document.getElementById("past")) document.getElementById("past").style.display="none";
					if(document.getElementById("present")) document.getElementById("present").style.display="none";
					if(document.getElementById("presentConti")) document.getElementById("presentConti").style.display="none";
					if(document.getElementById("future")) document.getElementById("future").style.display="none";
					if(document.getElementById("condi")) document.getElementById("condi").style.display="none";
					if(document.getElementById("pastConti")) document.getElementById("pastConti").style.display="none";
					if(document.getElementById("imper")) document.getElementById("imper").style.display="none";
					if(document.getElementById("subj")) document.getElementById("subj").style.display="none";
					if(document.getElementById(val)) document.getElementById(val).style.display="";
				}
			]]>
		</script>
		<xsl:apply-templates select="past|present|presentConti|future|condi|pastConti|imper|subj" mode="data"/>
			<script>
				<![CDATA[
					pick('past'); document.getElementById('selector').selectedIndex=0;
				]]>
			</script>
		</xsl:template>
	<xsl:template match="verb/vn|verb/va">
		<div class="line"><span class="value primary"><xsl:value-of select="text()"/></span></div>
	</xsl:template>
	<xsl:template match="verb/past|verb/present|verb/presentConti|verb/future|verb/condi|verb/pastConti|verb/imper|verb/subj" mode="list">
		<xsl:element name="option">
			<xsl:attribute name="value"><xsl:value-of select="name()"/></xsl:attribute>
			<xsl:choose>
				<xsl:when test="name()='past'"><xsl:choose><xsl:when test="$metalang='ga'">Aimsir chaite</xsl:when><xsl:when test="$metalang='en'">Past</xsl:when></xsl:choose></xsl:when>
				<xsl:when test="name()='present'"><xsl:choose><xsl:when test="$metalang='ga'">Aimsir láithreach</xsl:when><xsl:when test="$metalang='en'">Present</xsl:when></xsl:choose></xsl:when>
				<xsl:when test="name()='presentConti'"><xsl:choose><xsl:when test="$metalang='ga'">Aimsir ghnáthláithreach</xsl:when><xsl:when test="$metalang='en'">Present habitual</xsl:when></xsl:choose></xsl:when>
				<xsl:when test="name()='future'"><xsl:choose><xsl:when test="$metalang='ga'">Aimsir fháistineach</xsl:when><xsl:when test="$metalang='en'">Future</xsl:when></xsl:choose></xsl:when>
				<xsl:when test="name()='condi'"><xsl:choose><xsl:when test="$metalang='ga'">Modh coinníollach</xsl:when><xsl:when test="$metalang='en'">Conditional</xsl:when></xsl:choose></xsl:when>
				<xsl:when test="name()='pastConti'"><xsl:choose><xsl:when test="$metalang='ga'">Aimsir ghnáthchaite</xsl:when><xsl:when test="$metalang='en'">Past habitual</xsl:when></xsl:choose></xsl:when>
				<xsl:when test="name()='imper'"><xsl:choose><xsl:when test="$metalang='ga'">Modh ordaitheach</xsl:when><xsl:when test="$metalang='en'">Imperative</xsl:when></xsl:choose></xsl:when>
				<xsl:when test="name()='subj'"><xsl:choose><xsl:when test="$metalang='ga'">Modh foshuiteach</xsl:when><xsl:when test="$metalang='en'">Subjunctive</xsl:when></xsl:choose></xsl:when>
			</xsl:choose>
		</xsl:element>
	</xsl:template>
	<xsl:template match="verb/past|verb/present|verb/presentConti|verb/future|verb/condi|verb/pastConti|verb/imper|verb/subj" mode="data">
		<div class="section">
			<!--
			<h2 class="clickable" onclick="var b=this.parentNode.childNodes[1]; if(b.style.display=='none') b.style.display='block'; else b.style.display='none';">
				<span class="arrow">▼</span>
				<xsl:choose>
					<xsl:when test="name()='past'"><xsl:choose><xsl:when test="$metalang='ga'">Aimsir chaite</xsl:when><xsl:when test="$metalang='en'">Past</xsl:when></xsl:choose></xsl:when>
					<xsl:when test="name()='present'"><xsl:choose><xsl:when test="$metalang='ga'">Aimsir láithreach</xsl:when><xsl:when test="$metalang='en'">Present</xsl:when></xsl:choose></xsl:when>
					<xsl:when test="name()='presentConti'"><xsl:choose><xsl:when test="$metalang='ga'">Aimsir ghnáthláithreach</xsl:when><xsl:when test="$metalang='en'">Present habitual</xsl:when></xsl:choose></xsl:when>
					<xsl:when test="name()='future'"><xsl:choose><xsl:when test="$metalang='ga'">Aimsir fháistineach</xsl:when><xsl:when test="$metalang='en'">Future</xsl:when></xsl:choose></xsl:when>
					<xsl:when test="name()='condi'"><xsl:choose><xsl:when test="$metalang='ga'">Modh coinníollach</xsl:when><xsl:when test="$metalang='en'">Conditional</xsl:when></xsl:choose></xsl:when>
					<xsl:when test="name()='pastConti'"><xsl:choose><xsl:when test="$metalang='ga'">Aimsir ghnáthchaite</xsl:when><xsl:when test="$metalang='en'">Past habitual</xsl:when></xsl:choose></xsl:when>
					<xsl:when test="name()='imper'"><xsl:choose><xsl:when test="$metalang='ga'">Modh ordaitheach</xsl:when><xsl:when test="$metalang='en'">Imperative</xsl:when></xsl:choose></xsl:when>
					<xsl:when test="name()='subj'"><xsl:choose><xsl:when test="$metalang='ga'">Modh foshuiteach</xsl:when><xsl:when test="$metalang='en'">Subjunctive</xsl:when></xsl:choose></xsl:when>
				</xsl:choose>
			</h2>
			-->
			<xsl:element name="div">
				<xsl:attribute name="class">body</xsl:attribute>
				<xsl:attribute name="id"><xsl:value-of select="name()"/></xsl:attribute>
				<xsl:if test="sg1 or sg2 or sg3Masc or sg3Fem">
					<div class="subsection">
						<h3><xsl:choose><xsl:when test="$metalang='ga'">UATHA</xsl:when><xsl:when test="$metalang='en'">SINGULAR</xsl:when></xsl:choose></h3>
						<xsl:apply-templates select="sg1/pos"/>
						<xsl:apply-templates select="sg2/pos"/>
						<xsl:apply-templates select="sg3Masc/pos"/>
						<xsl:apply-templates select="sg3Fem/pos"/>
					</div>
				</xsl:if>
				<xsl:if test="pl1 or pl2 or pl3">
					<div class="subsection">
						<h3><xsl:choose><xsl:when test="$metalang='ga'">IOLRA</xsl:when><xsl:when test="$metalang='en'">PLURAL</xsl:when></xsl:choose></h3>
						<xsl:apply-templates select="pl1/pos"/>
						<xsl:apply-templates select="pl2/pos"/>
						<xsl:apply-templates select="pl3/pos"/>
					</div>
				</xsl:if>
				<xsl:if test="auto">
					<div class="subsection">
						<h3><xsl:choose><xsl:when test="$metalang='ga'">SAORBHRIATHAR</xsl:when><xsl:when test="$metalang='en'">PASSIVE</xsl:when></xsl:choose></h3>
						<xsl:apply-templates select="auto/pos"/>
					</div>
				</xsl:if>
			</xsl:element>
		</div>
	</xsl:template>
	<xsl:template match="verb/*/sg1/pos | verb/*/sg2/pos | verb/*/sg3Masc/pos | verb/*/sg3Fem/pos | verb/*/pl1/pos | verb/*/pl2/pos | verb/*/pl3/pos | verb/*/auto/pos">
		<xsl:variable name="pos" select="position()"/>
		<div class="block introd">
			<xsl:if test="$pos=1 and name(parent::*)!='sg3Fem' and name(parent::*)!='auto'">
				<div class="intro">
					<xsl:choose>
						<xsl:when test="name(parent::*)='sg1' or name(parent::*)='pl1'">1</xsl:when>
						<xsl:when test="name(parent::*)='sg2' or name(parent::*)='pl2'">2</xsl:when>
						<xsl:when test="name(parent::*)='sg3Masc' or name(parent::*)='pl3'">3</xsl:when>
					</xsl:choose>
				</div>
			</xsl:if>
			<div class="line">
				<span class="value primary"><xsl:value-of select="text()"/></span>
				<xsl:if test="name(parent::*)='sg3Masc'">
					<xsl:text xml:space="preserve"> </xsl:text>
					<span class="label"><xsl:choose><xsl:when test="$metalang='ga'">(FIR.)</xsl:when><xsl:when test="$metalang='en'">(MASC.)</xsl:when></xsl:choose></span>
				</xsl:if>
				<xsl:if test="name(parent::*)='sg3Fem'">
					<xsl:text xml:space="preserve"> </xsl:text>
					<span class="label"><xsl:choose><xsl:when test="$metalang='ga'">(BAIN.)</xsl:when><xsl:when test="$metalang='en'">(FEM.)</xsl:when></xsl:choose></span>
				</xsl:if>
			</div>
			<xsl:if test="parent::*/quest[$pos]">
				<div class="line bulletted">
					<span class="bullet">▪</span>
					<span class="value"><xsl:value-of select="parent::*/quest[$pos]/text()"/></span>
				</div>
			</xsl:if>
			<xsl:if test="parent::*/neg[$pos]">
				<div class="line bulletted">
					<span class="bullet">▪</span>
					<span class="value"><xsl:value-of select="parent::*/neg[$pos]/text()"/></span>
				</div>
			</xsl:if>
		</div>
	</xsl:template>
	
</xsl:stylesheet>