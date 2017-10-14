select F_OBJECT_ID, [Длина], [Ширина], [Наименование]
from	(select atr.F_NAME, objAtr.F_STRING_VALUE, temp.F_OBJECT_ID
		from
			 (select F_OBJECT_ID from IMS_OBJECTS where F_OBJECT_TYPE = 1001)
		as temp, IMS_OBJECT_ATTRS objAtr, IMS_ATTRIBUTES atr 
		where temp.F_OBJECT_ID = objAtr.F_OBJECT_ID
		and objAtr.F_ATTRIBUTE_ID = atr.F_ATTRIBUTE_ID) as TEMPORAR
pivot
(
	min(TEMPORAR.F_STRING_VALUE) for TEMPORAR.F_NAME IN ( [Длина],[Ширина],[Наименование])
)
AS TESTpIVOT
------------------------------------------------------------------------------------------


	SELECT distinct(F_NAME) FROM IMS_ATTRIBUTES ATR
    RIGHT JOIN
	(SELECT F_ATTRIBUTE_ID FROM IMS_OBJECT_ATTRS objATTR
	RIGHT JOIN
		(SELECT F_OBJECT_ID FROM IMS_OBJECTS WHERE F_OBJECT_TYPE = 1001) AS tempOBJ
		    ON objATTR.F_OBJECT_ID = tempOBJ.F_OBJECT_ID) AS ATRid
		    ON ATR.F_ATTRIBUTE_ID = ATRid.F_ATTRIBUTE_ID