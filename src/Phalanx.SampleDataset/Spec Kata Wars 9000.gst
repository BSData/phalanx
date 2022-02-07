<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<gameSystem id="bc3d-3e2a-b2b3-fef7" name="Spec Kata Wars 9000" revision="2" battleScribeVersion="2.03" authorName="Rylok" authorContact="@rylok3" xmlns="http://www.battlescribe.net/schema/gameSystemSchema">
  <publications>
    <publication id="2ea4-57c2-c067-7215" name="Spec Kata Wars 9000 Core Rulebook" shortName="corebook" publisher="Rylok Enterprises, Inc, Esq, Ltd." publicationDate="2021" publisherUrl=""/>
  </publications>
  <costTypes>
    <costType id="a66b-3b27-93dd-68ce" name="Points" defaultCostLimit="-1.0" hidden="false"/>
    <costType id="f129-c6bb-02e0-923e" name="Fuel" defaultCostLimit="-1.0" hidden="false"/>
  </costTypes>
  <profileTypes>
    <profileType id="5f4a-2d9d-d324-0d8b" name="Vehicle">
      <characteristicTypes>
        <characteristicType id="0505-be28-4cc1-c30f" name="Hit Points"/>
        <characteristicType id="2a19-89bb-b447-44bf" name="Speed"/>
        <characteristicType id="db87-a58d-19f7-6a5f" name="Defense"/>
        <characteristicType id="6b85-49c9-f4e2-e51a" name="Style"/>
        <characteristicType id="ec2d-c0b5-a1fd-3cf4" name="Seats"/>
      </characteristicTypes>
    </profileType>
    <profileType id="8c47-af7e-49ae-907b" name="Abilities">
      <characteristicTypes>
        <characteristicType id="c898-4f9c-d2b9-160b" name="Description"/>
      </characteristicTypes>
    </profileType>
    <profileType id="6cfe-fe74-1a3d-6d17" name="Weapon">
      <characteristicTypes>
        <characteristicType id="5fce-07c3-4ac4-725c" name="Type"/>
        <characteristicType id="e9b6-5ffa-ead8-7da4" name="Damage"/>
        <characteristicType id="b3d9-5892-fc1d-cd47" name="Attack Speed"/>
        <characteristicType id="9972-d522-3bdb-4501" name="Parry"/>
        <characteristicType id="b352-1258-db4f-2de1" name="Range"/>
      </characteristicTypes>
    </profileType>
    <profileType id="a6e8-411d-f765-546a" name="Character">
      <characteristicTypes>
        <characteristicType id="1744-8129-95a0-536e" name="Attack Power"/>
        <characteristicType id="9aab-d556-7966-cc13" name="Evasion"/>
        <characteristicType id="8b10-f201-846f-ab9f" name="Driving Skill"/>
        <characteristicType id="3f4b-aa15-3364-134b" name="Navigation Skill"/>
      </characteristicTypes>
    </profileType>
  </profileTypes>
  <categoryEntries>
    <categoryEntry id="1f30-56d0-ac83-1212" name="Lead Vehicles" publicationId="2ea4-57c2-c067-7215" hidden="false">
      <constraints>
        <constraint field="selections" scope="roster" value="1.0" percentValue="false" shared="true" includeChildSelections="false" includeChildForces="false" id="658d-ed15-6c44-cb72" type="min"/>
        <constraint field="selections" scope="force" value="1.0" percentValue="false" shared="true" includeChildSelections="true" includeChildForces="true" id="54f3-cd22-5b50-a6d5" type="max"/>
      </constraints>
    </categoryEntry>
    <categoryEntry id="2c8f-63d4-2b0c-01ad" name="Support Vehicles" publicationId="2ea4-57c2-c067-7215" hidden="false"/>
    <categoryEntry id="c735-947c-993c-e57d" name="Attack Vehicles" publicationId="2ea4-57c2-c067-7215" hidden="false">
      <constraints>
        <constraint field="selections" scope="roster" value="1.0" percentValue="false" shared="true" includeChildSelections="false" includeChildForces="false" id="427d-5390-45fc-ea11" type="min"/>
      </constraints>
    </categoryEntry>
    <categoryEntry id="10c7-1327-32a1-6bee" name="Game Options" publicationId="2ea4-57c2-c067-7215" hidden="false">
      <constraints>
        <constraint field="selections" scope="roster" value="1.0" percentValue="false" shared="true" includeChildSelections="false" includeChildForces="false" id="4922-b715-1460-89ee" type="max"/>
      </constraints>
    </categoryEntry>
  </categoryEntries>
  <forceEntries>
    <forceEntry id="f92c-51d5-ff55-166c" name="Convoy" publicationId="2ea4-57c2-c067-7215" hidden="false">
      <modifiers>
        <modifier type="set" field="4899-26df-4d1c-e095" value="100.0">
          <conditions>
            <condition field="selections" scope="10c7-1327-32a1-6bee" value="1.0" percentValue="false" shared="true" includeChildSelections="true" includeChildForces="false" childId="9d78-85fe-4de7-2099" type="equalTo"/>
          </conditions>
        </modifier>
        <modifier type="set" field="4899-26df-4d1c-e095" value="200.0">
          <conditions>
            <condition field="selections" scope="10c7-1327-32a1-6bee" value="1.0" percentValue="false" shared="true" includeChildSelections="true" includeChildForces="false" childId="d31f-2b03-7d58-4df1" type="equalTo"/>
          </conditions>
        </modifier>
      </modifiers>
      <constraints>
        <constraint field="forces" scope="roster" value="1.0" percentValue="false" shared="true" includeChildSelections="true" includeChildForces="true" id="f08a-7851-68dc-35cb" type="max"/>
        <constraint field="a66b-3b27-93dd-68ce" scope="roster" value="-1.0" percentValue="false" shared="true" includeChildSelections="false" includeChildForces="true" id="4899-26df-4d1c-e095" type="max"/>
      </constraints>
      <categoryLinks>
        <categoryLink id="7477-7281-bfcf-096a" name="Game Options" publicationId="2ea4-57c2-c067-7215" hidden="false" targetId="10c7-1327-32a1-6bee" primary="false"/>
        <categoryLink id="e224-408f-6a89-f0f2" name="Attack Vehicles" publicationId="2ea4-57c2-c067-7215" hidden="false" targetId="c735-947c-993c-e57d" primary="false"/>
        <categoryLink id="5aff-4f5d-eb36-e563" name="Support Vehicles" publicationId="2ea4-57c2-c067-7215" hidden="false" targetId="2c8f-63d4-2b0c-01ad" primary="false"/>
        <categoryLink id="63ea-7954-9af1-b857" name="Lead Vehicles" publicationId="2ea4-57c2-c067-7215" hidden="false" targetId="1f30-56d0-ac83-1212" primary="false"/>
      </categoryLinks>
    </forceEntry>
  </forceEntries>
  <entryLinks>
    <entryLink id="f0bc-6218-0276-0632" name="Game Size" hidden="false" collective="false" import="true" targetId="a23f-f979-d5f7-34d5" type="selectionEntry">
      <categoryLinks>
        <categoryLink id="3162-f1fa-a269-707d" name="New CategoryLink" hidden="false" targetId="10c7-1327-32a1-6bee" primary="true"/>
      </categoryLinks>
    </entryLink>
  </entryLinks>
  <sharedSelectionEntries>
    <selectionEntry id="a23f-f979-d5f7-34d5" name="Game Size" publicationId="2ea4-57c2-c067-7215" hidden="false" collective="false" import="true" type="upgrade">
      <constraints>
        <constraint field="selections" scope="roster" value="1.0" percentValue="false" shared="true" includeChildSelections="true" includeChildForces="true" id="d064-7a0a-7c4a-d9bb" type="min"/>
        <constraint field="selections" scope="roster" value="1.0" percentValue="false" shared="true" includeChildSelections="true" includeChildForces="true" id="65a5-aef8-ecdc-38af" type="max"/>
      </constraints>
      <selectionEntryGroups>
        <selectionEntryGroup id="c2b2-23d3-76b2-6d2d" name="Game Size" publicationId="2ea4-57c2-c067-7215" hidden="false" collective="false" import="true" defaultSelectionEntryId="9d78-85fe-4de7-2099">
          <constraints>
            <constraint field="selections" scope="parent" value="1.0" percentValue="false" shared="true" includeChildSelections="false" includeChildForces="false" id="9255-4da3-05f1-e44a" type="min"/>
            <constraint field="selections" scope="parent" value="1.0" percentValue="false" shared="true" includeChildSelections="false" includeChildForces="false" id="cda1-52e7-8210-6883" type="max"/>
          </constraints>
          <selectionEntries>
            <selectionEntry id="9d78-85fe-4de7-2099" name="100 Points" hidden="false" collective="false" import="true" type="upgrade">
              <costs>
                <cost name="Fuel" typeId="f129-c6bb-02e0-923e" value="0.0"/>
                <cost name="Points" typeId="a66b-3b27-93dd-68ce" value="0.0"/>
              </costs>
            </selectionEntry>
            <selectionEntry id="d31f-2b03-7d58-4df1" name="200 Points" hidden="false" collective="false" import="true" type="upgrade">
              <costs>
                <cost name="Fuel" typeId="f129-c6bb-02e0-923e" value="0.0"/>
                <cost name="Points" typeId="a66b-3b27-93dd-68ce" value="0.0"/>
              </costs>
            </selectionEntry>
          </selectionEntries>
        </selectionEntryGroup>
      </selectionEntryGroups>
      <costs>
        <cost name="Fuel" typeId="f129-c6bb-02e0-923e" value="0.0"/>
        <cost name="Points" typeId="a66b-3b27-93dd-68ce" value="0.0"/>
      </costs>
    </selectionEntry>
  </sharedSelectionEntries>
  <sharedProfiles>
    <profile id="f4c2-9be9-0465-eed9" name="Shotgun" publicationId="2ea4-57c2-c067-7215" hidden="false" typeId="6cfe-fe74-1a3d-6d17" typeName="Weapon">
      <characteristics>
        <characteristic name="Type" typeId="5fce-07c3-4ac4-725c">Gun</characteristic>
        <characteristic name="Damage" typeId="e9b6-5ffa-ead8-7da4">5</characteristic>
        <characteristic name="Attack Speed" typeId="b3d9-5892-fc1d-cd47">1</characteristic>
        <characteristic name="Parry" typeId="9972-d522-3bdb-4501">1</characteristic>
        <characteristic name="Range" typeId="b352-1258-db4f-2de1">3</characteristic>
      </characteristics>
    </profile>
    <profile id="efc0-b211-074b-a9ab" name="Katana" publicationId="2ea4-57c2-c067-7215" hidden="false" typeId="6cfe-fe74-1a3d-6d17" typeName="Weapon">
      <characteristics>
        <characteristic name="Type" typeId="5fce-07c3-4ac4-725c">Sword</characteristic>
        <characteristic name="Damage" typeId="e9b6-5ffa-ead8-7da4">3</characteristic>
        <characteristic name="Attack Speed" typeId="b3d9-5892-fc1d-cd47">5</characteristic>
        <characteristic name="Parry" typeId="9972-d522-3bdb-4501">4</characteristic>
        <characteristic name="Range" typeId="b352-1258-db4f-2de1">1</characteristic>
      </characteristics>
    </profile>
    <profile id="c092-75d7-d592-9cde" name="Shriek" publicationId="2ea4-57c2-c067-7215" page="1" hidden="false" typeId="8c47-af7e-49ae-907b" typeName="Abilities">
      <characteristics>
        <characteristic name="Description" typeId="c898-4f9c-d2b9-160b">A character on this vehicle can shriek loudly, and when they do so, it hurts your ears.</characteristic>
      </characteristics>
    </profile>
    <profile id="1917-10ff-b05e-cf43" name="Spit" publicationId="2ea4-57c2-c067-7215" hidden="false" typeId="8c47-af7e-49ae-907b" typeName="Abilities">
      <characteristics>
        <characteristic name="Description" typeId="c898-4f9c-d2b9-160b">A character on this vehicle can spit a long distance. Gross.</characteristic>
      </characteristics>
    </profile>
    <profile id="536a-04b6-ca02-d522" name="Turbo Boost" publicationId="2ea4-57c2-c067-7215" hidden="false" typeId="8c47-af7e-49ae-907b" typeName="Abilities">
      <characteristics>
        <characteristic name="Description" typeId="c898-4f9c-d2b9-160b">This vehicle can turn on it&apos;s turboboost ability. If it does so, it can move 3&quot; in a random direction. Wheeeeeeee!</characteristic>
      </characteristics>
    </profile>
  </sharedProfiles>
</gameSystem>